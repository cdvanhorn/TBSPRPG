using System;
using System.Collections.Generic;

namespace TbspRpgLib.Aggregates {
    public class Aggregate {
        public string Id { get; set; }

        public List<string> ProcessedEventIds { get; set; }
    }
}
