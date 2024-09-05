﻿using DAlertsApi.Models.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace DAlertsApi.Models.ApiV1.Merchandise
{
    /// <summary> 
    /// PUT:  https://www.donationalerts.com/api/v1/merchandise/{:id}
    /// 
    /// merchant_identifier:    Merchant's ID on DonationAlerts
    /// merchandise_identifier: Up to 16 characters long unique merchandise ID generated by the merchant
    /// title:                  Array of up to 1024 characters long strings representing the name of the merchandise in different locales.At minimum, a title for the en_US locale is required
    /// is_active:              A value containing 0 or 1. Determines whether the merchandise is available for purchase or not.Default value: 0
    /// is_percentage:          A value containing 0 or 1. Determines whether the price_service and price_user parameters are recognized as amounts in a currency of the currency parameter or calculated as a percent of the sale's total. Default value: 0
    /// currency:               One of the available currencies of merchandise. All revenue calculations will be performed according this value
    /// price_user:             Amount of revenue added to streamer for each sale of the merchandise
    /// price_service:          Amount of revenue added to DonationAlerts for each sale of the merchandise
    /// url:                    Up to 128 characters long URL to the merchandise's web page. You may include the {user_id} and {user_merchandise_promocode} patterns that will be replaced in a UI with the user's ID and user's merchandise promocode
    /// img_url:                Up to 128 characters long URL to the merchandise's image
    /// end_at_ts:              Date and time when the merchandise becomes inactive represented as Unix timestamp
    /// signature:              Request signature
    /// </summary>
    public class UpdateMerchandiseRequest
    {
        /// <summary>
        /// Merchant's ID on DonationAlerts
        /// </summary>
        public string MerchantIdentifier { get; set; } = string.Empty;

        /// <summary>
        /// Up to 16 characters long unique merchandise ID generated by the merchant
        /// </summary>
        public string MerchandiseIdentifier { get; set; } = string.Empty;

        /// <summary>
        /// Array of up to 1024 characters long strings representing the name of the merchandise in different locales.
        /// At minimum, a title for the en_US locale is required.
        /// </summary>
        public Dictionary<string, string> Title { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// A value containing 0 or 1. Determines whether the merchandise is available for purchase or not. Default value: 0
        /// </summary>
        public int IsActive { get; set; } = 0;

        /// <summary>
        /// A value containing 0 or 1. Determines whether the price_service and price_user parameters are recognized
        /// as amounts in a currency of the currency parameter or calculated as a percent of the sale's total. Default value: 0
        /// </summary>
        public int IsPercentage { get; set; } = 0;

        /// <summary>
        /// One of the available currencies of merchandise. All revenue calculations will be performed according to this value.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrenciesType Currency { get; set; }

        /// <summary>
        /// Amount of revenue added to streamer for each sale of the merchandise
        /// </summary>
        public decimal PriceUser { get; set; }

        /// <summary>
        /// Amount of revenue added to DonationAlerts for each sale of the merchandise
        /// </summary>
        public decimal PriceService { get; set; }

        /// <summary>
        /// Up to 128 characters long URL to the merchandise's web page. 
        /// You may include the {user_id} and {user_merchandise_promocode} patterns.
        /// </summary>
        public string Url { get; set; } = string.Empty;

        /// <summary>
        /// Up to 128 characters long URL to the merchandise's image
        /// </summary>
        public string ImgUrl { get; set; } = string.Empty;

        /// <summary>
        /// Date and time when the merchandise becomes inactive represented as Unix timestamp
        /// </summary>
        public long EndAtTs { get; set; }

        /// <summary>
        /// Request signature
        /// </summary>
        public string Signature { get; set; } = string.Empty;
    }
}
