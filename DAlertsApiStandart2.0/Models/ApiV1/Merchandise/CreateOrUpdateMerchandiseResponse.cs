using DAlertsApi.Models.Data;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DAlertsApi.Models.ApiV1.Merchandise
{
    public class CreateOrUpdateMerchandiseResponseWrap
    {         
        [JsonProperty("data")]
        public CreateOrUpdateMerchandiseResponse Data { get; set; } = new CreateOrUpdateMerchandiseResponse();
    }
    public class CreateOrUpdateMerchandiseResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("merchant")]
        public MerchantInfo Merchant { get; set; } = new MerchantInfo();

        [JsonProperty("identifier")]
        public string Identifier { get; set; } = string.Empty;

        [JsonProperty("title")]
        [JsonConverter(typeof(LocalesTypeDictionaryConverter))]
        public Dictionary<LocalesType, string> Title { get; set; } = new Dictionary<LocalesType, string>();

        [JsonProperty("is_active")]
        public int IsActive { get; set; }

        [JsonProperty("is_percentage")]
        public int IsPercentage { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; } = string.Empty;

        [JsonProperty("price_user")]
        public decimal PriceUser { get; set; }

        [JsonProperty("price_service")]
        public decimal PriceService { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }

        [JsonProperty("img_url")]
        public string? ImgUrl { get; set; }

        [JsonProperty("end_at")]
        public string? EndAt { get; set; }
    }
}
