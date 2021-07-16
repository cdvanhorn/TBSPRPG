using System;
using System.Text;
using EventStore.Client;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events.Game;
using TbspRpgLib.Events.Location;

namespace TbspRpgLib.Events
{
    public abstract class Event {
        public const string GAME_NEW_EVENT_TYPE = "game_new";
        public const string GAME_ADD_SOURCE_KEY_EVENT_TYPE = "game_add_source_key";
        public const string LOCATION_ENTER_EVENT_TYPE = "location_enter";
        public const string LOCATION_ENTER_CHECK_EVENT_TYPE = "location_enter_check";
        public const string LOCATION_ENTER_PASS_EVENT_TYPE = "location_enter_pass";
        public const string LOCATION_ENTER_FAIL_EVENT_TYPE = "location_enter_fail";

        public Event() {
            EventId = Guid.NewGuid();
            EventStoreUuid = Uuid.FromGuid(EventId);
        }

        public Guid EventId { get; set; }

        public Uuid EventStoreUuid { get; set; }

        public ulong GlobalPosition { get; set; }

        public ulong StreamPosition { get; set; }

        public string Type { get; set; }

        public string StreamId { get; set; }

        public abstract string GetDataId();

        protected abstract EventContent GetData();

        protected abstract void SetData(string jsonString);

        public abstract string GetDataJson();

        // public abstract string GetStreamId();

        // public abstract string GetStreamIdPrefix();

        public abstract void UpdateAggregate(Aggregate agg);

        private static Event CreateEvent(string eventType) {
            if(eventType.StartsWith('$'))
                return null;
            
            Event evnt;
            //I'm not really happy with this part
            switch(eventType) {
                case GAME_NEW_EVENT_TYPE:
                    evnt = new GameNewEvent();
                    break;
                case LOCATION_ENTER_EVENT_TYPE:
                    evnt = new LocationEnterEvent();
                    break;
                case LOCATION_ENTER_CHECK_EVENT_TYPE:
                    evnt = new LocationEnterCheckEvent();
                    break;
                case LOCATION_ENTER_PASS_EVENT_TYPE:
                    evnt = new LocationEnterPassEvent();
                    break;
                case LOCATION_ENTER_FAIL_EVENT_TYPE:
                    evnt = new LocationEnterFailEvent();
                    break;
                case GAME_ADD_SOURCE_KEY_EVENT_TYPE:
                    evnt = new GameAddSourceKeyEvent();
                    break;
                default:
                    return null;
            }
            return evnt;
        }

        public static Event FromEventStoreEvent(ResolvedEvent resolvedEvent) {
            Event evnt = CreateEvent(resolvedEvent.Event.EventType);
            if(evnt == null)
                return evnt;
            
            string jsonData = Encoding.UTF8.GetString(resolvedEvent.Event.Data.ToArray());
            evnt.SetData(jsonData);
            evnt.StreamId = resolvedEvent.Event.EventStreamId;
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
