using Newtonsoft.Json;

namespace DAlertsApi.Models.Centrifugo
{
    public class CentrifugoResponse
    {
        public int Id { get; set; }
        public ResultData Result { get; set; } = new();

        public class ResultData
        {
            /// <summary>
            /// uuidv4_client_id
            /// </summary>
            public string Client { get; set; } = string.Empty;
            public string Version { get; set; } = string.Empty;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
