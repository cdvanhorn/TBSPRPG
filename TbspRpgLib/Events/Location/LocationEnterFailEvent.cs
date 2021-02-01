using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Aggregates;

using System.Text.Json;

namespace TbspRpgLib.Events.Location {
    public class LocationEnterFailEvent : LocationEnterResultEvent {
        public LocationEnterFailEvent(LocationEnterResult data) : base(data) {
            Type = Event.LOCATION_ENTER_FAIL_EVENT_TYPE;
        }

        public LocationEnterFailEvent() : base() {
            Type = Event.LOCATION_ENTER_FAIL_EVENT_TYPE;
        }
    }
}