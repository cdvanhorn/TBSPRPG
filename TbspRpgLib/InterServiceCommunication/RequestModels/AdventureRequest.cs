using System;

namespace TbspRpgLib.InterServiceCommunication.RequestModels {
    public class AdventureRequest {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid LocationId { get; set; }
        public string Language { get; set; }
        public Guid SourceKey { get; set; }
    }
}