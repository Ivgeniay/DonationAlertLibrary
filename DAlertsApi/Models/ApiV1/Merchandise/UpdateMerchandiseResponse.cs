using DAlertsApi.Models.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAlertsApi.Models.ApiV1.Merchandise
{
    /// <summary>
    /// 
    /// </summary>
    public class UpdateMerchandiseResponseWrap
    {
        [JsonProperty("data")]
        public UpdateMerchandiseResponse? Data { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
    public class UpdateMerchandiseResponse
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
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrenciesType Currency { get; set; }

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
