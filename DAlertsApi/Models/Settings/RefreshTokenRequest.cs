namespace DAlertsApi.Models.Settings
{
    public class RefreshTokenRequest
    {
        public string Grant_type { get; set; } = "refresh_token";
        public string Refresh_token { get; set; } = string.Empty;
        public int Client_id { get; set; }
        public string Client_secret { get; set; } = string.Empty;
        public string Scope { get; set; } = string.Empty;

        public override string ToString()
        {
            return  "RefreshTokenRequest:\n" +
                    $"Grant_type: {Grant_type}\n" +
                    $"Refresh_token: {Refresh_token}\n" +
                    $"Client_id: {Client_id}\n" +
                    $"Client_secret: {Client_secret}\n" +
                    $"Scope: {Scope}\n";
        }
    }
}
