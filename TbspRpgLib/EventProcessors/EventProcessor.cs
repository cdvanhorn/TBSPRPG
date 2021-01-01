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
        private Task<Service> _serviceTask;
        protected Service _service;

        public EventProcessor(string serviceName, IEventStoreSettings eventStoreSettings, IDatabaseSettings databaseSettings) {
            _eventService = new EventService(eventStoreSettings);
            _aggregateService = new AggregateService(_eventService);
            var serviceRepository = new ServiceRepository(databaseSettings);
            _serviceService = new ServiceService(serviceRepository);
            _serviceTask = _serviceService.GetServiceByName(serviceName);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Console.WriteLine("Event Processor is starting.");
            PreTask();
            _backgroundTask = BackgroundTask();
            return Task.CompletedTask;
        }

        protected async void PreTask() {
            //get where we need to start reading from
            _service = await _serviceTask;

            _aggregateService.SubscribeByType(
                GetEventName(),
                (aggregate, eventId, position) => {
                    HandleEvent(aggregate, eventId, position);
                },
                _service.EventPrefix,
                _service.GetStartPosition(GetEventName())
            );
        }

        protected abstract void HandleEvent(Aggregate aggregate, string eventId, ulong position);

        protected void UpdatePosition(ulong position) {
            if(_service.UpdatePosition(position, GetEventName()))
                _serviceService.UpdateService(_service, GetEventName());
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
