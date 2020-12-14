using System;
using System.Collections.Generic;

namespace TbspRpgLib.Aggregates {
    public class Aggregate {
        public string Id { get; set; }

        public List<string> ProcessedEventIds { get; set; }

        public void AddProcessedEventId(string processedEventId) {
            if(ProcessedEventIds == null)
                ProcessedEventIds = new List<string>();
            ProcessedEventIds.Add(processedEventId);
        }

        public bool HasEventIdBeenProcessed(string eventId) {
            if(ProcessedEventIds == null)
                return false;
            return ProcessedEventIds.Contains(eventId);
        }
    }
}
