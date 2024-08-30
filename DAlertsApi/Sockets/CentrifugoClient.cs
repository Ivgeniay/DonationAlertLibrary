using DAlertsApi.Logger;
using DAlertsApi.Models;
using DAlertsApi.Models.Centrifugo;
using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Channels;

namespace DAlertsApi.Sockets
{
    public class CentrifugoClient
    {
        public int MaxRetryAttempts {get; set;} = 3;
        public int RetryDelayMilliseconds { get; set; } = 2000; 

        private readonly ClientWebSocket _webSocket;
        private readonly ILogger _logger; 

        public CentrifugoClient(ILogger logger)
        {
            this._logger = logger;
            _webSocket = new ClientWebSocket();
        }

        /// <summary>
        /// First step of connection to Centrifugo WebSocket server. Connects to the server and sends authentication message.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ConnectAsync(string token, int id)
        {
            int attempt = 0;
            while (attempt < MaxRetryAttempts)
            {
                attempt++;
                try
                {
                    // Открываем WebSocket соединение
                    await _webSocket.ConnectAsync(new Uri(Links.CentrifugoSocketEndpoint), CancellationToken.None);
                    _logger?.Log($"Connected to Centrifugo WebSocket server on attempt {attempt}.");

                    CentrifugoRequest centrifugoRequest = new()
                    {
                        Params = new()
                        {
                            Token = token
                        },
                        Id = id
                    };
                    string jsonMessage = JsonConvert.SerializeObject(centrifugoRequest);
                    _logger?.Log($"Sending authentication message: {jsonMessage}");
                    var bytes = Encoding.UTF8.GetBytes(jsonMessage);

                    await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    _logger?.Log("Sent authentication message.");
                    return true;
                }
                catch (WebSocketException ex)
                {
                    _logger?.Log($"WebSocketException on attempt {attempt}: {ex.Message}", LogLevel.Error);
                }
                catch (Exception ex)
                {
                    _logger?.Log($"Exception on attempt {attempt}: {ex.Message}", LogLevel.Error);
                }

                _logger?.Log($"Retrying connection in {RetryDelayMilliseconds}ms...", LogLevel.Warning);
                await Task.Delay(RetryDelayMilliseconds);
            }

            _logger?.Log("Failed to connect to Centrifugo after maximum retry attempts.", LogLevel.Error);
            return false; // Подключение не удалось
        }


        /// <summary>
        /// Second step of connection to Centrifugo WebSocket server. Receives Client ID from the server.
        /// </summary>
        /// <returns></returns>
        public async Task<CentrifugoResponse?> ReceiveClientIdAsync()
        {
            var buffer = new byte[2048]; // Увеличенный размер буфера
            try
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger.Log($"WebSocket closed with status {result.CloseStatus} and description {result.CloseStatusDescription}", LogLevel.Error);
                    return null;
                }

                var responseJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (string.IsNullOrWhiteSpace(responseJson))
                {
                    _logger.Log("Received empty response. Possible error in the communication.", LogLevel.Error);
                    return null;
                }

                CentrifugoResponse? response = JsonConvert.DeserializeObject<CentrifugoResponse>(responseJson);
                _logger.Log("Deserialized response: " + responseJson);
                return response;
            }
            catch (Exception ex)
            {
                _logger.Log("Error receiving WebSocket message: " + ex.Message, LogLevel.Error);
                return null;
            }
        }

        /// <summary>
        /// Subscribe to channels.
        /// </summary>
        /// <param name="uuidv4_client_id"></param>
        /// <param name="channels"></param>
        /// <returns></returns>
        public async Task<SubscriptionResponse?> SubscribeToChannelsAsync(string uuidv4_client_id, params string[] channels)
        {
            try
            { 
                using (var httpClient = new HttpClient())
                { 
                    SubscriptionRequest request = new()
                    {
                        Client = uuidv4_client_id,
                        Channels = channels.ToList()
                    };
                    // Формируем параметры запроса
                    var queryParams = new Dictionary<string, string>
                    {
                        { "client", request.Client },
                        { "channels", string.Join(",", request.Channels) }
                    };

                    var content = new FormUrlEncodedContent(queryParams);  
                    var response = await httpClient.PostAsync(Links.CentrifugoPrivateSubscribe, content);
                    response.EnsureSuccessStatusCode();

                    var responseJson = await response.Content.ReadAsStringAsync();
                    _logger?.Log("Received subscription response: " + responseJson);

                    // Десериализация ответа
                    SubscriptionResponse? subscriptionResponse = JsonConvert.DeserializeObject<SubscriptionResponse>(responseJson);
                    return subscriptionResponse;
                }
            }
            catch (Exception ex)
            {
                _logger?.Log("Error while subscribing to channels: " + ex.Message, LogLevel.Error);
                return null;
            }
        }


        /// <summary>
        /// Subscribe to private channels.
        /// </summary>
        /// <param name="channels"></param>
        /// <param name="tokens"></param>
        /// <returns></returns>
        public async Task SubscribeToPrivateChannelsAsync(string[] channels, string[] tokens)
        {
            for (int i = 0; i < channels.Length; i++)
            {
                var message = new
                {
                    @params = new
                    {
                        channel = channels[i],
                        token = tokens[i]
                    },
                    method = 1,
                    id = i + 2 // ID для каждого сообщения должно быть уникальным
                };

                string jsonMessage = JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(jsonMessage);

                await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                _logger?.Log($"Subscribed to channel: {channels[i]}");
            }
        }
    }
}
