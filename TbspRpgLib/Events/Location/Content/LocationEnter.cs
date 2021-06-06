using System.Collections.Generic;

namespace TbspRpgLib.Events.Location.Content {
    public class LocationEnter : EventContent {
        public string DestinationLocation { get; set; }
        
        public List<string> DestinationRoutes { get; set; }
    }
}