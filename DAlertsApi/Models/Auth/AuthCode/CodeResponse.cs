using Newtonsoft.Json;

namespace DAlertsApi.Models.Auth.AuthCode
{
    /// <summary>
    /// If the user allows access to personal data, the service redirects the user-agent to the application redirect URI, which was specified during the client registration, along with an authorization code. The authorization code will be available as the value of the code parameter.
    /// </summary>
    public class CodeModel
    {
        public string Code { get; set; } = string.Empty;

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
