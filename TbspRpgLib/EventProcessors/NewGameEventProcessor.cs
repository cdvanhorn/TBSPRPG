using TbspRpgLib.Events;
using TbspRpgLib.Settings;
using TbspRpgLib.Repositories;
using TbspRpgLib.Services;

namespace TbspRpgLib.EventProcessors {
    public abstract class NewGameEventProcessor : EventProcessor {
        public NewGameEventProcessor(string serviceName, IEventStoreSettings eventStoreSettings, ServiceTrackingContext serviceTrackingContext) : 
                base(eventStoreSettings, serviceTrackingContext) {
            //ServiceService and ServiceRepository used to get service information
            var serviceRepository = new ServiceRepository();
            _serviceService = new ServiceService(serviceRepository);

            //the service using this processor
            _service = _serviceService.GetServiceByName(serviceName);
            _eventType = _serviceService.GetEventTypeByName(Event.NEW_GAME_EVENT_TYPE);
        }

        protected override string GetEventName()
        {
            return _eventType.TypeName;
        }
    }
}
