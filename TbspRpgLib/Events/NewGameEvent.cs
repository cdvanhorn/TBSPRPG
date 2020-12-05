using System.Text.Json;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events.Content;

namespace TbspRpgLib.Events {
    public class NewGameEvent : Event {
        public NewGame Data { get; set; }

        public NewGameEvent(NewGame data) : base() {
            Type = NEW_GAME_EVENT_TYPE;
            Data = data;
        }

        public NewGameEvent() : base() {
            Type = NEW_GAME_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            GameAggregate aggregate = (GameAggregate)agg;
            aggregate.Id = Data.Id;
            aggregate.UserId = Data.UserId;
            aggregate.AdventureId = Data.AdventureId;
            aggregate.AdventureName = Data.AdventureName;
        }

        protected override EventContent GetData() {
            return Data;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            NewGame ngame = JsonSerializer.Deserialize<NewGame>(jsonString);
            Data = ngame;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize(Data);
        }

        public override string GetStreamId() {
            return Data.Id;
        }

        public override string ToString() {
            return $"{EventId}\n{Type}\n{GetDataJson()}";
        }
    }
}