using System.Text.Json;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events.Content;

namespace TbspRpgLib.Events {
    public class NewGameEvent : EventCore {
        public NewGameEvent(NewGame data) : base() {
            Type = NEW_GAME_EVENT_TYPE;
            Data = data;
        }

        public NewGameEvent() : base() {
            Type = NEW_GAME_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            GameAggregate aggregate = (GameAggregate)agg;
            NewGame gdata = (NewGame)Data;
            aggregate.Id = Data.Id;
            aggregate.UserId = gdata.UserId;
            aggregate.AdventureId = gdata.AdventureId;
            aggregate.AdventureName = gdata.AdventureName;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            NewGame ngame = JsonSerializer.Deserialize<NewGame>(jsonString);
            Data = ngame;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((NewGame)Data);
        }
    }
}
