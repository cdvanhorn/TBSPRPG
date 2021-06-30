using System;

namespace TbspRpgLib.InterServiceCommunication.RequestModels {
    public class ContentRequest {
        public Guid GameId { get; set; }
        public string Language { get; set; }
        public Guid SourceKey { get; set; }
        public ulong Position { get; set; }
    }
}