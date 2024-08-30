using Newtonsoft.Json;

namespace DAlertsApi.Models.Auth.AuthCode
{
    public class CodeModel
    {
        public string Code { get; set; } = string.Empty;

        override public string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
