using TbspRpgLib.Aggregates;
using System.Text.Json;

namespace TbspRpgLib.Events.Content {
    public class ContentEvent : EventCore{
        public ContentEvent(ContentContent data) : base() {
            //Type = GAME_NEW_EVENT_TYPE;
            Data = data;
        }

        public ContentEvent() : base() {
            //Type = GAME_NEW_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            // GameAggregate aggregate = (GameAggregate)agg;
            // GameNew gdata = (GameNew)Data;
            // aggregate.Id = Data.Id;
            // aggregate.UserId = gdata.UserId;
            // aggregate.AdventureId = gdata.AdventureId;
            // aggregate.AdventureName = gdata.AdventureName;
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

        public override string GetStreamId() {
            return $"content_{Data.Id}";
        }
    }
}