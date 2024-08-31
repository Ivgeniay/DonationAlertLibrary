using Newtonsoft.Json;

namespace DAlertsApi.Models.Centrifugo
{
    public class SubscriptionRequest
    {
        public List<string> Channels { get; set; } = new();
        public string Client { get; set; } = string.Empty;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
