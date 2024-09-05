using Newtonsoft.Json;
using System.Collections.Generic;

namespace DAlertsApi.Models.Centrifugo
{
    public class SubscriptionRequest
    {
        public List<string> Channels { get; set; } = new List<string>();
        public string Client { get; set; } = string.Empty;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
