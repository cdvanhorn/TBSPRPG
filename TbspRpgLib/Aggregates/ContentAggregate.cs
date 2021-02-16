using System.Collections.Generic;

namespace TbspRpgLib.Aggregates {
    public class ContentAggregate : Aggregate {
        public ContentAggregate() {
            Text = new List<string>();
        }
        
        public List<string> Text { get; set; }
    }
}