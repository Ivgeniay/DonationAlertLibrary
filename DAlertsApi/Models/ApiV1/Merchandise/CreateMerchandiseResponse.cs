using DAlertsApi.Models.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAlertsApi.Models.ApiV1.Merchandise
{
    /// <summary> 
    /// 
    /// id:             Unique merchandise ID on DonationAlerts
    /// merchant:       Object carrying identifier and name fields that contains information about the merchant
    /// identifier:     Unique merchandise ID on the merchant's online store
    /// title:          Object carrying merchandise's titles in different locales
    /// is_active:      A flag indicating whether the merchandise is available for purchase or not
    /// is_percentage:  A flag indicating whether the price_service and price_user parameters should be recognized as absolute values of the currency currency or as a percent of the sale's total
    /// currency:       The currency code of the merchandise(ISO 4217 formatted)
    /// price_user:     Amount of revenue added to streamer for each sale of the merchandise
    /// price_service:  Amount of revenue added to DonationAlerts for each sale of the merchandise
    /// url:            URL to the merchandise's web page. Or null if URL is not set
    /// img_url:        URL to the merchandise's image. Or null if image is not set
    /// end_at:         Date and time indicating when the merchandise becomes inactive(YYYY-MM-DD HH.MM.SS formatted). Or null if end date is not set
    /// </summary>
    public class CreateMerchandiseResponseWrap
    {
        [JsonProperty("data")]
        public CreateMerchandiseResponse Data { get; set; } = new CreateMerchandiseResponse();
    }
    public class CreateMerchandiseResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; } = new(); 
        [JsonProperty("identifier")]
        public string identifier { get; set; } = string.Empty;
        [JsonProperty("title")]
        [JsonConverter(typeof(LocalesTypeDictionaryConverter))]
        public Dictionary<LocalesType, string> Title { get; set; } = new();
        [JsonProperty("is_active")]
        public int Is_active { get; set; }
        [JsonProperty("is_percentage")]
        public int Is_percentage { get; set; }
        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrenciesType Currency { get; set; }
        [JsonProperty("price_user")]
        public decimal Price_user { get; set; }
        [JsonProperty("price_service")]
        public decimal Price_service { get; set;}
        [JsonProperty("url")]
        public string? Url { get; set; }
        [JsonProperty("img_url")]
        public string? Img_url { get; set; }
        [JsonProperty("end_at")]
        public string? End_at { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class Merchant
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; } = string.Empty;
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
   
}
