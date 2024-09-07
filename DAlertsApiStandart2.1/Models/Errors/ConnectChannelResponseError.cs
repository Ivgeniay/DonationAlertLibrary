using Newtonsoft.Json;

namespace DAlertsApi.Models.Errors
{
    public class ConnectChannelResponseError
    {
        public int id { get; set; }
        public Result result { get; set; } = new Result();

        public class Result
        {
            public bool recoverable { get; set; }
            public string epoch { get; set; } = string.Empty;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
