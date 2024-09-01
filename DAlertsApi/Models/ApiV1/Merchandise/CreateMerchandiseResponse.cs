using DAlertsApi.Models.Data;
using Newtonsoft.Json; 

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
    public class CreateMerchandiseResponse
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("merchant")]
        public Merchant Merchant { get; set; } = new(); 
        [JsonProperty("identifier")]
        public string identifier { get; set; } = string.Empty;
        [JsonProperty("title")]
        public MerchLocalizations Title { get; set; } = new();
        [JsonProperty("is_active")]
        public int Is_active { get; set; }
        [JsonProperty("is_percentage")]
        public int Is_percentage { get; set; }
        [JsonProperty("currency")]
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
    }

    public class Merchant
    {
        [JsonProperty("identifier")]
        public string Identifier { get; set; } = string.Empty;
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
    }
    public class MerchLocalizations
    {
        [JsonProperty("be_BY")]
        public string be_BY { get; set; } = string.Empty;
        [JsonProperty("de_DE")]
        public string de_DE { get; set; } = string.Empty;
        [JsonProperty("en_US")]
        public string en_US { get; set; } = string.Empty;
        [JsonProperty("es_ES")]
        public string es_ES { get; set; } = string.Empty;
        [JsonProperty("es_US")]
        public string es_US { get; set; } = string.Empty;
        [JsonProperty("et_EE")]
        public string et_EE { get; set; } = string.Empty;
        [JsonProperty("fr_FR")]
        public string fr_FR { get; set; } = string.Empty;
        [JsonProperty("he_HE")]
        public string he_HE { get; set; } = string.Empty;
        [JsonProperty("it_IT")]
        public string it_IT { get; set; } = string.Empty;
        [JsonProperty("ka_GE")]
        public string ka_GE { get; set; } = string.Empty;
        [JsonProperty("kk_KZ")]
        public string kk_KZ { get; set; } = string.Empty;
        [JsonProperty("ko_KR")]
        public string ko_KR { get; set; } = string.Empty;
        [JsonProperty("lv_LV")]
        public string lv_LV { get; set; } = string.Empty;
        [JsonProperty("pl_PL")]
        public string pl_PL { get; set; } = string.Empty;
        [JsonProperty("pt_BR")]
        public string pt_BR { get; set; } = string.Empty;
        [JsonProperty("ru_RU")]
        public string ru_RU { get; set; } = string.Empty;
        [JsonProperty("sv_SE")]
        public string sv_SE { get; set; } = string.Empty;
        [JsonProperty("tr_TR")]
        public string tr_TR { get; set; } = string.Empty;
        [JsonProperty("uk_UA")]
        public string uk_UA { get; set; } = string.Empty;
        [JsonProperty("zh_CN")]
        public string zh_CN { get; set; } = string.Empty;
    }
}
