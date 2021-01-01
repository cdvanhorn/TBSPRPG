using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using System.Collections.Generic;
using System.Linq;

namespace TbspRpgLib.Entities {
    public class Service {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("url")]
        public string Url { get; set; }

        [BsonElement("event_prefix")]
        public string EventPrefix { get; set;}

        [BsonElement("event_indexes")]
        public List<EventIndex> EventIndexes { get; set; }

        public ulong GetStartPosition(string eventName) {
            if(EventIndexes == null) 
                return 0;

            EventIndex ei = EventIndexes.Where(ei => ei.EventName == eventName).FirstOrDefault();
            ulong startPosition = 0;
            if(ei != null && ei.Index > 0)
                startPosition = ei.Index;
            return startPosition;
        }

        public bool UpdatePosition(ulong position, string eventName) {
            if(EventIndexes != null && EventIndexes.Where(ei => ei.EventName == eventName).Count() > 0) {
                var eventIndex = EventIndexes.Where(ei => ei.EventName == eventName).First();
                if(eventIndex.Index < position) {
                    eventIndex.Index = position;
                    return true;
                }
            }
            else {
                //create and insert a new event index
                EventIndex eventIndex = new EventIndex();
                eventIndex.EventName = eventName;
                eventIndex.Index = position;
                if(EventIndexes == null)
                    EventIndexes = new List<EventIndex>();
                EventIndexes.Add(eventIndex);
                return true;
            }
            return false;
        }
    }
}