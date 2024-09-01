using DAlertsApi.Models.Data;
using Newtonsoft.Json; 

namespace DAlertsApi.Models.Centrifugo
{
    /// <summary>
    /// Subscribing to this channel allows to receive updates regarding donation goals. Requires user authorization with the oauth-goal-subscribe scope.
    /// Centrifugo channel name — $goals:goal_<user_id>
    /// Messages sent to this channel contain the donation goal resource represented as described below.
    /// 
    /// 
    /// id:                 The unique donation goal identifier
    /// is_active:          A flag indicating whether the donation goal is in progress or not
    /// title:              The donation goal title
    /// currency:           The currency code of the donation goal(ISO 4217 formatted)
    /// start_amount:       Starting amount of the donation goal
    /// raised_amount:      Currently raised amount including the start_amount value
    /// goal_amount:        Goal amount of the donation goal
    /// started_at:         The date and time(YYYY-MM-DD HH.MM.SS formatted) when donation goal was started
    /// expires_at:         The date and time(YYYY-MM-DD HH.MM.SS formatted) when donation goal is scheduled to end.Or null if end date is not set
    /// </summary>

    public class GoalsUpdates
    {
        [JsonProperty("id")]
        public int Id {get; set;} 
        [JsonProperty("is_active")]
        public int Is_active {get; set;}
        [JsonProperty("title")]
        public string Title {get; set;} = string.Empty;
        [JsonProperty("currency")]
        public CurrenciesType Currency {get; set;}
        [JsonProperty("start_amount")]
        public float Start_amount {get; set;}
        [JsonProperty("raised_amount")]
        public float Raised_amount {get; set;}
        [JsonProperty("goal_amount")]
        public float Goal_amount {get; set;}
        [JsonProperty("started_at")]
        public string Started_at {get; set;} = string.Empty;
        [JsonProperty("expires_at")]
        public string? Expires_at {get; set; }
        [JsonProperty("reason")]
        public string? Reason { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this); 
    }

    public class GoalsUpdateWrapper
    {
        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("data")]
        public GoalsUpdates Data { get; set; } = new GoalsUpdates();
        public override string ToString() => JsonConvert.SerializeObject(this); 
    }

    public class GoalInfo
    {
        [JsonProperty("user")]
        public string User { get; set; } = string.Empty;
        [JsonProperty("client")]
        public string Client { get; set; } = string.Empty;
        /// <summary>
        /// Launch button status
        /// </summary>
        public bool IsLauched { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
