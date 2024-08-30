namespace DAlertsApi.Models.Settings
{
    public class AutorizationResponse
    {
        public string Token_type { get; set; } = string.Empty;
        public int Expires_in { get; set; }
        public string Access_token { get; set; } = string.Empty;
        public string Refresh_token { get; set; } = string.Empty;

        public override string ToString()
        {
            return "AutorizationResponse:\n" +
                   "Token_type: " + Token_type + "\n" +
                   "Expires_in: " + Expires_in + "\n" +
                   "Access_token: " + Access_token + "\n" +
                   "Refresh_token: " + Refresh_token + "\n";
        }
    }
}
