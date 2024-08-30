using Newtonsoft.Json;

namespace DAlertsApi.Models.Auth.Inplicit
{
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
