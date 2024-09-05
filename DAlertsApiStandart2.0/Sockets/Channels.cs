using System;
using System.Collections.Generic;

namespace DAlertsApi.Sockets
{

    /// <summary>
    /// DonationAlerts API offers the variety of Centrifugo channels for receiving real-time event notifications.
    /// https://www.donationalerts.com/apidoc#api_v1__centrifugo_channels
    /// Each message sent to a channel carries the reason attribute in addition to the original resource. This attribute describes the event that occured upon message dispatch.
    /// </summary>
    public static class Channels
    {
        private static string allerts = "$alerts:donation_<user_id>";
        private static string goals = "$goals:goal_<user_id>";
        private static string pools = "$polls:poll_<user_id>";

        public static string[] GetChannels(int userId, params ChannelsType[] types)
        {
            List<string> channels = new List<string>();
            foreach (var type in types)
            {
                channels.Add(GetChannel(type, userId));
            }
            return channels.ToArray();
        }

        public static string GetChannel(ChannelsType type, int userId)
        {
            return type switch
            {
                ChannelsType.Allerts => allerts.Replace("<user_id>", userId.ToString()),
                ChannelsType.Goals => goals.Replace("<user_id>", userId.ToString()),
                ChannelsType.Pools => pools.Replace("<user_id>", userId.ToString()),
                _ => throw new ArgumentException("Unknown channel type")
            };
        }
    }

    public enum ChannelsType
    {
        Allerts,
        Goals,
        Pools
    }
}
