using System;

namespace TbspRpgLib.Events.Game.Content
{
    public class GameAddSourceKey : EventContent
    {
        public Guid SourceKey { get; set; }
    }
}