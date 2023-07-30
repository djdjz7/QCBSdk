using QCBSdk.Types;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using static QCBSdk.Types.RequestTypes;
using static QCBSdk.Types.ResponseTypes;
using static QCBSdk.Url;
using static QCBSdk.Utils;

namespace QCBSdk
{
    /// <summary>
    /// 机器人客户端
    /// </summary>
    public partial class QCBClient
    {
        private string appId;
        private string token;
        private string secret;
        private string? sessionId;
        private string? gateway;
        private Timer? heartbeatTimer;
        private HttpClient httpClient = new HttpClient();
        internal ClientWebSocket clientWebSocket = new ClientWebSocket();
        private JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = new JsonSnakeCaseLowerNamingPolicy(),
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };

        private int? latestS = null;

        /// <summary>
        /// 实例化一个机器人客户端
        /// </summary>
        /// <remarks>
        /// 如果要启动 WebSocket 连接，使用 <see cref="InitializeAsync"/>
        /// </remarks>
        /// <seealse cref="InitializeAsync"/>
        /// <param name="botAppId">BotAppID（开发者 ID）</param>
        /// <param name="botToken">机器人令牌</param>
        /// <param name="botSecret">机器人密钥</param>
        /// <param name="isInSandbox">是否在沙箱环境</param>
        public QCBClient(string botAppId, string botToken, string botSecret, bool isInSandbox)
        {
            appId = botAppId;
            token = botToken;
            secret = botSecret;
            httpClient.BaseAddress = new Uri(isInSandbox ? SandboxEnvironmentBase : FormalEnvironmentBase);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {botAppId}.{botToken}");
        }

        /// <summary>
        /// 初始化 WebSocket 连接
        /// </summary>
        /// <exception cref="Exception">任何异常导致无法初始化 WebSocket 连接时，抛出异常</exception>
        public async Task InitializeAsync(int intents)
        {
            gateway = await GetGeneralWssGateway();
            if (gateway is null)
                throw new Exception("初始化失败，获取 WebSocket 接入点返回值为 null");

            // CONNECT
            await clientWebSocket.ConnectAsync(new Uri(gateway), CancellationToken.None);
            // HELLO & HEARTBEAT
            string? helloResponseString = await ClientWebSocketReceiveString() ?? throw new Exception("初始化失败，WebSocket 连接 Hello 消息为空");
            var helloResponseMessage = JsonSerializer.Deserialize<WebSocketMessage>(helloResponseString) ?? throw new Exception("初始化失败，WebSocket 连接 Hello 消息为空");
            int? interval = (((JsonElement)helloResponseMessage.d).Deserialize<ConnectResponseD>()?.heartbeat_interval) ?? throw new Exception("初始化失败，WebSocket 连接 Hello 消息异常");
            heartbeatTimer = new Timer(Heartbeat, null, 0, (int)interval);

            // IDENTIFY
            var identifyMessage = new
            {
                op = 2,
                d = new
                {
                    token = $"{appId}.{token}",
                    intents = intents,
                }
            };
            string identifyString = JsonSerializer.Serialize(identifyMessage);
            var identifyBytes = Encoding.UTF8.GetBytes(identifyString);
            clientWebSocket.SendAsync(new ArraySegment<byte>(identifyBytes), WebSocketMessageType.Text, true, CancellationToken.None).Wait();

            // READY
            var readyMessageString = await ClientWebSocketReceiveString() ?? throw new Exception("初始化失败，WebSocket 连接 READY 消息为空");
            var readyMessage = JsonSerializer.Deserialize<WebSocketMessage>(readyMessageString) ?? throw new Exception("初始化失败，WebSocket 连接 READY 消息为空");
            sessionId = ((JsonElement)readyMessage.d).Deserialize<ReadyD>()?.session_id;
            latestS = readyMessage.s;


            Thread thread = new(WebSocketMessageListener);
            thread.IsBackground = true;
            thread.Start();
        }
        private async Task<string?> ClientWebSocketReceiveString()
        {
            if (clientWebSocket.State == WebSocketState.Open || clientWebSocket.State == WebSocketState.CloseSent)
            {
                byte[] buffer = new byte[1024];
                byte[] fullResult;
                var receiveResult = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                fullResult = buffer;

                while (receiveResult.EndOfMessage != true)
                {
                    buffer = new byte[1024];
                    receiveResult = await clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    fullResult = ConcatByteArray(fullResult, buffer);
                }
                return Encoding.UTF8.GetString(fullResult).TrimEnd('\0');
            }
            else
                return null;
        }
        private void Heartbeat(object? state)
        {
            if (clientWebSocket.State == WebSocketState.Open || clientWebSocket.State == WebSocketState.CloseSent)
            {
                byte[] content;
                if (latestS is not null)
                    content = Encoding.UTF8.GetBytes($"{{\"op\": 1,\"d\": {latestS}}}");
                else
                    content = Encoding.UTF8.GetBytes("{\"op\": 1,\"d\": null}");
                clientWebSocket.SendAsync(new ArraySegment<byte>(content), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        private async void WebSocketMessageListener(object? obj)
        {

            while (true)
            {
                if (clientWebSocket.State == WebSocketState.Open || clientWebSocket.State == WebSocketState.CloseSent)
                {

                    string? content = await ClientWebSocketReceiveString();
                    if (string.IsNullOrEmpty(content))
                        continue;
                    Console.WriteLine(content);
                    var message = JsonSerializer.Deserialize<WebSocketMessage>(content);
                    switch (message?.op)
                    {
                        case Opcode.Dispatch:
                            // Console.WriteLine($"{DateTime.Now}:\n{message?.d}");
                            ActionsInvoker(message.t, (JsonElement)message.d);
                            break;
                        case Opcode.Identity:
                            break;
                        case Opcode.Resume:
                            Console.WriteLine($"{DateTime.Now}: Resumed.");
                            break;
                        case Opcode.Reconnect:
                            Console.WriteLine($"{DateTime.Now}: Reconnect required.");
                            await Reconnect();
                            break;
                        case Opcode.InvalidSession:
                            break;
                        case Opcode.Hello:
                            break;
                        case Opcode.HeartbeatACK:
                            Console.WriteLine($"{DateTime.Now}: HeartbeatACK.");
                            break;
                        case Opcode.HttpCallbackACK:
                            break;
                        default:
                            break;
                    }

                    if (message is not null && message.s is not null)
                        latestS = message.s;
                }

                if (clientWebSocket.State == WebSocketState.Closed)
                {
                    Console.WriteLine($"{DateTime.Now}: WEBSOCKET CLOSED: {clientWebSocket.CloseStatus}, {clientWebSocket.CloseStatusDescription}");
                }
            }
        }
        private async Task Reconnect()
        {
            if (clientWebSocket.State != WebSocketState.Closed)
            {
                await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            clientWebSocket.Dispose();
            clientWebSocket = new ClientWebSocket();
            if (gateway is null)
                throw new Exception("WebSocket 重新连接异常：接入点为 null");
            clientWebSocket.ConnectAsync(new Uri(gateway), CancellationToken.None).Wait();

            string helloResponseString = await ClientWebSocketReceiveString() ?? throw new Exception("WebSocket 重新连接异常，HELLO 消息为 null");
            Console.WriteLine(helloResponseString);
            var helloResponseMessage = JsonSerializer.Deserialize<WebSocketMessage>(helloResponseString) ?? throw new Exception("WebSocket 重新连接异常，HELLO 消息异常");
            int interval = ((JsonElement)helloResponseMessage.d).Deserialize<ConnectResponseD>()?.heartbeat_interval ?? throw new Exception("WebSocket 重新连接异常，HELLO 消息异常");

            heartbeatTimer?.Dispose();
            heartbeatTimer = new Timer(Heartbeat, null, 0, interval);

            var resumeMessage = new
            {
                op = 6,
                d = new
                {
                    token = $"{appId}.{token}",
                    session_id = sessionId,
                    seq = latestS,
                }
            };

            string resumeMessageString = JsonSerializer.Serialize(resumeMessage);
            byte[] resumeMessageBytes = Encoding.UTF8.GetBytes(resumeMessageString);
            clientWebSocket.SendAsync(new ArraySegment<byte>(resumeMessageBytes), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
        }


        /// <summary>
        /// 获取机器人详情
        /// </summary>
        /// <returns></returns>
        public async Task<User?> GetBotDetailsAsync()
        {
            return await httpClient.GetFromJsonAsync<User>("/users/@me", jsonSerializerOptions);
        }

        /// <summary>
        /// 获取机器人所在频道
        /// </summary>
        /// <returns>一个 <see cref="Guild"/> 列表，表明机器人所在频道</returns>
        public async Task<List<Guild>?> GetBotGuildListAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Guild>>("/users/@me/guilds", jsonSerializerOptions);
        }

        /// <summary>
        /// 获取指定频道详情
        /// </summary>
        /// <param name="guildId">指定频道 ID</param>
        /// <returns>一个 <see cref="Guild"/> 对象，表明请求频道的详情</returns>
        public async Task<Guild?> GetGuildDetailsAsync(string guildId)
        {
            return await httpClient.GetFromJsonAsync<Guild>($"/guilds/{guildId}", jsonSerializerOptions);
        }

        /// <summary>
        /// 获取指定频道的子频道
        /// </summary>
        /// <param name="guildId">指定频道 ID</param>
        /// <returns>一个 <see cref="Channel"/> 列表，表明指定频道包含的子频道</returns>
        public async Task<List<Channel>?> GetChannelListAsync(string guildId)
        {
            return await httpClient.GetFromJsonAsync<List<Channel>>($"/guilds/{guildId}/channels", jsonSerializerOptions);
        }

        /// <summary>
        /// 获取指定子频道详情
        /// </summary>
        /// <param name="channelId">指定子频道 ID</param>
        /// <returns>一个 <see cref="Channel"/> 对象，表明请求子频道的详情</returns>
        public async Task<Channel?> GetChannelDetailsAsync(string channelId)
        {
            return await httpClient.GetFromJsonAsync<Channel>($"/channels/{channelId}", jsonSerializerOptions);
        }

        /// <summary>
        /// 创建一个子频道
        /// </summary>
        /// <param name="guildId">子频道的父频道 ID</param>
        /// <param name="createChannelRequest">携带的请求</param>
        /// <returns>一个 <see cref="Channel"/> 对象，表明新创建的子频道的详情</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Channel?> CreateChannelAsync(string guildId, CreateChannelRequest createChannelRequest)
        {
            var response = await httpClient.PostAsJsonAsync($"/guilds/{guildId}/channels", createChannelRequest, jsonSerializerOptions);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"创建子频道出错，其 HTTP 状态码为：{response.StatusCode}");
            return await response.Content.ReadFromJsonAsync<Channel>(jsonSerializerOptions);
        }

        /// <summary>
        /// 修改指定的子频道
        /// </summary>
        /// <param name="channelId">指定的子频道 ID</param>
        /// <param name="editChannelRequest">携带的修改请求</param>
        /// <returns>一个 <see cref="Channel"/> 对象，表明修改后的子频道的详情</returns>
        /// <exception cref="Exception"></exception>
        public async Task<Channel?> EditChannelAsync(string channelId, EditChannelRequest editChannelRequest)
        {
            var response = await httpClient.PatchAsJsonAsync($"/channels/{channelId}", editChannelRequest, jsonSerializerOptions);
            if (!response.IsSuccessStatusCode)
                throw new Exception($"修改子频道出错，其 HTTP 状态码为：{response.StatusCode}");
            return await response.Content.ReadFromJsonAsync<Channel>(jsonSerializerOptions);
        }

        /// <summary>
        /// 删除指定的子频道
        /// </summary>
        /// <param name="channelId">指定的子频道 ID</param>
        /// <exception cref="Exception"></exception>
        public async Task DeleteChannelAsync(string channelId)
        {
            var response = await httpClient.DeleteAsync($"/channels/{channelId}");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"删除子频道出错，其 HTTP 状态码为：{response.StatusCode}");
        }

        /// <summary>
        /// 获取指定子频道在线人数
        /// </summary>
        /// <param name="channelId">指定的子频道 ID</param>
        /// <returns></returns>
        public async Task<int?> GetChannelOnlineCount(string channelId)
        {
            var response = await httpClient.GetFromJsonAsync<GetChannelOnlineCountResponse>($"/channels/{channelId}/online_nums", jsonSerializerOptions);
            return response?.OnlineNums;
        }

        /// <summary>
        /// 获取通用 WebSocket 接入点
        /// </summary>
        /// <returns>成功则返回接入点 url，异常则返回 <see langword="null"/></returns>
        public async Task<string?> GetGeneralWssGateway()
        {
            var response = await httpClient.GetFromJsonAsync<GetWssGatewayResponse>("/gateway", jsonSerializerOptions);
            return response?.Url;
        }

        
        public async Task<Message?> SendMessageAsync(string channelId, SendMessageRequest sendMessageRequest)
        {
            var response = await httpClient.PostAsJsonAsync($"/channels/{channelId}/messages", sendMessageRequest, jsonSerializerOptions);
            if (!response.IsSuccessStatusCode)
                Console.WriteLine(await response.Content.ReadAsStringAsync());
            return await response.Content.ReadFromJsonAsync<Message>(jsonSerializerOptions);
        }
        public async Task<Message?> SendMessageAsync(string channelId, SendMessageRequest sendMessageRequest, Stream imageStream, string imageFileName)
        {
            var content = new MultipartFormDataContent();
            if (sendMessageRequest.Content is not null)
                content.Add(new StringContent(sendMessageRequest.Content), "content");
            if (sendMessageRequest.Embed is not null)
                content.Add(new StringContent(
                    JsonSerializer.Serialize(sendMessageRequest.Embed, jsonSerializerOptions)),
                    "embed");
            if (sendMessageRequest.Ark is not null)
                content.Add(new StringContent(
                    JsonSerializer.Serialize(sendMessageRequest.Ark, jsonSerializerOptions)),
                    "ark");
            if (sendMessageRequest.MessageReference is not null)
                content.Add(new StringContent(
                    JsonSerializer.Serialize(sendMessageRequest.MessageReference, jsonSerializerOptions)),
                    "message_reference");
            if (sendMessageRequest.Image is not null)
                content.Add(new StringContent(sendMessageRequest.Image), "image");
            if (sendMessageRequest.MsgId is not null)
                content.Add(new StringContent(sendMessageRequest.MsgId), "msg_id");
            if (sendMessageRequest.Markdown is not null)
                content.Add(new StringContent(
                    JsonSerializer.Serialize(sendMessageRequest.Markdown, jsonSerializerOptions)),
                    "markdown");

            content.Add(new StreamContent(imageStream), "file_image", imageFileName);

            var response = await httpClient.PostAsync($"/channels/{channelId}/messages", content);
            var message = await response.Content.ReadFromJsonAsync<Message>(jsonSerializerOptions);
            Console.WriteLine(response.StatusCode.ToString());
            Console.WriteLine((int)response.StatusCode); // 202 here
            Console.WriteLine(await response.Content.ReadAsStringAsync());
            return message;
        }

        internal void ActionsInvoker(string type, JsonElement data)
        {
            switch (type)
            {
                case "MESSAGE_CREATE":
                    if (OnMessageCreate is null)
                        return;
                    var message1 = data.Deserialize<Message>(jsonSerializerOptions);
                    if (message1 is null)
                        return;
                    OnMessageCreate.Invoke(message1);
                    break;
                case "AT_MESSAGE_CREATE":
                    if (OnAtMessageCreate is null)
                        return;
                    var message2 = data.Deserialize<Message>(jsonSerializerOptions);
                    if (message2 is null)
                        return;
                    OnAtMessageCreate.Invoke(message2);
                    break;

                default:
                    OnUnknownWebSocketMessageReceived?.Invoke(type, data);
                    break;
            }
        }
    }
}