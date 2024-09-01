using Newtonsoft.Json;

namespace DAlertsApi.Models.Centrifugo
{
    /// <summary>
    /// Centrifugo connection response
    /// </summary>
    public class ConnectChannelResponse
    {
        public Result result { get; set; } = new Result();

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

        public class Result
        {
            public int type { get; set; }
            public string channel { get; set; } = string.Empty;
            public Data data { get; set; } = new Data();
        }
        public class Info
        {
            public string user { get; set; } = string.Empty;
            public string client { get; set; } = string.Empty;
        }

        public class Data
        {
            public Info info { get; set; } = new Info();
        }
    }
}
