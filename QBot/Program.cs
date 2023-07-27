using QCBSdk;
using System.Runtime.CompilerServices;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Xml.Linq;
using static QBot.Secrets;
using static QCBSdk.BasicObjects;
using static QCBSdk.RequestTypes;
using static QCBSdk.Enums;
namespace QBot
{
    internal class Program
    {
        
        static async Task Main(string[] args)
        {
            var botClient = new QCBClient(BotAppId, BotToken, BotSecret, true);
            var botDetails = await botClient.GetBotDetailsAsync();
            Console.WriteLine(botDetails.SerializeObject());
            Console.WriteLine(botDetails.Avatar);
            var guildList = await botClient.GetBotGuildListAsync();
            Console.WriteLine(guildList.SerializeObject());
            var channelList = await botClient.GetChannelListAsync(guildList[0].Id);
            Console.WriteLine(channelList.SerializeObject());

            string channelId = Console.ReadLine();
            Console.WriteLine(await botClient.GetChannelOnlineCount(channelId));
        }
        
    }
}