using Newtonsoft.Json; 

namespace DAlertsApi.Models.Auth.Inplicit
{
    /// <summary>
    /// The user is given an authorization request (or redirect) to the https://www.donationalerts.com/oauth/authorize with parameters client_id, redirect_uri, response_type and scope.
    /// GET:            https://www.donationalerts.com/oauth/authorize
    /// client_id:               The client ID received from DonationAlerts
    /// redirect_uri:            The URL in your where users will be sent after authorization
    /// response_type=token:     Specifies that application is requesting an access token
    /// scope:                   A space-delimited list of scopes
    /// </summary>
    public class AuthorizationImplicitRequest
    {
        public int Client_id { get; set; }
        public string Redirect_uri { get; set; } = string.Empty;
        public string Response_type { get; set; } = "token";
        public string Scope { get; set; } = string.Empty;

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
