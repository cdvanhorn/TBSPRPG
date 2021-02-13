using System;
using System.Collections.Generic;

namespace TbspRpgLib.Aggregates {
    public class Aggregate {
        public const string CONTENT_AGGREGATE_PREFIX = "content";

        public string Id { get; set; }

        public ulong GlobalPosition { get; set; }

        public ulong StreamPosition { get; set; }
    }
}
