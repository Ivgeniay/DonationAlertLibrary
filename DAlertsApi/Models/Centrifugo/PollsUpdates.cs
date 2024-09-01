using Newtonsoft.Json; 

namespace DAlertsApi.Models.Centrifugo
{
    /// <summary>
    /// Subscribing to this channel allows to receive updates regarding polls. Requires user authorization with the oauth-poll-subscribe scope.
    /// Centrifugo channel name — $polls:poll_<user_id>
    /// Messages sent to this channel contain the poll resource represented as described below.
    /// 
    /// 
    /// id:                     The unique poll identifier
    /// is_active:              A flag indicating whether the poll is in progress or not
    /// title:                  The poll title
    /// allow_user_options:     A flag indicating whether the poll allows donors to add their own poll options or not
    /// type:                   Type of the poll that defines how poll winner is calculated.count - finds winner by the most number of donations; sum - finds winner by the most sum of donations
    /// options:                Array of available poll options represented as Poll Option resource
    /// </summary>
    public class PollsUpdates
    {
        [JsonProperty("id")]
        public int Id { get; set; }   
        [JsonProperty("is_active")]
        public int Is_active {get; set;}    
        [JsonProperty("title")]
        public string Title {get; set;}
        [JsonProperty("allow_user_options")]
        public int Allow_user_options { get; set;}
        [JsonProperty("type")]
        public string Type {get; set;}   
        [JsonProperty("options")]
        public List<PoolOption> Options { get; set; } = new();
        [JsonProperty("per_amount_votes")]
        public PerAmountVotes PerAmountVotes { get; set; } = new PerAmountVotes();
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; } = string.Empty;

        [JsonProperty("ended_at")]
        public string EndedAt { get; set; } = string.Empty;

        [JsonProperty("reason")]
        public string? Reason { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }


    /// <summary>
    /// id:                 The unique poll option identifier
    /// title:              The poll option title
    /// amount_value:       The absolute value of poll option.Depending on poll type the value may contain number or sum of donations
    /// amount_percent:     The percent value of poll option relative other poll options
    /// is_winner:          A flag indicating whether the poll option is the poll winner or not.Please note that poll may have multiple winners if maximium amount_value value is shared by several poll options
    /// </summary>
    public class PoolOption
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("amount_value")]
        public float Amount_value { get; set; }
        [JsonProperty("amount_percent")]
        public float Amount_percent { get; set; }
        [JsonProperty("is_winner")]
        public int Is_winner { get; set; }
    }
    public class PerAmountVotes
    {
        [JsonProperty("USD")]
        public double USD { get; set; }

        [JsonProperty("RUB")]
        public double RUB { get; set; }

        [JsonProperty("EUR")]
        public double EUR { get; set; }

        [JsonProperty("BYR")]
        public double BYR { get; set; }

        [JsonProperty("KZT")]
        public double KZT { get; set; }

        [JsonProperty("UAH")]
        public double UAH { get; set; }

        [JsonProperty("BYN")]
        public double BYN { get; set; }

        [JsonProperty("BRL")]
        public double BRL { get; set; }

        [JsonProperty("TRY")]
        public double TRY { get; set; }

        [JsonProperty("PLN")]
        public double PLN { get; set; }
    }
    public class PollDataWrapper
    {
        [JsonProperty("seq")]
        public int Seq { get; set; }

        [JsonProperty("data")]
        public PollsUpdates Data { get; set; } = new PollsUpdates();
    }
}
