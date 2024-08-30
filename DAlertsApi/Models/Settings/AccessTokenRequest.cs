namespace DAlertsApi.Models.Settings
{
    public class AccessTokenRequest
    {
        public string Grant_type { get; set; } = "authorization_code";
        public int Client_id { get; set; }
        public string Client_secret { get; set; } = string.Empty;
        public string Redirect_uri { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;

        public override string ToString()
        {
            return "AccessTokenRequest:\n" +
                   "Grant_type: " + Grant_type + "\n" +
                   "Client_id: " + Client_id + "\n" +
                   "Client_secret: " + Client_secret + "\n" +
                   "Redirect_uri: " + Redirect_uri + "\n" +
                   "Code: " + Code + "\n";
        }
    }
}