using DAlertsApi.DTOs;
using DAlertsApi.Logger;
using DAlertsApi.Mappers;
using DAlertsApi.Models;
using DAlertsApi.Models.Auth.AuthCode;
using DAlertsApi.Models.Settings;
using DAlertsApi.SystemFunc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Net.Mime.MediaTypeNames;
using System.Net;
using DAlertsApi.Models.Auth.AuthCode.Refresh;

namespace DAlertsApi.Auth
{
    public delegate void AccessTokenGetted(AccessTokenResponseDTO response);
    public delegate void RefreshTokenGetted(RefreshTokenRequest request, RefreshTokenResponse response);

    /// <summary>
    /// https://www.donationalerts.com/apidoc#authorization__authorization_code
    /// The authorization code grant type used because it is optimized for server-side applications, where source code is not publicly exposed, and client secret confidentiality can be maintained. This is a redirection-based flow, which means that the application must be capable of interacting with the user-agent and receiving API authorization codes that are routed through the user-agent.
    ///
    /// 1. Application registration (https://www.donationalerts.com/application/clients);
    /// 2. Authorization request (redirect user to GetAuthorizationUrl());
    /// 3. Getting access code (getting code from response query)
    /// 4. Authorization code to access token exchange (getting access token from GetAccessTokenAsync(AccessTokenCodeRequest request))
    /// </summary>
    public class DonationAlertsGrandTypeAuth
    {
        public AccessTokenGetted OnAccessTokenGetted;
        public RefreshTokenGetted OnRefreshTokenGetted;

        private readonly Credentials credentials;
        private readonly ILogger logger;

        public DonationAlertsGrandTypeAuth(Credentials credentials)
        {
            this.credentials = credentials;
        }
        public DonationAlertsGrandTypeAuth(Credentials credentials, ILogger logger)
        {
            this.credentials = credentials;
            this.logger = logger;
        }

        public string GetAuthorizationUrl()
        {
            var link = $"{Links.AuthorizationEndpoint}?client_id={credentials.ClientId}&redirect_uri={StaticMethods.GetUrl(credentials.Redirect, credentials.Port)}&response_type=code";
            if (credentials.Scope.Length > 0) link += "&scope=" + Scope.GetScopeToQueryString(credentials.Scope);
            return link;
        }
        public string GetAuthorizationUrl(AuthorizationCodeRequest authCodeRequest)
        {
            var link = $"{Links.AuthorizationEndpoint}?client_id={authCodeRequest.Client_id}&redirect_uri={authCodeRequest.Redirect_uri}&response_type={authCodeRequest.Response_type}";
            if (authCodeRequest.Scope.Length > 0) link += authCodeRequest.Scope;
            return link;
        }
        public async Task<AccessTokenResponse?> GetAccessTokenAsync(AccessTokenCodeRequest request)
        {
            using (var client = new HttpClient())
            {
                string content = await GetContentAsync(request, client);
                if (content == null) return null;

                AccessTokenResponse? tokenResponse = JsonConvert.DeserializeObject<AccessTokenResponse>(content);
                if (tokenResponse != null)
                {
                    var response = (AccessTokenResponse)tokenResponse.Clone();
                    OnAccessTokenGetted?.Invoke(response.FromAccessTokenResponseToAccessTokenResponseDTO());
                }
                return tokenResponse;
            }
        }
        public async Task<RefreshTokenResponse?> GetRefreshTokenAsync(RefreshTokenRequest request)
        {
            using (var client = new HttpClient())
            {
                string content = await GetContentAsync(request, client);
                if (content == null) return null;

                RefreshTokenResponse? response = JsonConvert.DeserializeObject<RefreshTokenResponse>(content);
                if (response != null)
                {
                    OnRefreshTokenGetted?.Invoke(request, response);
                }

                return response;
            }
        }
        private async Task<string> GetContentAsync(object request, HttpClient client)
        {
            IEnumerable<KeyValuePair<string, string>> dic = StaticMethods.FromClassToDictionary(request);
            var requestContent = new FormUrlEncodedContent(dic);

            var response = await client.PostAsync(Links.TokenEndpoint, requestContent);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                logger?.Log($"Failed to retrieve token: status: {response.StatusCode}, content: {errorContent}", LogLevel.Error);
                return null;
            }
            return await response.Content.ReadAsStringAsync();
        }
    }
}
