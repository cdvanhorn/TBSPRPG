using System.Text.Json;

using TbspRpgLib.Events.Content;

using TbspRpgLib.Aggregates;

namespace TbspRpgLib.Events {
    public class EnterLocationEvent : EventCore {
        public EnterLocationEvent(EnterLocation data) : base() {
            Type = ENTER_LOCATION_EVENT_TYPE;
            Data = data;
        }

        public EnterLocationEvent() : base() {
            Type = ENTER_LOCATION_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            GameAggregate aggregate = (GameAggregate)agg;
            EnterLocation data = (EnterLocation)Data;
            aggregate.Id = Data.Id;
            aggregate.Destination = data.Destination;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            EnterLocation newdata = JsonSerializer.Deserialize<EnterLocation>(jsonString);
            Data = newdata;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((EnterLocation)Data);
        }
    }
}