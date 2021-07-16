using System.Text.Json;
using TbspRpgLib.Aggregates;
using TbspRpgLib.Events.Game.Content;

namespace TbspRpgLib.Events.Game
{
    public class GameAddSourceKeyEvent : EventCore
    {
        public GameAddSourceKeyEvent(GameAddSourceKey data) : base() {
            Type = GAME_ADD_SOURCE_KEY_EVENT_TYPE;
            Data = data;
        }

        public GameAddSourceKeyEvent() : base() {
            Type = GAME_ADD_SOURCE_KEY_EVENT_TYPE;
        }
        
        protected override void SetData(string jsonString)
        {
            var gameAddSourceKey = JsonSerializer.Deserialize<GameAddSourceKey>(jsonString);
            Data = gameAddSourceKey;
        }

        public override string GetDataJson()
        {
            return JsonSerializer.Serialize((GameAddSourceKey)Data);
        }

        public override void UpdateAggregate(Aggregate agg)
        {
            var gameAggregate = (GameAggregate)agg;
            var gameAddSourceKey = (GameAddSourceKey) Data;
            gameAggregate.SourceKeys.Add(gameAddSourceKey.SourceKey);
        }
    }
}