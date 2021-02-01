using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Aggregates;

using System.Text.Json;

namespace TbspRpgLib.Events.Location {
    public class LocationEnterPassEvent : EventCore {
        public LocationEnterPassEvent(LocationEnterPass data) : base() {
            Type = Event.LOCATION_ENTER_PASS_EVENT_TYPE;
            Data = data;
        }

        public LocationEnterPassEvent() : base() {
            Type = Event.LOCATION_ENTER_PASS_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            GameAggregate aggregate = (GameAggregate)agg;
            LocationEnterPass data = (LocationEnterPass)Data;
            aggregate.Destination = data.Destination;
            aggregate.CurrentLocation = data.CurrentLocation;
            aggregate.Checks.Location = false;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            LocationEnterPass newdata = JsonSerializer.Deserialize<LocationEnterPass>(jsonString);
            Data = newdata;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((LocationEnterPass)Data);
        }
    }
}