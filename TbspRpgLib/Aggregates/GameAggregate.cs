using System.Collections.Generic;

namespace TbspRpgLib.Aggregates {
    public class GameAggregateChecks { 
        public bool Location { get; set; }
    }

    public class MapData
    {
        public string CurrentLocation { get; set; }
        
        public List<string> CurrentRoutes { get; set; }
        
        public string DestinationLocation { get; set; }
        
        public List<string> DestinationRoutes { get; set; }
        
        public string DestinationViaRoute { get; set; }
    } 

    // ReSharper disable once ClassNeverInstantiated.Global
    public class GameAggregate : Aggregate {
        public GameAggregate() {
            Checks = new GameAggregateChecks();
            MapData = new MapData
            {
                CurrentRoutes = new List<string>(),
                DestinationRoutes = new List<string>()
            };
        }

        public string UserId { get; set; }
        public string AdventureId { get; set; }
        public string AdventureName { get; set; }
        public MapData MapData { get; set; }
        public GameAggregateChecks Checks { get; set; }
    }
}
