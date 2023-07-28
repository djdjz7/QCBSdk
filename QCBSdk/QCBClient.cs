using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using static QCBSdk.BasicObjects;
using static QCBSdk.Enums;
using static QCBSdk.RequestTypes;
using static QCBSdk.ResponseTypes;
using static QCBSdk.Url;
using static QCBSdk.Utils;
namespace QCBSdk
{
    public class QCBClient
    {
        private string appId;
        private string token;
        private string secret;
        private string? sessionId;
        private string? gateway;
        private Timer? heartbeatTimer;
        private HttpClient httpClient = new HttpClient();
        public ClientWebSocket clientWebSocket = new ClientWebSocket();
        private JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };
        public delegate void MessageEventHandler(object sender, string data);
        public event MessageEventHandler? OnMessageReceived;
        public int? latestS = null;
        public QCBClient(string botAppId, string botToken, string botSecret, bool isInSandbox)
        {
            appId = botAppId;
            token = botToken;
            secret = botSecret;
            httpClient.BaseAddress = new Uri(isInSandbox ? SandboxEnvironmentBase : FormalEnvironmentBase);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {botAppId}.{botToken}");
        }

        public async Task InitializeAsync()
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
                    intents = 0 | 1 << 30,
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

                    var message = JsonSerializer.Deserialize<WebSocketMessage>(content);
                    switch (message?.op)
                    {
                        case Opcode.Dispatch:
                            Console.WriteLine($"{DateTime.Now}:\n{message?.d}");
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
        public async Task<User?> GetBotDetailsAsync()
        {
            return await httpClient.GetFromJsonAsync<User>("/users/@me", jsonSerializerOptions);
        }
        public async Task<List<Guild>?> GetBotGuildListAsync()
        {
            return await httpClient.GetFromJsonAsync<List<Guild>>("/users/@me/guilds", jsonSerializerOptions);
        }
        public async Task<Guild?> GetGuildDetailsAsync(string guildId)
        {
            return await httpClient.GetFromJsonAsync<Guild>($"/guilds/{guildId}", jsonSerializerOptions);
        }
        public async Task<List<Channel>?> GetChannelListAsync(string guildId)
        {
            return await httpClient.GetFromJsonAsync<List<Channel>>($"/guilds/{guildId}/channels", jsonSerializerOptions);
        }
        public async Task<Channel?> GetChannelDetailsAsync(string channelId)
        {
            return await httpClient.GetFromJsonAsync<Channel>($"/channels/{channelId}", jsonSerializerOptions);
        }
        public async Task<Channel?> CreateChannelAsync(string guildId, CreateChannelRequest createChannelRequest)
        {
            var response = await httpClient.PostAsJsonAsync($"/guilds/{guildId}/channels", createChannelRequest, jsonSerializerOptions);
            return await response.Content.ReadFromJsonAsync<Channel>(jsonSerializerOptions);
        }
        public async Task<Channel?> EditChannelAsync(string channelId, EditChannelRequest editChannelRequest)
        {
            var response = await httpClient.PatchAsJsonAsync($"/channels/{channelId}", editChannelRequest, jsonSerializerOptions);
            return await response.Content.ReadFromJsonAsync<Channel>(jsonSerializerOptions);
        }
        public async Task DeleteChannelAsync(string channelId)
        {
            var response = await httpClient.DeleteAsync($"/channels/{channelId}");
            if (!response.IsSuccessStatusCode)
                throw new Exception($"删除子频道出错，其 HTTP 状态码为：{response.StatusCode}");
        }
        public async Task<int?> GetChannelOnlineCount(string channelId)
        {
            var response = await httpClient.GetFromJsonAsync<GetChannelOnlineCountResponse>($"/channels/{channelId}/online_nums", jsonSerializerOptions);
            return response?.OnlineNums;
        }
        public async Task<string?> GetGeneralWssGateway()
        {
            var response = await httpClient.GetFromJsonAsync<GetWssGatewayResponse>("/gateway", jsonSerializerOptions);
            return response?.Url;
        }

        public async Task<string?> ClientWebSocketReceiveString()
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

    }
}