using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        protected EventType _eventType;
        protected ServiceTrackingService _serviceTrackingService;

        public EventProcessor(IEventStoreSettings eventStoreSettings) {
            //used to retrieve events
            _eventService = new EventService(eventStoreSettings);
        }

        protected void InitializeServices(ServiceTrackingContext serviceTrackingContext) {
            //context used to update the status of the service reading events
            var strepo = new ServiceTrackingRepository(serviceTrackingContext);
            _serviceTrackingService = new ServiceTrackingService(strepo);
            //aggregate service used to subscribe to events
            _aggregateService = new AggregateService(_eventService, _serviceTrackingService);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Event Processor is starting.");
            PreTask();
            _backgroundTask = BackgroundTask();
            return Task.CompletedTask;
        }

        protected async void PreTask() {
            ulong position = await _serviceTrackingService.GetPosition(
                _service.Id, _eventType.Id);

            _aggregateService.SubscribeByType(
                GetEventName(),
                (aggregate, eventId, position) => {
                    HandleEvent(aggregate, eventId, position);
                },
                _service.Id,
                position
            );
        }

        protected abstract void HandleEvent(Aggregate aggregate, string eventId, ulong position);

        protected async Task UpdatePosition(ulong position) {
            await _serviceTrackingService.UpdatePosition(_service.Id, _eventType.Id, position);
        }

        protected async Task AddEventTracked(string eventId) {
            await _serviceTrackingService.EventProcessed(_service.Id, new Guid(eventId));
        }

        protected abstract string GetEventName();

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
