using TbspRpgLib.Events;
using TbspRpgLib.Settings;

namespace TbspRpgLib.EventProcessors {
    public abstract class NewGameEventProcessor : EventProcessor {
        public NewGameEventProcessor(IEventStoreSettings eventStoreSettings, IDatabaseSettings databaseSettings) : 
            base("game", eventStoreSettings, databaseSettings) {

        }

        protected override string GetEventName()
        {
            return Event.NEW_GAME_EVENT_TYPE;
        }
    }
}