using System.Collections.Generic;

namespace TbspRpgLib.Events.Location.Content {
    public class LocationEnterPass : EventContent {
        public string CurrentLocation { get; set; }
        
        public List<string> CurrentRoutes { get; set; }

        public string DestinationLocation { get; set; }
        
        public List<string> DestinationRoutes { get; set; }
    }
}