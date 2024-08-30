namespace DAlertsApi.Models.Settings
{
    public class AutorizationRequest
    {
        public int Client_id { get; set; }
        public string Redirect_uri { get; set; } = string.Empty;
        public string Response_type { get; set; } = "code";
        public string Scope { get; set; } = string.Empty;
    }
}
