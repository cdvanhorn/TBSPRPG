using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Aggregates;

using System.Text.Json;

namespace TbspRpgLib.Events.Location {
    public class LocationEnterPassEvent : LocationEnterResultEvent {
        public LocationEnterPassEvent(LocationEnterResult data) : base(data) {
            Type = Event.LOCATION_ENTER_PASS_EVENT_TYPE;
        }

        public LocationEnterPassEvent() : base() {
            Type = Event.LOCATION_ENTER_PASS_EVENT_TYPE;
        }
    }
}