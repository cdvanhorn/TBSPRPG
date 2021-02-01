using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Aggregates;

using System.Text.Json;

namespace TbspRpgLib.Events.Location {
    public class LocationEnterFailEvent : EventCore {
        public LocationEnterFailEvent(LocationEnterFail data) : base() {
            Type = Event.LOCATION_ENTER_FAIL_EVENT_TYPE;
            Data = data;
        }

        public LocationEnterFailEvent() : base() {
            Type = Event.LOCATION_ENTER_FAIL_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            GameAggregate aggregate = (GameAggregate)agg;
            LocationEnterFail data = (LocationEnterFail)Data;
            aggregate.Destination = data.Destination;
            aggregate.Checks.Location = false;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            LocationEnterFail newdata = JsonSerializer.Deserialize<LocationEnterFail>(jsonString);
            Data = newdata;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((LocationEnterFail)Data);
        }
    }
}