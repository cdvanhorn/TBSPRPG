using TbspRpgLib.Events;
using TbspRpgLib.Settings;
using TbspRpgLib.Repositories;

namespace TbspRpgLib.EventProcessors {
    public abstract class NewGameEventProcessor : EventProcessor {
        public NewGameEventProcessor(string serviceName, IEventStoreSettings eventStoreSettings, ServiceTrackingContext serviceTrackingContext) : 
            base(serviceName, eventStoreSettings, serviceTrackingContext) {

        }

        protected override string GetEventName()
        {
            return Event.NEW_GAME_EVENT_TYPE;
        }
    }
}
