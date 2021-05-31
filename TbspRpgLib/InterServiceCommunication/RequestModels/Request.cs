using System;

namespace TbspRpgLib.InterServiceCommunication.RequestModels
{
    public class Request
    {
        public string ServiceName { get; set; }
        public string EndPoint { get; set; }
        public string Token { get; set; }
        public object Parameters { get; set; }
        public object PostData { get; set; }
    }
}