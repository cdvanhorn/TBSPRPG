using System.Text.Json;

using TbspRpgLib.Aggregates;
using TbspRpgLib.Events.Game.Content;
using TbspRpgLib.Settings;

namespace TbspRpgLib.Events.Game {
    public class GameNewEvent : EventCore {
        public GameNewEvent(GameNew data) : base() {
            Type = GAME_NEW_EVENT_TYPE;
            Data = data;
        }

        public GameNewEvent() : base() {
            Type = GAME_NEW_EVENT_TYPE;
        }

        public override void UpdateAggregate(Aggregate agg) {
            GameAggregate aggregate = (GameAggregate)agg;
            GameNew gdata = (GameNew)Data;
            aggregate.Id = Data.Id;
            aggregate.UserId = gdata.UserId;
            aggregate.AdventureId = gdata.AdventureId;
            aggregate.AdventureName = gdata.AdventureName;
            if (gdata.Language != null)
                aggregate.Settings.Language = gdata.Language;
            else
                aggregate.Settings.Language = Languages.DEFAULT;
        }

        protected override void SetData(string jsonString) {
            //parse the string as json and set the content
            GameNew ngame = JsonSerializer.Deserialize<GameNew>(jsonString);
            Data = ngame;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((GameNew)Data);
        }
    }
}
