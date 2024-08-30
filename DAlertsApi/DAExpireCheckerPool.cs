using DAlertsApi.Auth;
using DAlertsApi.DTOs;
using DAlertsApi.Mappers;
using DAlertsApi.Models.Settings;
using Timer = System.Timers.Timer;

namespace DAlertsApi
{
    public class DAExpireCheckerPool
    {
        /// <summary>
        /// Minimum time for which you need to update the access token (in milliseconds)
        /// </summary>
        public int ExpireToken { get; set; } = 36000;
        /// <summary>
        /// Verification ExpireToken period (in milliseconds)
        /// </summary>
        public int CheckPeriod { get; set; } = 3000;

        private readonly DonationAlertsGrandTypeAuth auth;
        private readonly Credentials credentials;
        private List<AccessTokenResponseDTO> tokens = new();
        private Timer timer;

        public DAExpireCheckerPool(DonationAlertsGrandTypeAuth auth, Credentials credentials, int expireToken)
        {
            this.credentials = credentials;
            this.auth = auth;
            auth.OnAccessTokenGetted += Auth_OnAccessTokenGetted;
            auth.OnRefreshTokenGetted += Auth_OnRefreshTokenGetted;

            ExpireToken = expireToken;
            timer = new Timer();
            timer.Interval = CheckPeriod;
            timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            CheckTokens();
        }

        private void Auth_OnRefreshTokenGetted(RefreshTokenRequest request, RefreshTokenResponse response)
        {
            AccessTokenResponseDTO? token = tokens
                .Where(e => e.Refresh_token == request.Refresh_token)
                .FirstOrDefault();
            if (token != null)
                token = token.FromRefreshTokenResponseToAccessTokenResponseDTO(response);
        }

        private void Auth_OnAccessTokenGetted(AccessTokenResponseDTO response)
        {
            AccessTokenResponseDTO? token = tokens
                .Where(e => e.Access_token == response.Access_token)
                .FirstOrDefault();

            if (token == null) tokens.Add(response);
            else token = response;
        }

        private void CheckTokens()
        {
            DateTime now = DateTime.Now;
            foreach (var token in tokens)
            {
                if (token.ExperedDate < now.AddSeconds(ExpireToken))
                {
                    var request = new RefreshTokenRequest
                    {
                        Refresh_token = token.Refresh_token,
                        Client_id = credentials.ClientId,
                        Client_secret = credentials.ClientSecret,
                    };
                    if (credentials.Scope != null) request.Scope = Scope.GetScopeToString(credentials.Scope);
                    auth?.GetRefreshToken(request);
                }
            }
        }
    }
}
