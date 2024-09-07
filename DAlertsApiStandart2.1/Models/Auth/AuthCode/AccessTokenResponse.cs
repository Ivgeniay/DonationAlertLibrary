using Newtonsoft.Json; 

namespace DAlertsApi.Models.Auth.AuthCode
{
    /// <summary>
    /// token_type:         Token type
    /// expires_in:         Token expiration timestamp
    /// access_token:       Access token
    /// refresh_token:      Refresh token

    /// </summary>
    public class AccessTokenResponse
    {
        public string Token_type { get; set; } = string.Empty;
        public int Expires_in { get; set; }
        public string Access_token { get; set; } = string.Empty;
        public string Refresh_token { get; set; } = string.Empty;

        public object Clone()
        {
            return new AccessTokenResponse
            {
                Token_type = Token_type,
                Expires_in = Expires_in,
                Access_token = Access_token,
                Refresh_token = Refresh_token
            };
        }

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
