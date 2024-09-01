using DAlertsApi.Models.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAlertsApi.Models.ApiV1.Merchandise
{
    public class CreateMerchandiseSaleNotificationResponseWrap
    {
        [JsonProperty("data")]
        public CreateMerchandiseSaleNotificationResponse Data { get; set; }
    }

    public class CreateMerchandiseSaleNotificationResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("external_id")]
        public string ExternalId { get; set; } = string.Empty;

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("message")]
        public string? Message { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrenciesType Currency { get; set; }

        [JsonProperty("bought_amount")]
        public int BoughtAmount { get; set; }

        [JsonProperty("is_shown")]
        public bool IsShown { get; set; }

        [JsonProperty("created_at")]
        public DateTime CreatedAt { get; set; }

        [JsonProperty("shown_at")]
        public DateTime? ShownAt { get; set; }
    }
}
