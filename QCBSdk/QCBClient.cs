using System.Linq;
using System.Net.Http.Json;
using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using static QCBSdk.BasicObjects;
using static QCBSdk.RequestTypes;
using static QCBSdk.ResponseTypes;
using static QCBSdk.Url;
namespace QCBSdk
{
    public class QCBClient
    {
        private string appId;
        private string token;
        private string secret;
        private HttpClient httpClient = new HttpClient();
        ClientWebSocket clientWebSocket = new ClientWebSocket();
        private JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };
        public delegate void MessageEventHandler(object sender, string data);
        public event MessageEventHandler OnMessageReceived;
        private int? latestS = null;
        public QCBClient(string botAppId, string botToken, string botSecret, bool isInSandbox)
        {
            appId = botAppId;
            token = botToken;
            secret = botSecret;
            httpClient.BaseAddress = new Uri(isInSandbox ? SandboxEnvironmentBase : FormalEnvironmentBase);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {botAppId}.{botToken}");
            string? gateway = GetWssGateway().Result;
            if (gateway is null) return;


            clientWebSocket.ConnectAsync(new Uri(gateway), CancellationToken.None).Wait();
            var byteArray = new byte[1024];
            clientWebSocket.ReceiveAsync(new ArraySegment<byte>(byteArray), CancellationToken.None).Wait();
            string connectResponseString = Encoding.UTF8.GetString(byteArray).TrimEnd('\0');
            Console.WriteLine(connectResponseString);
            var connectResponseMessage = JsonSerializer.Deserialize<WebSocketMessage>(connectResponseString);
            int interval = ((JsonElement)connectResponseMessage.d).Deserialize<ConnectResponseD>().heartbeat_interval;
            Timer timer = new Timer(Heartbeat, null, 0, interval);

            var message = new
            {
                op = 2,
                d = new
                {
                    token = $"{botAppId}.{botToken}",
                    intents = 0 | 1 << 30,
                }
            };
            string contentString = JsonSerializer.Serialize(message);
            Console.WriteLine(contentString);
            byteArray = Encoding.UTF8.GetBytes(contentString);
            clientWebSocket.SendAsync(new ArraySegment<byte>(byteArray), WebSocketMessageType.Text, true, CancellationToken.None).Wait();
            byteArray = new byte[1024];
            clientWebSocket.ReceiveAsync(new ArraySegment<byte>(byteArray), CancellationToken.None).Wait();
            Console.WriteLine(Encoding.UTF8.GetString(byteArray));

            Thread thread = new(WebSocketMessageListener);
            thread.IsBackground = true;
            thread.Start();

            
        }

        private void Heartbeat(object? state)
        {
            if (clientWebSocket.State == WebSocketState.Open || clientWebSocket.State == WebSocketState.CloseSent )
            {
                byte[] content;
                if (latestS is not null)
                    content = Encoding.UTF8.GetBytes($"{{\"op\": 1,\"d\": {latestS}}}");
                else
                    content = Encoding.UTF8.GetBytes("{\"op\": 1,\"d\": null}");
                clientWebSocket.SendAsync(new ArraySegment<byte>(content), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private void WebSocketMessageListener(object? obj)
        {
            
            while (true)
            {
                if (clientWebSocket.State == WebSocketState.Open || clientWebSocket.State == WebSocketState.CloseSent)
                {
                    byte[] buffer = new byte[1024];
                    byte[] fullResult = new byte[0];
                    var receiveResult = clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).Result;
                    fullResult = buffer;

                    while (receiveResult.EndOfMessage != true)
                    {
                        buffer = new byte[1024];
                        receiveResult = clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None).Result;
                        fullResult = ConcatByteArray(fullResult, buffer);
                    }

                    string content = Encoding.UTF8.GetString(fullResult).TrimEnd('\0');
                    if (string.IsNullOrEmpty(content))
                        continue;

                    Console.WriteLine(content);
                    var message = JsonSerializer.Deserialize<WebSocketMessage>(content);

                    if (message is not null)
                        latestS = message.s;
                }
            }
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
        public async Task<string?> GetWssGateway()
        {
            var response = await httpClient.GetFromJsonAsync<GetWssGatewayResponse>("/gateway", jsonSerializerOptions);
            return response?.Url;
        }

        public byte[] ConcatByteArray(byte[] array1, byte[] array2)
        {
            var result = new byte[array1.Length + array2.Length];
            array1.CopyTo(result, 0);
            array2.CopyTo(result, array1.Length);
            return result;
        }
    }
}