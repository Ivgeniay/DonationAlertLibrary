using Newtonsoft.Json;

namespace DAlertsApi.Models.Centrifugo
{
    public class CentrifugoRequest
    {
        public StartMessageParams Params { get; set; } = new StartMessageParams();
        public int Id { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class StartMessageParams
    {
        public string Token { get; set; } = string.Empty;
    }

}
