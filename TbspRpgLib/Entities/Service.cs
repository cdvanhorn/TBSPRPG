using System;
using System.Collections.Generic;
using System.Linq;

namespace TbspRpgLib.Entities {
    public class Service {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public void GetEventIndexForEventName(string eventName) {
            //return EventIndexes.Where(ei => ei.EventName == eventName).FirstOrDefault();
        }

        public bool DoesEventExistInIndex(string eventName) {
            // if(EventIndexes == null || GetEventIndexForEventName(eventName) == null)
            //     return false;
            // return true;
            return false;
        }

        public ulong GetStartPosition(string eventName) {
            // if(!DoesEventExistInIndex(eventName)) {
            //     throw new InvalidOperationException("service object not initialized properly");
            // }

            // EventIndex eventIndex = GetEventIndexForEventName(eventName);
            // return eventIndex.Index;
            return 0;
        }

        public bool UpdatePosition(ulong position, string eventName) {
            // if(!DoesEventExistInIndex(eventName)) {
            //     throw new InvalidOperationException("service object not initialized properly");
            // }

            // var eventIndex = GetEventIndexForEventName(eventName);
            // if(eventIndex.Index < position) {
            //     eventIndex.Index = position;
            //     return true;
            // }
            return false;
        }
    }
}