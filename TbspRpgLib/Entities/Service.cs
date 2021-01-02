using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

using System;
using System.Collections.Generic;
using System.Linq;

namespace TbspRpgLib.Entities {
    public class Service {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        private string _url { get; set; }

        [BsonElement("url")]
        public string Url 
        {
             get {
                 return _url;
             } 
             set {
                 if(string.IsNullOrEmpty(value))
                    throw new ArgumentNullException($"service url can't be null");
                _url = value;
             }
        }

        [BsonElement("event_prefix")]
        public string EventPrefix { get; set;}

        [BsonElement("event_indexes")]
        public List<EventIndex> EventIndexes { get; set; }

        public EventIndex GetEventIndexForEventName(string eventName) {
            return EventIndexes.Where(ei => ei.EventName == eventName).FirstOrDefault();
        }

        public bool DoesEventExistInIndex(string eventName) {
            if(EventIndexes == null || GetEventIndexForEventName(eventName) == null)
                return false;
            return true;
        }

        public ulong GetStartPosition(string eventName) {
            if(!DoesEventExistInIndex(eventName)) {
                throw new InvalidOperationException("service object not initialized properly");
            }

            EventIndex eventIndex = GetEventIndexForEventName(eventName);
            return eventIndex.Index;
        }

        public bool UpdatePosition(ulong position, string eventName) {
            if(!DoesEventExistInIndex(eventName)) {
                throw new InvalidOperationException("service object not initialized properly");
            }

            var eventIndex = GetEventIndexForEventName(eventName);
            if(eventIndex.Index < position) {
                eventIndex.Index = position;
                return true;
            }
            return false;
        }
    }
}