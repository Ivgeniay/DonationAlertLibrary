using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAlertsApi.Models.Auth.AuthCode.Refresh
{
    public class RefreshTokenResponse
    {
        public string Token_type { get; set; } = string.Empty;
        public int Expires_in { get; set; }
        public string Access_token { get; set; } = string.Empty;
        public string Refresh_token { get; set; } = string.Empty;

        public override string ToString()
        {
            return "RefreshTokenResponse:\n" +
                    $"Token_type: {Token_type}\n" +
                    $"Expires_in: {Expires_in}\n" +
                    $"Access_token: {Access_token}\n" +
                    $"Refresh_token: {Refresh_token}\n";
        }
    }
}
