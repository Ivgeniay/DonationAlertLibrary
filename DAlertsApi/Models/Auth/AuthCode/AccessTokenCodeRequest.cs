using Newtonsoft.Json; 

namespace DAlertsApi.Models.Auth.AuthCode
{
    /// <summary>
    /// The authorization code must be exchanged for an access token in https://www.donationalerts.com/oauth/token with grant_type, client_id, client_secret, redirect_uri and code parameters.
    /// Post:                           https://www.donationalerts.com/oauth/token
    /// 
    /// grant_type=authorization_code:  The grant type
    /// client_id:                      The application ID you received from DonationAlerts
    /// client_secret:                  The application secret you received from DonationAlerts
    /// redirect_uri:                   The URL where users will be sent after authorization
    /// code:                           The authorization code
    /// </summary>
    public class AccessTokenCodeRequest
    {
        public string Grant_type { get; set; } = "authorization_code";
        public int Client_id { get; set; }
        public string Client_secret { get; set; } = string.Empty;
        public string Redirect_uri { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}