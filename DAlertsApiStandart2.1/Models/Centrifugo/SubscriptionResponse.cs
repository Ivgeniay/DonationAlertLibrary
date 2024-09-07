using Newtonsoft.Json;
using System.Collections.Generic;

namespace DAlertsApi.Models.Centrifugo
{
    public class SubscriptionResponse
    {
        public List<SubscriptionData> Channels { get; set; } = new List<SubscriptionData>();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    public class SubscriptionData
    {
        public string Channel { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
