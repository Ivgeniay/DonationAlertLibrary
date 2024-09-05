using Newtonsoft.Json; 

namespace DAlertsApi.Models.Auth.AuthCode
{
    /// <summary>
    /// token_type:         Token type
    /// expires_in:         Token expiration timestamp
    /// access_token:       Access token
    /// refresh_token:      Refresh token
    /// </summary>
    public class AuthorizationResponse
    {
        public string Token_type { get; set; } = string.Empty;
        public int Expires_in { get; set; }
        public string Access_token { get; set; } = string.Empty;
        public string Refresh_token { get; set; } = string.Empty;

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
