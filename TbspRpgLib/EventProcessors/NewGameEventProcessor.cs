using TbspRpgLib.Events;
using TbspRpgLib.Settings;

namespace TbspRpgLib.EventProcessors {
    public abstract class NewGameEventProcessor : EventProcessor {
        public NewGameEventProcessor(string serviceName, IEventStoreSettings eventStoreSettings, IDatabaseSettings databaseSettings) : 
            base(serviceName, eventStoreSettings, databaseSettings) {

        }

        protected override string GetEventName()
        {
            return Event.NEW_GAME_EVENT_TYPE;
        }
    }
}
