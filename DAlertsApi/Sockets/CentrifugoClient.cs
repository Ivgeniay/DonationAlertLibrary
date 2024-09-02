using DAlertsApi.Models.ApiV1.Donations;
using DAlertsApi.Models.Centrifugo;
using DAlertsApi.Models.Errors;
using System.Net.WebSockets;
using DAlertsApi.Logger;
using DAlertsApi.Models;
using Newtonsoft.Json; 
using System.Text;

namespace DAlertsApi.Sockets
{
    public class CentrifugoClient
    {
        public event Action<WebSocketMessage>? OnMessageReceived; 
        public event Action<GoalsUpdateWrapper>? OnGoalUpdated;
        public event Action<GoalInfo>? OnGoalLaunchUpdate; 
        public event Action<PollDataWrapper>? OnPollUpdated;
        public event Action<Donation>? OnDonationReceived;
        public int MaxRetryAttempts { get; set; } = 3;
        public int RetryDelayMilliseconds { get; set; } = 2000;

        private readonly ClientWebSocket _webSocket;
        private readonly ILogger? logger;

        public CentrifugoClient()
        {
            _webSocket = new ClientWebSocket(); 
        }
        public CentrifugoClient(ILogger? logger) : this()
        {
            this.logger = logger; 
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
                    logger?.Log($"Connected to Centrifugo WebSocket server on attempt {attempt}.");

                    CentrifugoRequest centrifugoRequest = new()
                    {
                        Params = new()
                        {
                            Token = token
                        },
                        Id = id
                    };
                    string jsonMessage = JsonConvert.SerializeObject(centrifugoRequest);
                    logger?.Log($"Sending authentication message: {jsonMessage}");
                    var bytes = Encoding.UTF8.GetBytes(jsonMessage);

                    await _webSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                    logger?.Log("Sent authentication message.");
                    return true;
                }
                catch (WebSocketException ex)
                {
                    logger?.Log($"WebSocketException on attempt {attempt}: {ex.Message}", LogLevel.Error);
                }
                catch (Exception ex)
                {
                    logger?.Log($"Exception on attempt {attempt}: {ex.Message}", LogLevel.Error);
                }

                logger?.Log($"Retrying connection in {RetryDelayMilliseconds}ms...", LogLevel.Warning);
                await Task.Delay(RetryDelayMilliseconds);
            }

            logger?.Log("Failed to connect to Centrifugo after maximum retry attempts.", LogLevel.Error);
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
                    logger?.Log($"WebSocket closed with status {result.CloseStatus} and description {result.CloseStatusDescription}", LogLevel.Error);
                    return null;
                }

                var responseJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                if (string.IsNullOrWhiteSpace(responseJson))
                {
                    logger?.Log("Received empty response. Possible error in the communication.", LogLevel.Error);
                    return null;
                }

                CentrifugoResponse? response = JsonConvert.DeserializeObject<CentrifugoResponse>(responseJson);
                logger?.Log("Deserialized response: " + responseJson);
                return response;
            }
            catch (Exception ex)
            {
                logger?.Log("Error receiving WebSocket message: " + ex.Message, LogLevel.Error);
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
                    logger?.Log("Sending subscription request: " + jsonContent);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync(Links.CentrifugoPrivateSubscribe, content);
                    response.EnsureSuccessStatusCode();

                    string responseJson = await response.Content.ReadAsStringAsync();
                    logger?.Log("Received subscription response: " + responseJson);

                    SubscriptionResponse? subscriptionResponse = JsonConvert.DeserializeObject<SubscriptionResponse>(responseJson);
                    return subscriptionResponse;
                }
            }
            catch (Exception ex)
            {
                logger?.Log("Error while subscribing to channels: " + ex.Message, LogLevel.Error);
                return null;
            }
        }
        /// <summary>
        /// Connect to channels.
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="channelToken"></param>
        /// <param name="methodId"></param>
        /// <param name="messageId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
                logger?.Log("Sending message to connect to channel: " + requestJson);

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
                if (response == null)
                {
                    var error = JsonConvert.DeserializeObject<ConnectChannelResponseError>(firstResponseJson);
                    logger?.Log("(first response) Failed to connect to channel: " + channel + ". Error: " + error, LogLevel.Error);
                    return false;
                }
                else
                {
                    logger?.Log("Received first response from channel connection: " + response); 
                }

                var secondResponse = await _webSocket.ReceiveAsync(new ArraySegment<byte>(responseBuffer), CancellationToken.None);
                var secondResponseJson = Encoding.UTF8.GetString(responseBuffer, 0, secondResponse.Count);
                ConnectChannelResponse? response2 = JsonConvert.DeserializeObject<ConnectChannelResponse>(secondResponseJson); 
                if (response2 == null)
                {
                    var error = JsonConvert.DeserializeObject<ConnectChannelResponseError>(secondResponseJson);
                    logger?.Log("(Second response) Failed to connect to channel: " + channel + ". Error: " + error, LogLevel.Error);
                    return false;
                }
                else
                {
                    logger?.Log("Received second response from channel connection: " + response2); 
                }


                // Проверяем успешное подключение
                if (response2.result.type == 1)
                {
                    logger?.Log("Successfully connected to channel: " + channel);
                    return true;
                }
                else
                {
                    logger?.Log("Failed to connect to channel: " + channel, LogLevel.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                logger?.Log("Error while connecting to channel: " + ex.Message, LogLevel.Error);
                return false;
            }
        }
        /// <summary>
        /// Listeting for messages from Centrifugo WebSocket server.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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
                        logger?.Log("WebSocket connection closed by the server.");
                        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, cancellationToken);
                        break;
                    }
                    else if (result.MessageType != WebSocketMessageType.Text)
                    {
                        logger?.Log("Received non-text message, ignoring.");
                        continue;
                    }
                    else
                    {
                        var responseJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        logger?.Log("Received WebSocket message: " + responseJson);
                        var wsMessage = JsonConvert.DeserializeObject<WebSocketMessage>(responseJson);
                        if (wsMessage == null)
                        {
                            logger?.Log("Received null message, ignoring.");
                            continue;
                        }
                        else
                        {
                            HandleMessage(wsMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger?.Log("Error while receiving WebSocket message: " + ex.Message, LogLevel.Error);
                }
            }
        }
        private void HandleMessage(WebSocketMessage wsMessage)
        {
            OnMessageReceived?.Invoke(wsMessage);

            try
            {  
                if (wsMessage.Result.Channel.Contains("goals:"))
                {  
                    dynamic data = wsMessage.Result.Data;
                    string dataJson = JsonConvert.SerializeObject(data);
                    if (dataJson.Contains("\"info\":{\"user\":\""))
                    {
                        dataJson = JsonConvert.SerializeObject(data.info);
                        GoalInfo? info = JsonConvert.DeserializeObject<GoalInfo>(dataJson);
                        if (info == null)
                        {
                            logger?.Log("Failed to deserialize goal info message.", LogLevel.Error); 
                            return;
                        }
                        if (wsMessage.Result.Type == 1) info.IsLauched = true;
                        else info.IsLauched = false;
                        OnGoalLaunchUpdate?.Invoke(info);
                        logger?.Log($"Received goal info message: {info}");
                    }
                    else if (dataJson.Contains("\"seq\":"))
                    {
                        dataJson = JsonConvert.SerializeObject(data);
                        GoalsUpdateWrapper? desData = JsonConvert.DeserializeObject<GoalsUpdateWrapper>(dataJson);
                        if (desData == null)
                        {
                            logger?.Log("Failed to deserialize goal update message.", LogLevel.Error);
                            return;
                        }
                        OnGoalUpdated?.Invoke(desData);
                        logger?.Log($"Received goal message: {desData}");
                    } 
                }
                else if (wsMessage.Result.Channel.Contains("polls:"))
                {
                    dynamic data = wsMessage.Result.Data;
                    string dataJson = JsonConvert.SerializeObject(data);
                    var desData = JsonConvert.DeserializeObject<PollDataWrapper>(dataJson);
                    if (desData == null)
                    {
                        logger?.Log("Failed to deserialize pool update message.", LogLevel.Error);
                        return;
                    }
                    OnPollUpdated?.Invoke(desData); 
                    logger?.Log($"Received pool update message: {desData}");
                }
                else if (wsMessage.Result.Channel.Contains("alerts:"))
                { 
                    Donation? donation = JsonConvert.DeserializeObject<Donation>(wsMessage.ToString());
                    if(donation == null)
                    {
                        logger?.Log("Failed to deserialize donation message.", LogLevel.Error);
                        return;
                    }
                    OnDonationReceived?.Invoke(donation); 
                    logger?.Log($"Received donation message: {donation}");
                } 
            }
            catch (Exception ex)
            {
                logger?.Log("Error while handling WebSocket message: " + ex.Message, LogLevel.Error);
            }
        } 
    }
    
}
