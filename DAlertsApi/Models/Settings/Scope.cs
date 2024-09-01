namespace DAlertsApi.Models.Settings
{
    /// <summary>
    /// ApiV1 scops:
    /// 
    /// oauth-user-show:                Obtain profile data (Model: User, UserWrap)
    /// oauth-donation-index:           View donations (Models: Donation, DonationWrap)
    /// oauth-custom_alert-store:       Create custom alerts (Models: CustomAlertsResponse, CustomAlertsRequest)
    /// 
    /// Centrifugo scopes:
    /// 
    /// oauth-donation-subscribe:       Subscribe to new donation alerts
    /// oauth-goal-subscribe:           Subscribe to donation goals updates
    /// oauth-poll-subscribe:           Subscribe to polls updates
    /// </summary>
    public static class Scope
    {
        public static readonly Dictionary<ScopeType, string> Scopes = new()
            {
                { ScopeType.OauthUserShow, "oauth-user-show" },
                { ScopeType.OauthDonationSubscribe ,"oauth-donation-subscribe" },
                { ScopeType.OauthDonationIndex ,"oauth-donation-index" },
                { ScopeType.OauthCustomAlertStore ,"oauth-custom_alert-store" },
                { ScopeType.OauthGoalSubscribe ,"oauth-goal-subscribe" },
                { ScopeType.OauthPollSubscribe ,"oauth-poll-subscribe" }
            };

        /// <summary>
        /// Return string with scopes separated by symbol '+'
        /// </summary>
        /// <param name="scopeType"></param>
        /// <returns></returns>
        public static string GetScopeToQueryString(params ScopeType[] scopeType)
        {
            string result = string.Empty;
            for (int i = 0; i < scopeType.Length; i++)
            {
                result += Scopes[scopeType[i]];
                if (i != scopeType.Length - 1) result += "+";
            }
            return result;
        }

        /// <summary>
        /// Return string with scopes separated by symbol ' '
        /// </summary>
        /// <param name="scopeType"></param>
        /// <returns></returns>
        public static string GetScopeToString(params ScopeType[] scopeType)
        {
            string result = string.Empty;
            for (int i = 0; i < scopeType.Length; i++)
            {
                result += Scopes[scopeType[i]];
                if (i != scopeType.Length - 1) result += " ";
            }
            return result;
        }
    }

    public enum ScopeType
    {
        OauthUserShow,
        OauthDonationSubscribe,
        OauthDonationIndex,
        OauthCustomAlertStore,
        OauthGoalSubscribe,
        OauthPollSubscribe
    }
}
