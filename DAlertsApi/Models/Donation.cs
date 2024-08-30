namespace DAlertsApi.Models
{
    public class Donation
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public decimal Amount { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
