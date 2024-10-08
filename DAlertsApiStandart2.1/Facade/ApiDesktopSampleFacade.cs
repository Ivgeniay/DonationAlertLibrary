﻿using DAlertsApi.Models.ApiV1.Donations;
using DAlertsApi.Models.Auth.AuthCode;
using DAlertsApi.Models.ApiV1.Users;
using DAlertsApi.Models.Settings;
using DAlertsApi.SystemFunc;
using DAlertsApi.ApiV1lib;
using DAlertsApi.Logger;
using DAlertsApi.Auth;
using DAlertsApi.Models.Centrifugo;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace DAlertsApi.Facade
{
    /// <summary>
    /// Simple solution for getting access token and start Centrifugo client.
    /// </summary>
    public class ApiDesktopSampleFacade
    {
        /// <summary>
        /// Centrifugo event returning any received message from WebSocket Donation Allerts
        /// </summary>
        public event Action<WebSocketMessage>? OnMessageReceived;
        /// <summary>
        /// Centrifugo event returning any Goals message from WebSocket Donation Allerts
        /// </summary>
        public event Action<GoalsUpdateWrapper>? OnGoalUpdated;
        /// <summary>
        /// Centrifugo event returning any Poll message from WebSocket Donation Allerts
        /// </summary>
        public event Action<PollDataWrapper>? OnPollUpdated;
        /// <summary>
        /// Centrifugo event returning any Goal Launch message from WebSocket Donation Allerts
        /// </summary>
        public event Action<GoalInfo>? OnGoalLaunchUpdate;
        /// <summary>
        /// Centrifugo event returning any Donation message from WebSocket Donation Allerts
        /// </summary>
        public event Action<DonationWrapper>? OnDonationReceived;

        /// <summary>
        /// AuthCode delegate to obtain Access Token
        /// </summary>
        public AccessTokenGetted OnAccessTokenGetted;
        /// <summary>
        /// AuthCode delegate to obtain Refresh Token information
        /// </summary>
        public RefreshTokenGetted OnRefreshTokenGetted;

        private CentrifugoClientFacade CentrifugoClientFacade;
        private DonationAlertsGrandTypeAuth alertsAuth;

        private readonly Credentials credentials;
        private readonly ILogger? logger;
        private CancellationToken cancellationToken;

        public ApiDesktopSampleFacade(Credentials credentials, ILogger? logger) : this(credentials) => this.logger = logger;
        public ApiDesktopSampleFacade(Credentials credentials) => this.credentials = credentials;


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                alertsAuth = new DonationAlertsGrandTypeAuth(credentials, logger);
                alertsAuth.OnAccessTokenGetted += (response) => OnAccessTokenGetted?.Invoke(response);
                alertsAuth.OnRefreshTokenGetted += (request, response) => OnRefreshTokenGetted?.Invoke(request, response);

                SimpleServer simpleServer = new SimpleServer(credentials.Redirect, credentials.Port, logger);
                simpleServer.Start();

                logger?.Log("Opening browser...");
                int idProcess = OpenProcess.Open(
                    alertsAuth.GetAuthorizationUrl(),
                    logger);

                await Task.Run(async () =>
                {
                    logger?.Log("Waiting for code...");
                    await Task.Delay(1500, cancellationToken);
                });

                CodeModel? codeModel = await simpleServer.AwaitCode(cancellationToken);
                logger?.Log(codeModel?.ToString() ?? "null");
                simpleServer.Dispose();
                OpenProcess.Close(idProcess, logger);

                AccessTokenCodeRequest accessTokenRequest = new AccessTokenCodeRequest()
                {
                    Client_id = credentials.ClientId,
                    Client_secret = credentials.ClientSecret,
                    Code = codeModel.Code,
                    Redirect_uri = StaticMethods.GetUrl(credentials.Redirect, credentials.Port)
                };
                logger?.Log(accessTokenRequest.ToString() ?? "null");
                AccessTokenResponse? accesTokenResponse = await alertsAuth.GetAccessTokenAsync(accessTokenRequest, cancellationToken);
                if (accesTokenResponse == null)
                {
                    logger?.Log("Failed to get access token.", LogLevel.Error);
                    return;
                }
                logger?.Log(accesTokenResponse?.ToString() ?? "null");

                ApiV1 apiV1 = new ApiV1(accesTokenResponse.Access_token, logger);
                UserWrap? userWrap = await apiV1.GetUserProfileAsync(cancellationToken);
                if (userWrap == null)
                {
                    logger?.Log("Failed to get user profile.", LogLevel.Error);
                    return;
                }
                DonationWrap? donation = await apiV1.GetDonationsAsync(cancellationToken);
                logger?.Log(userWrap?.ToString() ?? "User is null");
                logger?.Log(donation?.ToString() ?? "Donation is null");

                CentrifugoClientFacade = new CentrifugoClientFacade(credentials, userWrap.Data, accesTokenResponse, logger);

                alertsAuth.OnRefreshTokenGetted += (request, response) => CentrifugoClientFacade.UpdateAccessToken(response);
                CentrifugoClientFacade.OnDonationReceived += (donation) => OnDonationReceived?.Invoke(donation);
                CentrifugoClientFacade.OnGoalLaunchUpdate += (goalInfo) => OnGoalLaunchUpdate?.Invoke(goalInfo);
                CentrifugoClientFacade.OnGoalUpdated += (goalsUpdateWrapper) => OnGoalUpdated?.Invoke(goalsUpdateWrapper);
                CentrifugoClientFacade.OnPollUpdated += (pollDataWrapper) => OnPollUpdated?.Invoke(pollDataWrapper);
                CentrifugoClientFacade.OnMessageReceived += (message) => OnMessageReceived?.Invoke(message);

                try
                {
                    await CentrifugoClientFacade.StartAsync(cancellationToken); 
                }
                catch (OperationCanceledException)
                {
                    logger?.Log("Centrifugo was failed.", LogLevel.Error);
                }
            }
            catch (OperationCanceledException)
            {
                logger?.Log("Operation was cancelled.", LogLevel.Warning);
            }
        }
    }
}
