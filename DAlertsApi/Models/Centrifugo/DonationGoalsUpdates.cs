using DAlertsApi.Models.ApiV1;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAlertsApi.Models.Centrifugo
{
    /// <summary>
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
    public class DonationGoalsUpdates
    {
        public int Id {get; set;} 
        public int Is_active {get; set;}
        public string Title {get; set;} = string.Empty;
        public string Currency {get; set;} = string.Empty;
        public float Start_amount {get; set;}
        public float Raised_amount {get; set;}
        public float Goal_amount {get; set;}
        public string Started_at {get; set;} = string.Empty;
        public string? Expires_at {get; set; }
    }
}
