namespace TbspRpgLib.InterServiceCommunication.RequestModels {
    public class ContentFilterRequest : ContentRequest {
        public string Direction { get; set; }
        public long? Start { get; set; }
        public long? Count { get; set; }
    }
}