using System.Net.Http.Headers;
using DAlertsApi.Logger;
using Newtonsoft.Json;
using DAlertsApi.Models;
using DAlertsApi.Models.ApiV1.Users;
using DAlertsApi.Models.ApiV1.Alerts;
using DAlertsApi.Models.ApiV1.Donations;
using DAlertsApi.Models.ApiV1.Merchandise;

namespace DAlertsApi.ApiV1lib
{
    /// <summary>
    /// ApiV1 class is used to interact with the Donation Alerts API v1.
    /// https://www.donationalerts.com/apidoc#api_v1
    /// </summary>
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
            catch (Exception ex)
            {
                logger?.Log($"Error while getting user profile: {ex.Message}", LogLevel.Error);
            }

            return null;
        }
        public async Task<DonationWrap?> GetDonationsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(Links.DonationAlertsListEndpoint);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var donations = JsonConvert.DeserializeObject<DonationWrap>(content);
                    logger?.Log($"Donations received: {donations?.Data.Count}");
                    return donations;
                }
            }
            catch (Exception ex)
            {
                logger?.Log($"Error while getting donations: {ex.Message}", LogLevel.Error);
            }

            return null;
        }
        public async Task<CustomAlertsResponseWrap?> PostCustomAlertAsync(CustomAlertsRequest request, string bearerToken)
        {
            using (var _httpClient = new HttpClient())
            {
                try
                {
                    var json = JsonConvert.SerializeObject(request);

                    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
                    _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    var requestData = new Dictionary<string, string>
                    {
                        { "external_id", request.External_id },
                        { "header", request.Header },
                        { "message", request.Message },
                        { "is_show", request.Is_show.ToString() },
                        { "image_url", request.Image_url },
                        { "sound_url", request.Sound_url }
                    };
                    var content = new FormUrlEncodedContent(requestData);
                    var response = await _httpClient.PostAsync(Links.CustomAlertsEndpoint, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        var customAlertResponse = JsonConvert.DeserializeObject<CustomAlertsResponseWrap>(responseContent);
                        logger?.Log($"Custom alert posted: {customAlertResponse?.Data}");
                        return customAlertResponse;
                    }
                }
                catch (Exception ex)
                {
                    logger?.Log($"Error while posting custom alert: {ex.Message}", LogLevel.Error);
                } 
                return null;
            }
        }
        public async Task<CreateMerchandiseResponse?> CreateMerchandiseAsync(CreateMerchandiseRequest request, string token)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                using (var formContent = new MultipartFormDataContent())
                {
                    formContent.Add(new StringContent(request.Merchant_identifier), "merchant_identifier");
                    formContent.Add(new StringContent(request.Merchandise_identifier), "merchandise_identifier"); 
                    foreach (var title in request.Title) formContent.Add(new StringContent(title.Value), $"title[{title.Key}]");
                    formContent.Add(new StringContent(request.Is_active.ToString()), "is_active");
                    formContent.Add(new StringContent(request.Is_percentage.ToString()), "is_percentage");
                    formContent.Add(new StringContent(request.Currency.ToString()), "currency");
                    formContent.Add(new StringContent(request.Price_user.ToString()), "price_user");
                    formContent.Add(new StringContent(request.Price_service.ToString()), "price_service");
                    formContent.Add(new StringContent(request.Url), "url");
                    formContent.Add(new StringContent(request.Img_url), "img_url");
                    formContent.Add(new StringContent(request.signature), "signature");

                    var response = await client.PostAsync(Links.MerchandiseEndpoint, formContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<CreateMerchandiseResponse>(content);
                    }
                    else
                    {
                        logger?.Log($"Error from {nameof(CreateMerchandiseAsync)} : {response.StatusCode}, {await response.Content.ReadAsStringAsync()}", LogLevel.Error);
                        return null;
                    }
                }
            }
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
