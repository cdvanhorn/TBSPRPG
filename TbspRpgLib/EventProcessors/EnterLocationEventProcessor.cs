using TbspRpgLib.Settings;
using TbspRpgLib.Events;

namespace TbspRpgLib.EventProcessors {
    public abstract class EnterLocationEventProcessor : EventProcessorService {
        public EnterLocationEventProcessor(string serviceName, IEventStoreSettings eventStoreSettings) : 
                base(serviceName, eventStoreSettings) {      
            _eventTypes.Add(_serviceService.GetEventTypeByName(Event.ENTER_LOCATION_EVENT_TYPE));
        }
    }
}