using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Aggregates;

using System.Text.Json;

namespace TbspRpgLib.Events.Location {
    public class LocationEnterFailEvent : EventCore {
        public LocationEnterFailEvent(LocationEnterFail data) {
            Type = Event.LOCATION_ENTER_FAIL_EVENT_TYPE;
            Data = data;
        }

        public LocationEnterFailEvent() {
            Type = Event.LOCATION_ENTER_FAIL_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            var aggregate = (GameAggregate)agg;
            var data = (LocationEnterFail)Data;
            aggregate.MapData.DestinationLocation = data.DestinationLocation;
            aggregate.MapData.DestinationRoutes = data.DestinationRoutes;
            aggregate.MapData.DestinationViaRoute = data.DestinationViaRoute;
            aggregate.Checks.Location = false;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            var locationEnterFail = JsonSerializer.Deserialize<LocationEnterFail>(jsonString);
            Data = locationEnterFail;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((LocationEnterFail)Data);
        }
    }
}