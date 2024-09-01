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
using Newtonsoft.Json;
using DAlertsApi.Models.ApiV1.Merchandise;

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
            ApiDesctopSampleFacade apiFacade = new(credentials);
            apiFacade.centrifugoClient.OnDonationReceived += (donation) =>
            {
                Console.WriteLine(donation.ToString());
            };
            apiFacade.centrifugoClient.OnGoalLaunchUpdate += (donation) =>
            {
                Console.WriteLine(donation.ToString());
            };
            apiFacade.centrifugoClient.OnGoalUpdated += (donation) =>
            {
                Console.WriteLine(donation.ToString());
            };
            apiFacade.centrifugoClient.OnPollUpdated += (donation) =>
            {
                Console.WriteLine(donation.ToString());
            };

            var t = @"{
                        ""id"": 3,
                        ""merchant"": {
                            ""identifier"": ""MARKET_GAMES_MAIL_RU"",
                            ""name"": ""Market games@mail.ru""
                        },
                        ""identifier"": ""8082"",
                        ""title"": {
                            ""en_US"": ""Credit case"",
                            ""ru_RU"": ""Кредитный кейс""
                        },
                        ""is_active"": 1,
                        ""is_percentage"": 1,
                        ""currency"": ""USD"",
                        ""price_user"": 30,
                        ""price_service"": 15,
                        ""url"": ""https://market.games.mail.ru/game/1?product_id=8082&user_id={user_id}"",
                        ""img_url"": ""https://market.games.mail.ru/s3/media/product/picture/2020/7/a5077d65bed0439dd78a01d12cee948d.png"",
                        ""end_at"": null
                        
                    }";
            var merch = JsonConvert.DeserializeObject<CreateMerchandiseResponse>(t);
            Console.WriteLine(merch.ToString());

            Console.ReadKey();

            apiFacade.Start();

            Console.ReadKey();
        }
    }
}