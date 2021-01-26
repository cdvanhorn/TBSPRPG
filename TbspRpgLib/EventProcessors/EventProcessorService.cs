using TbspRpgLib.Settings;
using TbspRpgLib.Services;
using TbspRpgLib.Repositories;

namespace TbspRpgLib.EventProcessors {
    public abstract class EventProcessorService : EventProcessor {
        public EventProcessorService(string serviceName, IEventStoreSettings eventStoreSettings) : base(eventStoreSettings) {
            //ServiceService and ServiceRepository used to get service information
            var serviceRepository = new ServiceRepository();
            _serviceService = new ServiceService(serviceRepository);

            //the service using this processor
            _service = _serviceService.GetServiceByName(serviceName);
        }
    }
}