using Newtonsoft.Json; 
using System.Dynamic; 

namespace DAlertsApi.Models.Centrifugo
{
    /// <summary>
    /// Message structure for receiving messages via the Centrifugo WebSocket protocol.
    /// </summary>
    public class WebSocketMessage
    {
        public int Id { get; set; }
        public WebSocketResult Result { get; set; } = new WebSocketResult();

        override public string ToString() => JsonConvert.SerializeObject(this);

    }
    public class WebSocketResult
    {
        public int Type { get; set; }
        public string Channel { get; set; } = string.Empty;
        public dynamic Data { get; set; } = new ExpandoObject();
    }
}
