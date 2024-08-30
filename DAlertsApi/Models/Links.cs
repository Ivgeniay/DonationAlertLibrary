namespace DAlertsApi.Models
{
    public static class Links
    {
        public static string AuthorizationEndpoint { get; } = "https://www.donationalerts.com/oauth/authorize";
        public static string TokenEndpoint { get; } = "https://www.donationalerts.com/oauth/token";
        public static string UserOauthEndpoint { get; } = "https://www.donationalerts.com/api/v1/user/oauth";
        public static string DonationAlertsListEndpoint { get; } = "https://www.donationalerts.com/api/v1/alerts/donations";
        public static string CentrifugoSocketEndpoint { get; } = "wss://centrifugo.donationalerts.com/connection/websocket";
        public static string CentrifugoPrivateSubscribe { get; } = "https://www.donationalerts.com/api/v1/centrifuge/subscribe";
    }
}
