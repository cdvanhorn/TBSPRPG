using TbspRpgLib.Aggregates;
using System.Text.Json;

namespace TbspRpgLib.Events.Content {
    public class ContentEvent : EventCore{
        public ContentEvent(ContentContent data) : base() {
            Type = CONTENT_EVENT_TYPE;
            Data = data;
        }

        public ContentEvent() : base() {
            Type = CONTENT_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            ContentAggregate aggregate = (ContentAggregate)agg;
            ContentContent ctnt = (ContentContent)Data;
            aggregate.Id = ctnt.Id;
            aggregate.Text = $"{aggregate.Text}\n{ctnt.Text}";
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            ContentContent ctnt = JsonSerializer.Deserialize<ContentContent>(jsonString);
            Data = ctnt;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((ContentContent)Data);
        }

        public override string GetStreamIdPrefix() {
            return Aggregate.CONTENT_AGGREGATE_PREFIX;
        }
    }
}