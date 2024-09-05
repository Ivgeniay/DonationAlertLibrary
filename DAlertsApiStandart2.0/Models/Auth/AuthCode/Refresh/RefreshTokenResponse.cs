using Newtonsoft.Json;

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
            return JsonConvert.SerializeObject(this);
        }
    }
}
