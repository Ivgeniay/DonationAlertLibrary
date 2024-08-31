using DAlertsApi.Logger;
using DAlertsApi.Models;
using DAlertsApi.Models.Centrifugo;
using DAlertsApi.Models.Errors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
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
        public async Task<SubscriptionResponse?> SubscribeToChannelsAsync(SubscriptionRequest request, string bearerToken)
        {
            try
            { 
                using (var httpClient = new HttpClient())
                {
                    httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", bearerToken);
                    string jsonContent = JsonConvert.SerializeObject(request);
                    jsonContent = jsonContent.ToLower();
                    _logger.Log("Sending subscription request: " + jsonContent);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");   
                    var response = await httpClient.PostAsync(Links.CentrifugoPrivateSubscribe, content);
                    response.EnsureSuccessStatusCode();

                    string responseJson = await response.Content.ReadAsStringAsync();
                    _logger?.Log("Received subscription response: " + responseJson);
                     
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
        public async Task<bool> ConnectToChannelAsync(string channel, string channelToken, int methodId, int messageId, CancellationToken cancellationToken)
        {
            try
            {
                var request = new
                {
                    @params = new
                    {
                        channel = channel,
                        token = channelToken
                    },
                    method = methodId,
                    id = messageId
                };

                // Сериализуем сообщение в JSON
                var requestJson = JsonConvert.SerializeObject(request);
                _logger?.Log("Sending message to connect to channel: " + requestJson);

                // Преобразуем JSON в массив байтов
                var bytes = Encoding.UTF8.GetBytes(requestJson);
                var buffer = new ArraySegment<byte>(bytes);

                // Отправляем сообщение через WebSocket
                await _webSocket.SendAsync(buffer, WebSocketMessageType.Text, true, cancellationToken);

                // Получаем ответ от сервера
                var responseBuffer = new byte[1024 * 4];
                var firstResponse = await _webSocket.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None); 
                var firstResponseJson = Encoding.UTF8.GetString(responseBuffer, 0, firstResponse.Count);
                CoonectToChannelFirstMessage? response = JsonConvert.DeserializeObject<CoonectToChannelFirstMessage>(firstResponseJson); 
                _logger?.Log("Received first response from channel connection: " + firstResponseJson);

                var secondResponse = await _webSocket.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None); 
                var secondResponseJson = Encoding.UTF8.GetString(responseBuffer, 0, secondResponse.Count);
                ConnectChannelResponse? response2 = JsonConvert.DeserializeObject<ConnectChannelResponse>(secondResponseJson);

                if (response == null)
                {
                    var error = JsonConvert.DeserializeObject<ConnectChannelResponseError>(firstResponseJson);
                    _logger?.Log("Failed to connect to channel: " + channel + ". Error: " + error, LogLevel.Error);
                } 
                _logger?.Log("Received second response from channel connection: " + secondResponseJson);

                // Проверяем успешное подключение
                if (response2.result.type == 1)
                {
                    _logger?.Log("Successfully connected to channel: " + channel);
                    return true;
                }
                else
                {
                    _logger?.Log("Failed to connect to channel: " + channel, LogLevel.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger?.Log("Error while connecting to channel: " + ex.Message, LogLevel.Error);
                return false;
            }
        }
        
        
        public async Task ListenForMessagesAsync(CancellationToken cancellationToken)
        {
            var buffer = new byte[1024 * 4];

            while (_webSocket.State == WebSocketState.Open)
            {
                try
                {
                    var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        _logger?.Log("WebSocket connection closed by the server.");
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
                        break;
                    }
                    else if (result.MessageType != WebSocketMessageType.Text)
                    {
                        _logger?.Log("Received non-text message, ignoring.");
                        continue;
                    }
                    else
                    {
                        var responseJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        _logger?.Log("Received message: " + responseJson); 
                        HandleMessage(responseJson); 
                    }
                }
                catch (Exception ex)
                {
                    _logger?.Log("Error while receiving WebSocket message: " + ex.Message, LogLevel.Error);
                }
            }
        }
        private void HandleMessage(string responseJson)
        {
            try
            { 
                var message = JsonConvert.DeserializeObject<WebSocketMessage>(responseJson);

                if (message == null)
                {
                    _logger?.Log("Received null message, ignoring.");
                    return;
                }

                // Логика обработки сообщения
                switch (message.Result.Type)
                {
                    case 1:
                        _logger?.Log($"Connected to channel: {message.Result.Channel}");
                        break;
                    default:
                        _logger?.Log($"Received message of type {message.Result.Type} on channel {message.Result.Channel}");
                        break;
                } 
            }
            catch (Exception ex)
            {
                _logger?.Log("Error while handling WebSocket message: " + ex.Message, LogLevel.Error);
            }
        }

        public class WebSocketMessage
        {
            public int Id { get; set; }
            public WebSocketResult Result { get; set; } = new WebSocketResult();

            override public string ToString()
            {
                return JsonConvert.SerializeObject(this);
            }
        }
        public class WebSocketResult
        {
            public int Type { get; set; }
            public string Channel { get; set; } = string.Empty;
            public dynamic Data { get; set; } = new ExpandoObject(); // В зависимости от структуры данных, можно заменить на конкретный тип
        }
    }
}
