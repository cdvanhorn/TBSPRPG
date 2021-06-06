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

//so the location service produces an enter_location event
//the game system service captures the event and checks if they can enter the location
//the game system service produces a enter_location_check event
//the location service listens for the enter_location_check event
//if succeeds produce entered_location event
//if fail produce fail_enter_location event 
