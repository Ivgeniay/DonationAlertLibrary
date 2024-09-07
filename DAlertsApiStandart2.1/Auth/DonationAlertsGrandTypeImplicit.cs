using DAlertsApi.Models.Auth.Inplicit; 
using DAlertsApi.Models.Settings;
using DAlertsApi.SystemFunc; 
using DAlertsApi.Logger; 
using DAlertsApi.Models;

namespace DAlertsApi.Auth
{
    /// <summary>
    /// The implicit grant is similar to the authorization code grant. The token is returned to the client without exchanging an authorization code. This grant is most commonly used for JavaScript or mobile applications where the client credentials can't be securely stored.
    /// 
    /// 1. Application registration (https://www.donationalerts.com/application/clients);
    /// 2. Authorization request (redirect user to GetAuthorizationUrl());
    /// 3. Getting access token (getting token from response query)
    /// </summary>
    public class DonationAlertsGrandTypeImplicit : DonationAlertsAuthBase
    { 

        public DonationAlertsGrandTypeImplicit(Credentials credentials) : base(credentials) { }
        public DonationAlertsGrandTypeImplicit(Credentials credentials, ILogger logger) : this(credentials) { }

        public string GetAuthorizationUrl(AuthorizationImplicitRequest authCodeRequest)
        {
            var link = $"{Links.AuthorizationEndpoint}?client_id={authCodeRequest.Client_id}&redirect_uri={authCodeRequest.Redirect_uri}&response_type={authCodeRequest.Response_type}";
            if (authCodeRequest.Scope.Length > 0) link += authCodeRequest.Scope;

            return link;
        }
        public override string GetAuthorizationUrl()
        {
            var link = $"{Links.AuthorizationEndpoint}?client_id={credentials.ClientId}&redirect_uri={StaticMethods.GetUrl(credentials.Redirect, credentials.Port)}&response_type=token";
            if (credentials.Scope.Length > 0) link += "&scope=" + Scope.GetScopeToQueryString(credentials.Scope); 
            return link;
        } 
    }
}
