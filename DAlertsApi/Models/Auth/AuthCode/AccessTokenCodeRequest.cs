using Newtonsoft.Json;

namespace DAlertsApi.Models.Auth.AuthCode
{
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