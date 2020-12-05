using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TbspRpgLib.Entities {
    public class EventIndex {
        [BsonElement("event_name")]
        public string EventName { get; set; }

        [BsonElement("index")]
        public ulong Index { get; set;}
    }
}