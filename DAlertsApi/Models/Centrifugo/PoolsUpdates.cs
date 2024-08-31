using DAlertsApi.Models.ApiV1;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DAlertsApi.Models.Centrifugo
{
    /// <summary>
    /// id:                     The unique poll identifier
    /// is_active:              A flag indicating whether the poll is in progress or not
    /// title:                  The poll title
    /// allow_user_options:     A flag indicating whether the poll allows donors to add their own poll options or not
    /// type:                   Type of the poll that defines how poll winner is calculated.count - finds winner by the most number of donations; sum - finds winner by the most sum of donations
    /// options:                Array of available poll options represented as Poll Option resource
    /// </summary>
    public class PoolsUpdates
    {
        public int Id { get; set; }   
        public int Is_active {get; set;}    
        public string Title {get; set;}
        public int Allow_user_options { get; set;}
        public string Type {get; set;}   
        public List<PoolOption> Options { get; set; } = new();
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
        public int Id { get; set; }
        public string Title { get; set; }
        public float Amount_value { get; set; }
        public float Amount_percent { get; set; }
        public int Is_winner { get; set; }
    }
}
