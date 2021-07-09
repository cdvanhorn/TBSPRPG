using System;

namespace TbspRpgLib.InterServiceCommunication.RequestModels {
    public class GameRequest {
        public Guid AdventureId { get; set; }
        public Guid GameId { get; set; }
        public ulong Position { get; set; }
    }
}