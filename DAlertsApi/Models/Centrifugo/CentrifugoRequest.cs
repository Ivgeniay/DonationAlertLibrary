namespace DAlertsApi.Models.Centrifugo
{
    public class CentrifugoRequest
    {
        public StartMessageParams @params { get; set; } = new StartMessageParams();
        public int id { get; set; }
    }

    public class StartMessageParams
    {
        public string token { get; set; }
    }
}
