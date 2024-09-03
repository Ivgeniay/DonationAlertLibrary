using DAlertsApi.Models.Centrifugo;
using DAlertsApi.Models.Data;
using DAlertsApi.Models.Pagination;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace DAlertsApi.Models.ApiV1.Donations
{
    /// <summary>
    /// Obtains array of objects of user donation alerts list. Requires user authorization with the oauth-donation-index scope.
    /// curl \
    /// -X GET https://www.donationalerts.com/api/v1/alerts/donations \
    /// -H "Authorization: Bearer <token>"
    ///  
    /// id:                 The unique donation alert identifier
    /// name:               Type of the alert.Always donation in this case
    /// username:           The name of the user who sent the donation and the alert
    /// message_type:       The message type.The possible values are text for a text messages and audio for an audio messages
    /// message:            The message sent along with the donation and the alert
    /// amount:             The donation amount
    /// currency:           The currency code (ISO 4217 formatted)
    /// is_shown:           A flag indicating whether the alert was shown in the streamer's widget
    /// created_at:         The donation date and time (YYYY-MM-DD HH.MM.SS formatted)
    /// shown_at:           Date and time indicating when the alert was shown (YYYY-MM-DD HH.MM.SS formatted). Or null if the alert is not shown yet
    /// </summary>
     
    public class Donation
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;
        [JsonProperty("username")]
        public string Username { get; set; } = string.Empty;
        [JsonProperty("message_type")]
        public string Message_type { get; set; } = string.Empty;
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        [JsonProperty("currency")]
        [JsonConverter(typeof(StringEnumConverter))]
        public CurrenciesType Currency { get; set; }
        [JsonProperty("is_shown")]
        public int Is_shown { get; set; }
        [JsonProperty("created_at")]
        public string Created_at { get; set; } = string.Empty;
        [JsonProperty("shown_at")]
        public string? Shown_at { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    } 

    /// <summary>
    /// DonationWrap is a wrapper for Donation model that contains links and meta information
    /// </summary>
    public class DonationWrap
    {
        [JsonProperty("data")]
        public List<Donation> Data { get; set; } = new();
        [JsonProperty("links")]
        public PaginationLinks Links { get; set; } = new();
        [JsonProperty("meta")]
        public PaginationInfo Meta { get; set; } = new();

        public override string ToString() => JsonConvert.SerializeObject(this);
    }


    /// <summary>
    /// Using in centrifugo client
    /// </summary>
    public class DonationWrapper
    {
        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("data")]
        public DonationData Data { get; set; } = new DonationData(); 
        public override string ToString() => JsonConvert.SerializeObject(this);


        public class DonationData
        {
            [JsonProperty("id")]
            public int Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; } = string.Empty;

            [JsonProperty("username")]
            public string? Username { get; set; }

            [JsonProperty("message")]
            public string? Message { get; set; }

            [JsonProperty("message_type")]
            public string MessageType { get; set; } = string.Empty;

            [JsonProperty("payin_system")]
            public PayinSystem PayinSystem { get; set; } = new PayinSystem();

            [JsonProperty("amount")]
            public decimal Amount { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; } = string.Empty;

            [JsonProperty("is_shown")]
            public int IsShown { get; set; }

            [JsonProperty("amount_in_user_currency")]
            public decimal AmountInUserCurrency { get; set; }

            [JsonProperty("recipient_name")]
            public string RecipientName { get; set; } = string.Empty;

            [JsonProperty("recipient")]
            public Recipient Recipient { get; set; } = new Recipient();

            [JsonProperty("created_at")]
            public DateTime CreatedAt { get; set; }

            [JsonProperty("shown_at")]
            public DateTime? ShownAt { get; set; }

            [JsonProperty("reason")]
            public string Reason { get; set; } = string.Empty;
        }

        public class PayinSystem
        {
            [JsonProperty("title")]
            public string Title { get; set; } = string.Empty;
        }

        public class Recipient
        {
            [JsonProperty("user_id")]
            public int UserId { get; set; }

            [JsonProperty("code")]
            public string Code { get; set; } = string.Empty;

            [JsonProperty("name")]
            public string Name { get; set; } = string.Empty;

            [JsonProperty("avatar")]
            public string Avatar { get; set; } = string.Empty;
        }
    }
}
