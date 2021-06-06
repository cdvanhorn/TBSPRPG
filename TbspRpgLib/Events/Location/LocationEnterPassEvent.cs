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
            var aggregate = (GameAggregate)agg;
            var data = (LocationEnterPass)Data;
            aggregate.MapData.DestinationLocation = data.DestinationLocation;
            aggregate.MapData.DestinationRoutes = data.DestinationRoutes;
            aggregate.MapData.CurrentLocation = data.CurrentLocation;
            aggregate.MapData.CurrentRoutes = data.CurrentRoutes;
            aggregate.Checks.Location = false;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            var locationEnterPass = JsonSerializer.Deserialize<LocationEnterPass>(jsonString);
            Data = locationEnterPass;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((LocationEnterPass)Data);
        }
    }
}