namespace TbspRpgLib.Events.Location.Content {
    public class LocationEnterResult : EventContent {
        public string CurrentLocation { get; set; }

        public string Destination { get; set; }
    }
}