using System;
using System.Text;
using System.Text.Json;

using EventStore.Client;

using TbspRpgLib.Aggregates;

namespace TbspRpgLib.Events
{
    public abstract class Event {
        public const string NEW_GAME_EVENT_TYPE = "new_game";
        public const string ENTER_LOCATION_EVENT_TYPE = "enter_location";

        public Event() {
            EventId = Guid.NewGuid();
            EventStoreUuid = Uuid.FromGuid(EventId);
        }

        public Guid EventId { get; set; }

        public Uuid EventStoreUuid { get; set; }

        public ulong GlobalPosition { get; set; }

        public ulong StreamPosition { get; set; }

        public string Type { get; set; }

        protected abstract EventContent GetData();

        protected abstract void SetData(string jsonString);

        public abstract string GetDataJson();

        public abstract string GetStreamId();

        public abstract void UpdateAggregate(Aggregate agg);

        public static Event FromEventStoreEvent(ResolvedEvent resolvedEvent) {
            Event evnt;
            if(resolvedEvent.Event.EventType.StartsWith('$')) {
                return null;
            }
            
            //I'm not really happy with this part
            switch(resolvedEvent.Event.EventType) {
                case NEW_GAME_EVENT_TYPE:
                    evnt = new NewGameEvent();
                    break;
                case ENTER_LOCATION_EVENT_TYPE:
                    evnt = new EnterLocationEvent();
                    break;
                default:
                    return null;
            }
            
            string jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());
            evnt.SetData(jsonData);
            evnt.EventStoreUuid = resolvedEvent.Event.EventId;
            evnt.EventId = evnt.EventStoreUuid.ToGuid();
            evnt.GlobalPosition = resolvedEvent.Event.Position.PreparePosition;
            evnt.StreamPosition = resolvedEvent.Event.EventNumber.ToUInt64();
            return evnt;
        }

        public EventData ToEventStoreEvent() {
            return new EventData(
                EventStoreUuid,
                Type, 
                Encoding.UTF8.GetBytes(GetDataJson())
            );
        }
    }
}
