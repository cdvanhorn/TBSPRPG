using RestSharp;

namespace TbspRpgLib.InterServiceCommunication {
    public class IscResponse {
        
        public string Content { get; set; }
        
        public int StatusCode { get; set; }
        
        public bool IsSuccessful { get; set; }
    }
}