namespace TbspRpgLib.Aggregates {
    public class GameAggregateChecks { 
        public bool Location { get; set; }
    }

    public class GameAggregate : Aggregate {
        public GameAggregate() {
            Checks = new GameAggregateChecks();
        }

        public string UserId { get; set; }
        public string AdventureId { get; set; }
        public string AdventureName { get; set; }
        public string CurrentLocation { get; set; }
        public string Destination { get; set; }
        public GameAggregateChecks Checks { get; set; }
    }
}

//so the location service produces an enter_location event
//the game system service captures the event and checks if they can enter the location
//the game system service produces a enter_location_check event
//the location service listens for the enter_location_check event
//if succeeds produce entered_location event
//if fail produce fail_enter_location event 
