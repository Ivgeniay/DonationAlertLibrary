using DAlertsApi.Logger;
using DAlertsApi.Models.Auth.AuthCode;
using DAlertsApi.Models.Settings;
using DAlertsApi.Models;
using DAlertsApi.Models.Auth.Inplicit;
using Newtonsoft.Json.Linq;
using DAlertsApi.SystemFunc;

namespace DAlertsApi.Auth
{
    public class DonationAlertsGrandTypeImplicit
    {
        private readonly Credentials credentials;
        private readonly ILogger logger;

        public DonationAlertsGrandTypeImplicit(Credentials credentials)
        {
            this.credentials = credentials;
        }
        public DonationAlertsGrandTypeImplicit(Credentials credentials, ILogger logger)
        {
            this.credentials = credentials;
            this.logger = logger;
        }

        public string GetAuthorizationUrl(AuthorizationImplicitRequest authCodeRequest)
        {
            var link = $"{Links.AuthorizationEndpoint}?client_id={authCodeRequest.Client_id}&redirect_uri={authCodeRequest.Redirect_uri}&response_type={authCodeRequest.Response_type}";
            if (authCodeRequest.Scope.Length > 0) link += authCodeRequest.Scope;

            return link;
        }
        public string GetAuthorizationUrl()
        {
            var link = $"{Links.AuthorizationEndpoint}?client_id={credentials.ClientId}&redirect_uri={StaticMethods.GetUrl(credentials.Redirect, credentials.Port)}&response_type=token";
            if (credentials.Scope.Length > 0) link += "&scope=" + Scope.GetScopeToQueryString(credentials.Scope); 
            return link;
        }


    }
}
