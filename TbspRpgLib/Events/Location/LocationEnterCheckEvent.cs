using TbspRpgLib.Events.Location.Content;
using TbspRpgLib.Aggregates;

using System.Text.Json;

namespace TbspRpgLib.Events.Location {
    public class LocationEnterCheckEvent : EventCore {
        public LocationEnterCheckEvent(LocationEnterCheck data) : base() {
            Type = LOCATION_ENTER_CHECK_EVENT_TYPE;
            Data = data;
        }

        public LocationEnterCheckEvent() : base() {
            Type = LOCATION_ENTER_CHECK_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            var aggregate = (GameAggregate)agg;
            var data = (LocationEnterCheck)Data;
            aggregate.Checks.Location = data.Result;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            var locationEnterCheck = JsonSerializer.Deserialize<LocationEnterCheck>(jsonString);
            Data = locationEnterCheck;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((LocationEnterCheck)Data);
        }
    }
}