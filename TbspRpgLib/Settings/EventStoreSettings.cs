namespace TbspRpgLib.Settings {
    public class EventStoreSettings : IEventStoreSettings{
        public string Url { get; set; }
        public string Port { get; set; }
    }

    public interface IEventStoreSettings {
        string Url { get; set; }
        string Port { get; set; }
    }
}
