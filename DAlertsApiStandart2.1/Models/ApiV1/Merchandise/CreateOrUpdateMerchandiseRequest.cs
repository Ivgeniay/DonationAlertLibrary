using DAlertsApi.Models.Data;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.Collections.Generic;

namespace DAlertsApi.Models.ApiV1.Merchandise
{
    /// <summary>
    /// A combined method which allows to update merchandise, or create if it doesn't exist yet. If preferred, it can be used insdead of separate create and update API methods as this method allows to update a merchandise without need store DonationAlerts merchandise ID. This API is a part of the Merchandise Advertisement API.
    /// PUT:            https://www.donationalerts.com/api/v1/merchandise/{:merchant_identifier}/{:merchandise_identifier}
    /// 
    /// title:          Array of up to 1024 characters long strings representing the name of the merchandise in different locales.At minimum, a title for the en_US locale is required
    /// is_active:      A value containing 0 or 1. Determines whether the merchandise is available for purchase or not.Default value: 0
    /// is_percentage:  A value containing 0 or 1. Determines whether the price_service and price_user parameters are recognized as amounts in a currency of the currency parameter or calculated as a percent of the sale's total. Default value: 0
    /// currency:       One of the available currencies of merchandise. All revenue calculations will be performed according this value
    /// price_user:     Amount of revenue added to streamer for each sale of the merchandise
    /// price_service:  Amount of revenue added to DonationAlerts for each sale of the merchandise
    /// url:            Up to 128 characters long URL to the merchandise's web page. You may include the {user_id} and {user_merchandise_promocode} patterns in the URL that will be replaced in a UI with the user's ID and user's merchandise promocode
    /// img_url:        Up to 128 characters long URL to the merchandise's image
    /// end_at_ts:      Date and time when the merchandise becomes inactive represented as Unix timestamp
    /// signature:      Request signature
    /// </summary>
    public class CreateOrUpdateMerchandiseRequest
    {
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
        public string Url { get; set; } = string.Empty;

        [JsonProperty("img_url")]
        public string ImgUrl { get; set; } = string.Empty;

        [JsonProperty("end_at_ts")]
        public long? EndAtTs { get; set; }

        [JsonProperty("signature")]
        public string Signature { get; set; } = string.Empty;
    }
}
