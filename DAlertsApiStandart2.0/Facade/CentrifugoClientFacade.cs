using DAlertsApi.Models.Auth.AuthCode.Refresh;
using DAlertsApi.Models.ApiV1.Donations;
using DAlertsApi.Models.Auth.AuthCode;
using DAlertsApi.Models.ApiV1.Users;
using DAlertsApi.Models.Centrifugo;
using DAlertsApi.Models.Settings;
using DAlertsApi.Mappers;
using DAlertsApi.Sockets;
using DAlertsApi.Logger;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;

namespace DAlertsApi.Facade
{
    /// <summary>
    /// Simple solution for starting Centrifugo client and taking server messages.
    /// </summary>
    public class CentrifugoClientFacade
    {
        public event Action<DonationWrapper>? OnDonationReceived;
        public event Action<WebSocketMessage>? OnMessageReceived;
        public event Action<GoalsUpdateWrapper>? OnGoalUpdated;
        public event Action<PollDataWrapper>? OnPollUpdated;
        public event Action<GoalInfo>? OnGoalLaunchUpdate;

        private AccessTokenResponse accessTokenResponse;

        private readonly CentrifugoClient centrifugoClient;
        private readonly Credentials credentials;
        private readonly User userWrap;
        private readonly ILogger? logger;

        public CentrifugoClientFacade(Credentials credentials, User userWrap, AccessTokenResponse accesTokenResponse, ILogger? logger = null)
        {
            centrifugoClient = new CentrifugoClient(logger);

            centrifugoClient.OnPollUpdated += (pollDataWrapper) => OnPollUpdated?.Invoke(pollDataWrapper);
            centrifugoClient.OnGoalUpdated += (goalsUpdateWrapper) => OnGoalUpdated?.Invoke(goalsUpdateWrapper);
            centrifugoClient.OnGoalLaunchUpdate += (goalInfo) => OnGoalLaunchUpdate?.Invoke(goalInfo);
            centrifugoClient.OnDonationReceived += (donation) => OnDonationReceived?.Invoke(donation);
            centrifugoClient.OnMessageReceived += (message) => OnMessageReceived?.Invoke(message);

            this.credentials = credentials;
            this.userWrap = userWrap;
            accessTokenResponse = accesTokenResponse;
        }

        public async Task StartAsync()
        {
            bool isConnected = await centrifugoClient.ConnectAsync(userWrap.Socket_connection_token, userWrap.Id);
            if (isConnected)
            {
                CentrifugoResponse? response = await centrifugoClient.ReceiveClientIdAsync();
                if (response != null && !string.IsNullOrEmpty(response.Result?.Client))
                {
                    logger?.Log($"Successfully authenticated with Client ID: {response.Result.Client}");
                    string clientId = response.Result.Client;
                    string[] channels = Channels.GetChannels(userWrap.Id, ChannelsType.Allerts, ChannelsType.Goals, ChannelsType.Pools);
                    SubscriptionRequest subscribeRequest = new SubscriptionRequest()
                    {
                        Client = clientId,
                        Channels = channels.ToList()
                    };
                    SubscriptionResponse? response1 = await centrifugoClient.SubscribeToChannelsAsync(subscribeRequest, accessTokenResponse.Access_token);
                    if (response1 != null)
                    {
                        logger?.Log($"Successfully subscribed to channels: {string.Join(", ", response1.Channels)}");
                        int messageId = 1;
                        int methodId = 1;
                        foreach (var channel in response1.Channels)
                        {
                            CancellationToken cancellationToken = CancellationToken.None;
                            bool isConnectedChannel = await centrifugoClient.ConnectToChannelAsync(channel.Channel, channel.Token, methodId, ++messageId, cancellationToken);
                            if (isConnectedChannel) { }
                        }
                        logger?.Log("Listening for messages...");
                        await centrifugoClient.ListenForMessagesAsync(CancellationToken.None);
                    }
                }
                else logger?.Log("Failed to authenticate with Centrifugo. (ReceiveClientId)", LogLevel.Error);
            }
            else logger?.Log("Failed to establish a connection with Centrifugo.", LogLevel.Error);
        }
        public void UpdateAccessToken(RefreshTokenResponse refreshTokenResponse) =>        
            accessTokenResponse = accessTokenResponse.FromRefreshTokenResponseToAccessTokenResponse(refreshTokenResponse);
        
    }
}
