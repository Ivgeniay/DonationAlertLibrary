using Newtonsoft.Json; 

namespace DAlertsApi.Models.Centrifugo
{
    public class CoonectToChannelFirstMessage
    {
        public int Id { get; set; }
        public Result Result { get; set; } = new Result();

        override public string ToString() => JsonConvert.SerializeObject(this);
        
    }
    public class Result
    {
        public bool Recoverable { get; set; }
        public string Epoch { get; set; } = string.Empty;
    }

}
