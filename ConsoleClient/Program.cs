using DAlertsApi.Models.Settings;
using DAlertsApi.Logger;
using DAlertsApi.Facade;

namespace ConsoleClient
{
    internal partial class Program
    {
        static void Main(string[] args)
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
            ApiDesktopSampleFacade apiFacade = new(credentials, logger);

            apiFacade.OnDonationReceived += (donation) => Console.WriteLine(donation.ToString());
            apiFacade.OnGoalLaunchUpdate += (donation) => Console.WriteLine(donation.ToString());
            apiFacade.OnGoalUpdated += (donation) => Console.WriteLine(donation.ToString()); 
            apiFacade.OnPollUpdated += (donation) => Console.WriteLine(donation.ToString());

            apiFacade.StartAsync();
            Console.ReadKey();
        }
    }
}