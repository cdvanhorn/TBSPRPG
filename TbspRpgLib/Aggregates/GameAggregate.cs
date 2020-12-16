namespace TbspRpgLib.Aggregates {
    public class GameAggregate : Aggregate {
        public string UserId { get; set; }
        public string AdventureId { get; set; }
        public string AdventureName { get; set; }
        public string CurrentLocation { get; set; }
        public string Destination { get; set; }
    }
}
