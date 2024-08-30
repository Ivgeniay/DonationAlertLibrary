namespace DAlertsApi.Models.Settings
{
    public class Credentials
    {
        public readonly int ClientId;
        public readonly string ClientSecret;
        public readonly string Redirect;
        public readonly string Port;
        public readonly ScopeType[] Scope;

        public Credentials(int clientId, string clientSecret, string redirect, string port = null, params ScopeType[] scope)
        {
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.Redirect = redirect;
            this.Port = port;
            Scope = scope;
        }
    }
}
