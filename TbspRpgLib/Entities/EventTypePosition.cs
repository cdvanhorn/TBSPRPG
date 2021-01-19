using System;

namespace TbspRpgLib.Entities {
    public class EventTypePosition {
        public Guid Id { get; set; }

        public Guid EventTypeId { get; set; }

        public ulong Position { get; set; }
    }
}