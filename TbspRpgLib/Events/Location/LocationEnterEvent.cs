using System.Text.Json;

using TbspRpgLib.Events.Location.Content;

using TbspRpgLib.Aggregates;

namespace TbspRpgLib.Events.Location {
    public class LocationEnterEvent : EventCore {
        public LocationEnterEvent(LocationEnter data) : base() {
            Type = LOCATION_ENTER_EVENT_TYPE;
            Data = data;
        }

        public LocationEnterEvent() : base() {
            Type = LOCATION_ENTER_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            GameAggregate aggregate = (GameAggregate)agg;
            LocationEnter data = (LocationEnter)Data;
            aggregate.Destination = data.Destination;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            LocationEnter newdata = JsonSerializer.Deserialize<LocationEnter>(jsonString);
            Data = newdata;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((LocationEnter)Data);
        }
    }
}