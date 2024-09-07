using DAlertsApi.Logger;
using DAlertsApi.Models.Settings;

namespace DAlertsApi.Auth
{
    public abstract class DonationAlertsAuthBase
    {
        protected readonly Credentials credentials;
        protected readonly ILogger? logger;

        public DonationAlertsAuthBase(Credentials credentials)
        {
            this.credentials = credentials;
        }
        public DonationAlertsAuthBase(Credentials credentials, ILogger? logger) : this(credentials)
        {
            this.logger = logger;
        }

        public abstract string GetAuthorizationUrl();
    }
}
