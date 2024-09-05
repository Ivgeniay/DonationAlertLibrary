﻿using Newtonsoft.Json;

namespace DAlertsApi.Models.ApiV1.Alerts
{
    /// <summary>
    /// Custom alerts are the fully content-customizable alerts that allow the developer to create uniquely designed alerts and send it to the streamer's broadcast.
    ///
    ///It is required for the streamer to have a variation for the Alerts widget with "Custom alerts" type for custom alerts to display. 
    ///Sends custom alert to the authorized user. Requires user authorization with the oauth-custom_alert-store scope. 
    /// 
    /// id:             The unique custom alert identifier
    /// external_id:    Unique alert ID generated by the application developer.Or null if ID was not provided
    /// header:         Text that will be displayed as a header. Or null if text was not provided
    /// message:        Text that will be displayed inside the message box. Or null if text was not provided
    /// image_url:      URL to the image file that will displayed along with the custom alert. Or null if URL was not provided
    /// sound_url:      URL to the sound file that will played when displaying the custom alert. Or null if URL was not provided
    /// is_shown:       A flag indicating whether the alert was shown in the streamer's widget
    /// created_at:     The date and time (YYYY-MM-DD HH.MM.SS formatted) when custom alert was created
    /// shown_at:       Date and time indicating when the alert was shown(YYYY-MM-DD HH.MM.SS formatted). Or null if the alert is not shown yet
    /// </summary>
    public class CustomAlertsResponse
    {
        public int Id { get; set; }
        public string? External_id { get; set; }
        public string? Header { get; set; }
        public string? Message { get; set; }
        public string? Image_url { get; set; }
        public string? Sound_url { get; set; }
        public int Is_show { get; set; }
        public string Created_at { get; set; } = string.Empty;
        public string? Shown_at { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }

    public class CustomAlertsResponseWrap
    {
        public CustomAlertsResponse Data { get; set; } = new CustomAlertsResponse();
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
