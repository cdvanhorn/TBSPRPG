namespace TbspRpgLib.Events.Game.Content {
    public class GameNew : EventContent {
        public string UserId { get; set; }

        public string AdventureName { get; set; }

        public string AdventureId { get; set; }
        
        public string Language { get; set; }
    }
}
