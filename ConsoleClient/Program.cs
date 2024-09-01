using DAlertsApi.Models.Auth.AuthCode;
using DAlertsApi.Models.Centrifugo;
using DAlertsApi.Models.Settings;
using DAlertsApi.SystemFunc;
using DAlertsApi.ApiV1lib;
using DAlertsApi.Sockets;
using DAlertsApi.Logger;
using DAlertsApi.Auth;
using DAlertsApi.Models.ApiV1.Users;
using DAlertsApi;

namespace ConsoleClient
{
    internal partial class Program
    {
        static async Task Main(string[] args)
        {
            ILogger logger = new Logger().LogAction((message, logLevel) =>
            {
                if (logLevel == LogLevel.Error)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"{logLevel} ({DateTime.Now}):");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{message}\n");
                }
                else if (logLevel == LogLevel.Warning)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"{logLevel} ({DateTime.Now}):");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{message}\n"); 
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{logLevel} ({DateTime.Now}):");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write($"{message}\n"); 
                }

            });
            Credentials credentials = new(
                clientId: 13330, 
                clientSecret: "i416YiL9yVOuCYcxI8CpM6swcZXFK5Tsj5z3i5sX", 
                redirect: "http://localhost",
                port: "3180",
                ScopeType.OauthDonationSubscribe,
                ScopeType.OauthGoalSubscribe,
                ScopeType.OauthPollSubscribe,
                ScopeType.OauthCustomAlertStore,
                ScopeType.OauthDonationIndex,
                ScopeType.OauthUserShow
                );
            ApiDesctopSampleFacade apiFacade = new(credentials, logger);
            apiFacade.Start();
            Console.ReadKey();
        }
    }
}