using System.Net.Http.Json;
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
        public QCBClient(string botAppId, string botToken, string botSecret, bool isInSandbox)
        {
            appId = botAppId;
            token = botToken;
            secret = botSecret;
            httpClient.BaseAddress = new Uri(isInSandbox ? SandboxEnvironmentBase : FormalEnvironmentBase);
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bot {botAppId}.{botToken}");
        }
        public async Task<List<Guild>?> GetGuildListAsync()
        {
            var response = await httpClient.GetFromJsonAsync<List<Guild>>("/users/@me/guilds");
            return response;
        }
        //public async Task<List<>>
        public async Task SendMessageAsync(string channelId, string message)
        {

        }
    }
}