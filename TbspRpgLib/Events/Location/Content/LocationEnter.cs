using System.Collections.Generic;

namespace TbspRpgLib.Events.Location.Content {
    public class LocationEnter : EventContent {
        public string DestinationViaRoute { get; set; }
        
        public string DestinationLocation { get; set; }
        
        public List<string> DestinationRoutes { get; set; }
    }
}