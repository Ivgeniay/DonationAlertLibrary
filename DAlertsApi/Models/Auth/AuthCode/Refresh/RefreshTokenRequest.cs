using Newtonsoft.Json;

namespace DAlertsApi.Models.Auth.AuthCode.Refresh
{
    /// <summary>
    /// The Refresh Token grant type is used by clients to exchange a refresh token for an access token when the access token has expired.
    /// The service will generate and return a new access token if provided data is valid.The server may issue a new refresh token as well, but if the response does not contain a new refresh token the existing refresh token will still be valid.
    ///
    /// POST:                       https://www.donationalerts.com/oauth/token
    /// 
    /// grant_type=refresh_token:   The grant type
    /// refresh_token:              The refresh_token you received from DonationAlerts
    /// client_id:                  The application ID you received from DonationAlerts
    /// client_secret:              The application secret you received from DonationAlerts
    /// scope:                      A space-delimited list of scopes 
    /// </summary>
    public class RefreshTokenRequest
    {
        public string Grant_type { get; set; } = "refresh_token";
        public string Refresh_token { get; set; } = string.Empty;
        public int Client_id { get; set; }
        public string Client_secret { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
