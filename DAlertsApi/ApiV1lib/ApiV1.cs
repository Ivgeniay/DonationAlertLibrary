using DAlertsApi.Models.ApiV1.Merchandise;
using DAlertsApi.Models.ApiV1.Donations;
using DAlertsApi.Models.ApiV1.Alerts;
using DAlertsApi.Models.ApiV1.Users;
using System.Net.Http.Headers;
using System.Globalization; 
using DAlertsApi.Logger;
using DAlertsApi.Models;
using Newtonsoft.Json;

namespace DAlertsApi.ApiV1lib
{
    /// <summary>
    /// ApiV1 class is used to interact with the Donation Alerts API v1.
    /// https://www.donationalerts.com/apidoc#api_v1
    /// 
    /// Some of the APIs require request SIGNATURE. 
    ///
    /// The request signature is a SHA256 hashed string formed from a alphabetically sorted values of request parameters(with every value interpreted as a string) and appended API client secret key to the end.
    /// For example, if request parameters contain: 
    /// foo=xyz&bar=abc
    /// 
    /// Then the signature must be generated as following: 
    /// SHA256(abc + xyz + <API client secret> ) 
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
        public async Task<CreateMerchandiseResponseWrap?> CreateMerchandiseAsync(CreateMerchandiseRequest request, string bearerToken)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

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
                    formContent.Add(new StringContent(request.Signature), "signature");

                    var response = await client.PostAsync(Links.MerchandiseEndpoint, formContent);

                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        return JsonConvert.DeserializeObject<CreateMerchandiseResponseWrap>(content);
                    }
                    else
                    {
                        logger?.Log($"Error from {nameof(CreateMerchandiseAsync)} : {response.StatusCode}, {await response.Content.ReadAsStringAsync()}", LogLevel.Error);
                        return null;
                    }
                }
            }
        }
        public async Task<UpdateMerchandiseResponseWrap?> UpdateMerchandiseAsync(UpdateMerchandiseRequest request, int merchandiseId, string bearerToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var titleContent = request.Title.Select(t => new KeyValuePair<string, string>($"title[{t.Key}]", t.Value));
            var content = new FormUrlEncodedContent(titleContent.Concat(new[]
            { 
                new KeyValuePair<string, string>("is_active", request.IsActive.ToString()),
                new KeyValuePair<string, string>("is_percentage", request.IsPercentage.ToString()),
                new KeyValuePair<string, string>("currency", request.Currency.ToString()),
                new KeyValuePair<string, string>("price_user", request.PriceUser.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("price_service", request.PriceService.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("url", request.Url),
                new KeyValuePair<string, string>("img_url", request.ImgUrl),
                new KeyValuePair<string, string>("signature", request.Signature)
            }));

            var response = await client.PutAsync($"{Links.MerchandiseEndpoint}/{merchandiseId}", content);

            if (!response.IsSuccessStatusCode)
            {
                logger?.Log($"Error: {response.StatusCode}", LogLevel.Error);
                return null;
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<UpdateMerchandiseResponseWrap>(responseContent);
        }
        public async Task<CreateOrUpdateMerchandiseResponseWrap?> CreateOrUpdateMerchandiseAsync(CreateOrUpdateMerchandiseRequest request, string merchantIdentifier, string merchandiseIdentifier, string bearerToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            var titleContent = request.Title.Select(t => new KeyValuePair<string, string>($"title[{t.Key}]", t.Value)); 
            var content = new FormUrlEncodedContent(titleContent.Concat(new[]
            { 
                new KeyValuePair<string, string>("is_active", request.IsActive.ToString()),
                new KeyValuePair<string, string>("is_percentage", request.IsPercentage.ToString()),
                new KeyValuePair<string, string>("currency", request.Currency.ToString()),
                new KeyValuePair<string, string>("price_user", request.PriceUser.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("price_service", request.PriceService.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("url", request.Url),
                new KeyValuePair<string, string>("img_url", request.ImgUrl),
                new KeyValuePair<string, string>("end_at_ts", request.EndAtTs.ToString()),
                new KeyValuePair<string, string>("signature", request.Signature)
            }));

            var response = await client.PutAsync($"{Links.MerchandiseEndpoint}/{merchantIdentifier}/{merchandiseIdentifier}", content);

            if (!response.IsSuccessStatusCode)
            {
                logger?.Log($"Error: {response.StatusCode}", LogLevel.Error);
                return null;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateOrUpdateMerchandiseResponseWrap>(jsonString);

            return result;
        }
        public async Task<CreateMerchandiseSaleNotificationResponseWrap?> CreateMerchandiseSaleNotificationAsync(CreateMerchandiseSaleNotificationRequest request, string bearerToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken); 

            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("user_id", request.UserId.ToString()),
                new KeyValuePair<string, string>("external_id", request.ExternalId),
                new KeyValuePair<string, string>("merchant_identifier", request.MerchantIdentifier),
                new KeyValuePair<string, string>("merchandise_identifier", request.MerchandiseIdentifier),
                new KeyValuePair<string, string>("amount", request.Amount.ToString(CultureInfo.InvariantCulture)),
                new KeyValuePair<string, string>("currency", request.Currency.ToString()),
                new KeyValuePair<string, string>("bought_amount", request.BoughtAmount.ToString()),
                new KeyValuePair<string, string>("username", request.Username),
                new KeyValuePair<string, string>("message", request.Message),
                new KeyValuePair<string, string>("signature", request.Signature)
            });

            var response = await client.PostAsync(Links.MerchandiseSaleEndpoint, content);

            if (!response.IsSuccessStatusCode)
            {
                logger?.Log($"Error: {response.StatusCode}", LogLevel.Error);
                return null;
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<CreateMerchandiseSaleNotificationResponseWrap>(jsonString);

            return result;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
