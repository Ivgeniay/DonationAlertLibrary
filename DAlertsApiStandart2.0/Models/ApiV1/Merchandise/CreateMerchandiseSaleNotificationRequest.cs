using DAlertsApi.Models.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAlertsApi.Models.ApiV1.Merchandise
{
    public class CreateMerchandiseSaleNotificationRequest
    { 
        [JsonProperty("user_id")]
        public int UserId { get; set; }

        [JsonProperty("external_id")]
        public string ExternalId { get; set; } = string.Empty;

        [JsonProperty("merchant_identifier")]
        public string MerchantIdentifier { get; set; } = string.Empty;

        [JsonProperty("merchandise_identifier")]
        public string MerchandiseIdentifier { get; set; } = string.Empty;

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrenciesType Currency { get; set; }

        [JsonProperty("bought_amount")]
        public int BoughtAmount { get; set; } = 1; // Default value

        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;

        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;

        [JsonProperty("signature")]
        public string Signature { get; set; } = string.Empty; 

        override public string ToString() => JsonConvert.SerializeObject(this);
    }
}
