using System;

namespace TbspRpgLib.Entities {
    public class ProcessedEvent {
        public Guid Id { get; set; }

        public Guid ServiceId { get; set; }

        public Guid EventId { get; set; }
    }
}