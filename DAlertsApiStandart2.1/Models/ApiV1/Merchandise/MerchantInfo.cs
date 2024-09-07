using Newtonsoft.Json;

namespace DAlertsApi.Models.ApiV1.Merchandise
{
    public class MerchantInfo
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; } = string.Empty;

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
}
