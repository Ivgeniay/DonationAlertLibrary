using DAlertsApi.Models.Data;
using DAlertsApi.Models.Pagination; 

namespace DAlertsApi.Models.Centrifugo
{
    /// <summary>
    /// id:             The unique donation alert identifier
    /// name:           Type of the alert.Always donation in this case
    /// username:       The name of the user who sent the donation and the alert
    /// message_type:   The message type.The possible values are text for a text messages and audio for an audio messages
    /// message:        The message sent along with the donation and the alert
    /// amount:         The donation amount
    /// currency:       The currency code (ISO 4217 formatted)
    /// is_shown:       A flag indicating whether the alert was shown in the streamer's widget
    /// created_at:     The donation date and time(YYYY-MM-DD HH.MM.SS formatted)
    /// shown_at:       Date and time indicating when the alert was shown(YYYY-MM-DD HH.MM.SS formatted). Or null if the alert is not shown yet
    /// </summary>
    public class Donation
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Message_type { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public float Amount { get; set; }
        public CurrenciesType Currency { get; set; }
        public int Is_shown { get; set; }
        public string Created_at { get; set; } = string.Empty;
        public string? Shown_at { get; set; }
    }


    /// <summary>
    /// DonationWrap is a wrapper for Donation model that contains links and meta information
    /// </summary>
    public class DonationWrap
    {
        public Donation Data { get; set; } = new();
        public PaginationLinks Links { get; set; } = new();
        public PaginationInfo Meta { get; set; } = new();
    }
}
