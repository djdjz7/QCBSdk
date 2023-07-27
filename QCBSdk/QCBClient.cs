using System.Net.Http.Json;
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
        private JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };
        public QCBClient(string botAppId, string botToken, string botSecret, bool isInSandbox)
        {
            appId = botAppId;
            token = botToken;
            secret = botSecret;
            httpClient.BaseAddress = new Uri(isInSandbox ? SandboxEnvironmentBase : FormalEnvironmentBase);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {botAppId}.{botToken}");
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
    }
}