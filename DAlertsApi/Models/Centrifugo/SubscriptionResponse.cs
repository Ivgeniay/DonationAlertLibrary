namespace DAlertsApi.Models.Centrifugo
{
    public class SubscriptionResponse
    {
        public List<SubscriptionData> Channels { get; set; } = new();

        public class SubscriptionData
        {
            public string Channel { get; set; } = string.Empty;
            public string Token { get; set; } = string.Empty;
        }
    }
}
