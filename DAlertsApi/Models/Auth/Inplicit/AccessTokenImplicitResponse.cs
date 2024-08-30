using Newtonsoft.Json;

namespace DAlertsApi.Models.Auth.Inplicit
{
    /// <summary>
    /// If the user allows access to personal data, the service redirects the user-agent to the application redirect URI, which was specified during the client registration, along with an access token. The access token will be available as the value of the access_token parameter in the hash part of the URL.
    /// </summary>
    public class AccessTokenImplicitResponse
    {
        public string Access_token { get; set; } = string.Empty;

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
