using System;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Hosting;

using TbspRpgLib.Repositories;
using TbspRpgLib.Entities;
using TbspRpgLib.Events;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Settings;
using TbspRpgLib.Services;

namespace TbspRpgLib.EventProcessors {
    public abstract class EventProcessor : IHostedService, IDisposable
    {
        private bool _stopping;
        private Task _backgroundTask;

        private IEventService _eventService;
        private IAggregateService _aggregateService;
        protected IServiceService _serviceService;
        protected Service _service;
        //protected EventType _eventType;
        // protected ulong _startPosition;
        protected List<EventType> _eventTypes;
        protected Dictionary<Guid, ulong> _startPositions;

        public EventProcessor(IEventStoreSettings eventStoreSettings) {
            //used to retrieve events
            _eventService = new EventService(eventStoreSettings);
            _aggregateService = new AggregateService(_eventService);
            _eventTypes = new List<EventType>();
            _startPositions = new Dictionary<Guid, ulong>();
        }

        protected async Task InitializeStartPosition(ServiceTrackingContext serviceTrackingContext) {
            //context used to update the status of the service reading events
            var strepo = new ServiceTrackingRepository(serviceTrackingContext);
            var serviceTrackingService = new ServiceTrackingService(strepo);
            //_startPosition = await serviceTrackingService.GetPosition(_eventType.Id);
            foreach(var eventType in _eventTypes) {
                var position = await serviceTrackingService.GetPosition(eventType.Id);
                _startPositions.Add(eventType.Id, position);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Event Processor is starting.");
            Console.WriteLine($"Watching for {_eventTypes.Count} event types: ");
            foreach(var eventType in _eventTypes) {
                Console.WriteLine($" - {eventType.TypeName}");
            }

            PreTask();
            _backgroundTask = BackgroundTask();
            return Task.CompletedTask;
        }

        protected void PreTask() {
            while(_startPositions.Count < _eventTypes.Count) {
                Console.WriteLine("waiting for start positions to populate");
                Thread.Sleep(500);
            }

            foreach(var eventType in _eventTypes) {
                _aggregateService.SubscribeByType(
                    eventType.TypeName,
                    HandleEvent,
                    _startPositions[eventType.Id]
                );
            }
            // _aggregateService.SubscribeByType(
            //     GetEventName(),
            //     HandleEvent,
            //     _startPosition
            // );
        }

        protected abstract Task HandleEvent(Aggregate aggregate, Event evnt);

        private async Task BackgroundTask() {
            while (!_stopping)
            {
                await Task.Delay(TimeSpan.FromSeconds(4));
                //Console.WriteLine("Event Processor is Alive.");
            }

            Console.WriteLine("Event Processor background task is stopping.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Event Processor is stopping.");
            _stopping = true;
            if (_backgroundTask != null)
            {
                await _backgroundTask;
            }
        }

        public void Dispose()
        {
            Console.WriteLine("Event Processor is disposing.");
        }
    }
}
