using TbspRpgLib.Events;
using TbspRpgLib.Settings;

namespace TbspRpgLib.EventProcessors {
    public abstract class NewGameEventProcessor : EventProcessorService {
        public NewGameEventProcessor(string serviceName, IEventStoreSettings eventStoreSettings) : 
                base(serviceName, eventStoreSettings) {      
            _eventType = _serviceService.GetEventTypeByName(Event.NEW_GAME_EVENT_TYPE);
        }

        protected override string GetEventName()
        {
            return _eventType.TypeName;
        }
    }
}
