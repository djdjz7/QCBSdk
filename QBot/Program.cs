using QCBSdk;
using System.Text.Json;
using static QBot.Secrets;

namespace QBot
{
    internal class Program
    {
        enum TestEnum
        {
            A = 1, B = 2, C = 3,
        }
        class TestClass
        {
            public TestEnum te { get; set; }
        }

        static async Task Main(string[] args)
        {
            
            Console.WriteLine("Hello, World!");
            var botClient = new QCBClient(BotAppId, BotToken, BotSecret, true);
            var channelList = await botClient.GetGuildListAsync();
            if(channelList is not null)
            {
                foreach (var channel in channelList)
                {
                    Console.WriteLine(channel.Name);
                }
            }
        }
    }
}