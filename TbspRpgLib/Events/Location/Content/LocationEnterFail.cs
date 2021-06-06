using System.Collections.Generic;

namespace TbspRpgLib.Events.Location.Content {
    public class LocationEnterFail : EventContent {
        public string DestinationLocation { get; set; }
        
        public List<string> DestinationRoutes { get; set; }
    }
}