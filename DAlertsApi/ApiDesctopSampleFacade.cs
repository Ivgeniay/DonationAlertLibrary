using DAlertsApi.Models.Auth.AuthCode;
using DAlertsApi.Models.ApiV1.Users;
using DAlertsApi.Models.Centrifugo;
using DAlertsApi.Models.Settings;
using DAlertsApi.SystemFunc;
using DAlertsApi.ApiV1lib;
using DAlertsApi.Sockets;
using DAlertsApi.Logger;
using DAlertsApi.Auth;

namespace DAlertsApi
{
    public class ApiDesctopSampleFacade
    {
        public CentrifugoClient centrifugoClient;
        private readonly Credentials credentials;
        private readonly ILogger? logger;
        public ApiDesctopSampleFacade(Credentials credentials, ILogger logger) : this(credentials)
        {
            this.logger = logger;
            centrifugoClient = new(logger);
        }

        public ApiDesctopSampleFacade(Credentials credentials) 
        {
            this.credentials = credentials; 
        }

        public async void Start()
        {
            DonationAlertsGrandTypeAuth alertsAuth = new(credentials, logger);
            SimpleServer simpleServer = new(credentials.Redirect, credentials.Port, logger);
            simpleServer.Start();

            logger.Log("Opening browser...");
            int idProcess = OpenProcess.Open(
                alertsAuth.GetAuthorizationUrl(),
                logger);

            await Task.Run(() =>
            { logger.Log("Waiting for code..."); Thread.Sleep(1500); });

            Task<CodeModel> serverCodeAwaiter = simpleServer.AwaitCode();
            CodeModel codeModel = await serverCodeAwaiter;
            logger.Log(codeModel.ToString() ?? "null");
            simpleServer.Dispose();
            OpenProcess.Close(idProcess, logger);

            AccessTokenCodeRequest accessTokenRequest = new()
            {
                Client_id = credentials.ClientId,
                Client_secret = credentials.ClientSecret,
                Code = codeModel.Code,
                Redirect_uri = StaticMethods.GetUrl(credentials.Redirect, credentials.Port)
            };
            logger.Log(accessTokenRequest.ToString() ?? "null");
            AccessTokenResponse? accesTokenResponse = await alertsAuth.GetAccessTokenAsync(accessTokenRequest);
            logger.Log(accesTokenResponse?.ToString() ?? "null");

            ApiV1 apiV1 = new(logger, accesTokenResponse.Access_token);
            UserWrap? userWrap = await apiV1.GetUserProfileAsync();

            if (centrifugoClient == null) centrifugoClient = new(logger);
            bool isConnected = await centrifugoClient.ConnectAsync(userWrap.Data.Socket_connection_token, userWrap.Data.Id);
            if (isConnected)
            {
                CentrifugoResponse? response = await centrifugoClient.ReceiveClientIdAsync();
                if (response != null && !string.IsNullOrEmpty(response.Result?.Client))
                {
                    logger?.Log($"Successfully authenticated with Client ID: {response.Result.Client}");

                    string clientId = response.Result.Client;
                    string[] channels = Channels.GetChannels(userWrap.Data.Id, ChannelsType.Allerts, ChannelsType.Goals, ChannelsType.Pools);
                    SubscriptionRequest request = new()
                    {
                        Client = clientId,
                        Channels = channels.ToList()
                    };
                    SubscriptionResponse response1 = await centrifugoClient.SubscribeToChannelsAsync(request, accesTokenResponse.Access_token);
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

                        logger.Log("Listening for messages...");
                        await centrifugoClient.ListenForMessagesAsync(CancellationToken.None);
                    }
                }
            }
            else logger.Log("Failed to establish a connection with Centrifugo.", LogLevel.Error);
        }
    }
}
