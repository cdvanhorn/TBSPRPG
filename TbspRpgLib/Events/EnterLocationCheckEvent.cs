using TbspRpgLib.Events.Content;
using TbspRpgLib.Aggregates;

using System.Text.Json;

namespace TbspRpgLib.Events {
    public class EnterLocationCheckEvent : EventCore {
        public EnterLocationCheckEvent(EnterLocationCheck data) : base() {
            Type = ENTER_LOCATION_CHECK_EVENT_TYPE;
            Data = data;
        }

        public EnterLocationCheckEvent() : base() {
            Type = ENTER_LOCATION_CHECK_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            GameAggregate aggregate = (GameAggregate)agg;
            EnterLocationCheck data = (EnterLocationCheck)Data;
            aggregate.Checks.Location = data.Result;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            EnterLocationCheck newdata = JsonSerializer.Deserialize<EnterLocationCheck>(jsonString);
            Data = newdata;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((EnterLocationCheck)Data);
        }
    }
}