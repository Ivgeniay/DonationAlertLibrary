namespace DAlertsApi.Models.Settings
{
    public class CodeModel
    {
        public string Code { get; set; } = string.Empty;

        override public string ToString()
        {
            return $"Code: {Code}";
        }
    }
}
