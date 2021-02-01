namespace TbspRpgLib.Events.Location.Content {
    public class LocationEnterPass : EventContent {
        public string CurrentLocation { get; set; }

        public string Destination { get; set; }
    }
}