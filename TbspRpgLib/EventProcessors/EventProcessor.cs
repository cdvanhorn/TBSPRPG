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
        protected ServiceTrackingContext _serviceTrackingContext;

        public EventProcessor(string serviceName, IEventStoreSettings eventStoreSettings, ServiceTrackingContext serviceTrackingContext) {
            //used to retrieve events
            _eventService = new EventService(eventStoreSettings);
            _aggregateService = new AggregateService(_eventService);

            //ServiceService and ServiceRepository used to get service information
            var serviceRepository = new ServiceRepository();
            _serviceService = new ServiceService(serviceRepository);

            //the service using this processor
            _service = _serviceService.GetServiceByName(serviceName);

            //context used to update the status of the service reading events
            _serviceTrackingContext = serviceTrackingContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Event Processor is starting.");
            PreTask();
            _backgroundTask = BackgroundTask();
            return Task.CompletedTask;
        }

        protected void PreTask() {
            _aggregateService.SubscribeByType(
                GetEventName(),
                (aggregate, eventId, position) => {
                    HandleEvent(aggregate, eventId, position);
                },
                _service.GetStartPosition(GetEventName())
            );
        }

        protected abstract void HandleEvent(Aggregate aggregate, string eventId, ulong position);

        protected void UpdatePosition(ulong position) {
            // if(_service.UpdatePosition(position, GetEventName()))
            //     _serviceService.UpdateService(_service, GetEventName());
            //this will be a database call
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
