using DAlertsApi;
using DAlertsApi.Auth;
using DAlertsApi.Logger;
using DAlertsApi.Models.ApiV1;
using DAlertsApi.Models.Centrifugo;
using DAlertsApi.Models.Settings;
using DAlertsApi.Sockets;
using DAlertsApi.SystemFunc;

namespace ConsoleClient
{
    internal partial class Program
    {
        static async Task Main(string[] args)
        {
            Logger logger = new();
            logger.LogAction((message, logLevel) =>
            {
                if (logLevel == LogLevel.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{logLevel}:");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{message}\n");
                }
                else if (logLevel == LogLevel.Warning)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{logLevel}:");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{message}\n"); 
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{logLevel}:");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{message}\n"); 
                }

            });

            Credentials credentials = new(
                clientId: 13330, 
                clientSecret: "i416YiL9yVOuCYcxI8CpM6swcZXFK5Tsj5z3i5sX", 
                redirect: "http://localhost",
                port: "3180",
                ScopeType.OauthDonationIndex,
                ScopeType.OauthDonationSubscribe,
                ScopeType.OauthPollSubscribe,
                ScopeType.OauthGoalSubscribe,
                ScopeType.OauthUserShow,
                ScopeType.OauthCustomAlertStore);

            DonationAlertsGrandTypeAuth alertsAuth = new(credentials, logger);
            SimpleServer simpleServer = new(credentials.Redirect, credentials.Port, logger);
            simpleServer.Start(); 

            logger.Log("Opening browser...");
            var idProcess = OpenProcess.Open(alertsAuth.GetAuthorizationUrl(credentials.Scope),
                logger); 
            
            await Task.Run(() =>
            { logger.Log("Waiting for code..."); Thread.Sleep(1500); });

            Task<CodeModel> serverCodeAwaiter = simpleServer.AwaitCode();
            CodeModel codeModel = await serverCodeAwaiter;
            logger.Log(codeModel.ToString() ?? "null");
            simpleServer.Dispose();
            OpenProcess.Close(idProcess, logger);

            AccessTokenRequest accessTokenRequest = new()
            {
                Client_id = credentials.ClientId,
                Client_secret = credentials.ClientSecret,
                Code = codeModel.Code,
                Redirect_uri = StaticMethods.GetUrl(credentials.Redirect, credentials.Port) 
            };
            logger.Log(accessTokenRequest.ToString() ?? "null");
            AccessTokenResponse? accesTokenResponse = await alertsAuth.GetAccessToken(accessTokenRequest); 
            logger.Log(accesTokenResponse?.ToString() ?? "null");

            ApiV1 apiV1 = new(logger, accesTokenResponse.Access_token);
            UserWrap? userWrap = await apiV1.GetUserProfileAsync();
             

            CentrifugoClient centrifugoClient = new(logger);
            bool isConnected = await centrifugoClient.ConnectAsync(userWrap.Data.Socket_connection_token, userWrap.Data.Id);
            if (isConnected)
            {
                // Проверка ответа
                CentrifugoResponse? response = await centrifugoClient.ReceiveClientIdAsync();
                if (response != null && !string.IsNullOrEmpty(response.Result?.Client))
                {
                    logger?.Log($"Successfully authenticated with Client ID: {response.Result.Client}");
                }
            }
            else
            {
                logger.Log("Failed to establish a connection with Centrifugo.", LogLevel.Error);
            } 

            Console.ReadKey();
        }
    }
}
