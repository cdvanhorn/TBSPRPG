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
            var aggregate = (GameAggregate)agg;
            var data = (LocationEnter)Data;
            aggregate.MapData.DestinationLocation = data.DestinationLocation;
            aggregate.MapData.DestinationRoutes = data.DestinationRoutes;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            var locationEnter = JsonSerializer.Deserialize<LocationEnter>(jsonString);
            Data = locationEnter;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((LocationEnter)Data);
        }
    }
}