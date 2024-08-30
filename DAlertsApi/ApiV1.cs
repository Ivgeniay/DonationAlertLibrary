using DAlertsApi.Models.ApiV1; 
using System.Net.Http.Headers; 
using DAlertsApi.Logger;
using Newtonsoft.Json;
using DAlertsApi.Models;

namespace DAlertsApi
{
    public class ApiV1 : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger logger;

        public ApiV1(ILogger logger, string token)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public async Task<UserWrap?> GetUserProfileAsync()
        {
            try
            { 
                var response = await _httpClient.GetAsync(Links.UserOauthEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    UserWrap? user = JsonConvert.DeserializeObject<UserWrap>(content);
                    logger?.Log($"User profile received: {user?.Data}");
                    return user;
                }
            }
            catch(Exception ex)
            {
                logger?.Log($"Error while getting user profile: {ex.Message}", LogLevel.Error);
            }

            return null;
        }

        public async Task<DonationsWrap?> GetDonationsAsync()
        {
            try
            { 
                var response = await _httpClient.GetAsync(Links.DonationAlertsListEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var donations = JsonConvert.DeserializeObject<DonationsWrap>(content);
                    logger?.Log($"Donations received: {donations?.Data.Count}");
                    return donations;
                }
            }
            catch(Exception ex)
            {
                logger?.Log($"Error while getting donations: {ex.Message}", LogLevel.Error);
            }

            return null;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
