using QCBSdk;
using QCBSdk.Types;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using static QBot.Secrets;

namespace QBot
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var botClient = new QCBClient(BotAppId, BotToken, BotSecret, true);
            await botClient.InitializeAsync();
            /*
            var guilds = await botClient.GetBotGuildListAsync();
            var channels = await botClient.GetChannelListAsync(guilds[0].Id);
            Console.WriteLine(channels.SerializeObject());
            var cid = Console.ReadLine();*/

            while (true)
            {
                Console.WriteLine(botClient.latestS);
                await Task.Delay(10000);
            }

        }
    }
}