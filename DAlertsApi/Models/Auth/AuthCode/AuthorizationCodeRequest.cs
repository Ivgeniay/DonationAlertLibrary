using Newtonsoft.Json;

namespace DAlertsApi.Models.Auth.AuthCode
{
    public class AuthorizationCodeRequest
    {
        public int Client_id { get; set; }
        public string Redirect_uri { get; set; } = string.Empty;
        public string Response_type { get; set; } = "code";
        public string Scope { get; set; } = string.Empty;

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
