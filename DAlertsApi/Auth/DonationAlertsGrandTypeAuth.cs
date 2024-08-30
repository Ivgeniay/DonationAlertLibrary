using DAlertsApi.DTOs;
using DAlertsApi.Logger;
using DAlertsApi.Mappers;
using DAlertsApi.Models;
using DAlertsApi.Models.Settings;
using DAlertsApi.SystemFunc;
using Newtonsoft.Json;

namespace DAlertsApi.Auth
{
    public delegate void AccessTokenGetted(AccessTokenResponseDTO response);
    public delegate void RefreshTokenGetted(RefreshTokenRequest request, RefreshTokenResponse response);
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

        public string GetAuthorizationUrl(params ScopeType[] scopes)
        {
            var link = $"{Links.AuthorizationEndpoint}?client_id={credentials.ClientId}&redirect_uri={StaticMethods.GetUrl(credentials.Redirect, credentials.Port)}&response_type=code";
            if (scopes != null && scopes.Length > 0)
                link += "&scope=" + Scope.GetScopeToQueryString(scopes);

            return link;
        }
        public async Task<AccessTokenResponse?> GetAccessToken(AccessTokenRequest request)
        {
            using (var client = new HttpClient())
            {
                string content = await GetContent(request, client);
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
        public async Task<RefreshTokenResponse?> GetRefreshToken(RefreshTokenRequest request)
        {
            using (var client = new HttpClient())
            {
                string content = await GetContent(request, client);
                if (content == null) return null;

                RefreshTokenResponse? response = JsonConvert.DeserializeObject<RefreshTokenResponse>(content);
                if (response != null)
                {
                    OnRefreshTokenGetted?.Invoke(request, response);
                }

                return response;
            }
        }

        private async Task<string> GetContent(object request, HttpClient client)
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
