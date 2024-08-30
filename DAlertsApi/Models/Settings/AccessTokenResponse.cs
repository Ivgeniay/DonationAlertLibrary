﻿
namespace DAlertsApi.Models.Settings
{
    public class AccessTokenResponse
    {
        public string Token_type { get; set; } = string.Empty;
        public int Expires_in { get; set;}
        public string Access_token { get; set;} = string.Empty;
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

        public override string ToString()
        {
            return "AccessTokenResponse:\n" +
                   "Token_type: " + Token_type + "\n" +
                   "Expires_in: " + Expires_in + "\n" +
                   "Access_token: " + Access_token + "\n" +
                   "Refresh_token: " + Refresh_token + "\n";
        }
    }
}
