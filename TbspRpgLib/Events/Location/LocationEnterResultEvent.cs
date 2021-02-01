using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Aggregates;

using System.Text.Json;

namespace TbspRpgLib.Events.Location {
    public class LocationEnterResultEvent : EventCore {
        public LocationEnterResultEvent(LocationEnterResult data) : base() {
            Data = data;
        }

        public LocationEnterResultEvent() : base() {
        }

        public override void UpdateAggregate(Aggregate agg) {
            GameAggregate aggregate = (GameAggregate)agg;
            LocationEnterResult data = (LocationEnterResult)Data;
            aggregate.Destination = data.Destination;
            aggregate.CurrentLocation = data.CurrentLocation;
            aggregate.Checks.Location = false;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            LocationEnterResult newdata = JsonSerializer.Deserialize<LocationEnterResult>(jsonString);
            Data = newdata;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((LocationEnterCheck)Data);
        }
    }
}