using QCBSdk;
using QCBSdk.Types;
using static QCBSdk.Types.RequestTypes;
using static QBot.Secrets;

namespace QBot
{
    internal class Program
    {
        public static QCBClient BotClient = new QCBClient(BotAppId, BotToken, BotSecret, true);
        static async Task Main(string[] args)
        {
            BotClient.OnMessageCreate += BotClient_OnMessageCreate;
            await BotClient.InitializeAsync(0 | 1 << 9 | 1 << 27 | 1 << 30);
            
            var guilds = await BotClient.GetBotGuildListAsync();
            var channels = await BotClient.GetChannelListAsync(guilds[0].Id);
            Console.WriteLine(channels.SerializeObject());



            while (true)
            {
                Console.WriteLine(BotClient.latestS);
                await Task.Delay(10000);
            }

        }

        private static void BotClient_OnMessageCreate(Message message)
        {
            Console.WriteLine(message.SerializeObject());
            if(message.Content == "Q宝，这是复读机")
            {
                BotClient.SendMessageAsync(message.ChannelId, new SendMessageRequest
                {
                    Content = "好的，这是复读机",
                    MessageReference = new MessageReference()
                    {
                        IgnoreGetMessageError = true,
                        MessageId = message.Id,
                    },
                }).Wait();
                BotClient.OnMessageCreate += Repeater;
            }
            else if (message.Content == "差不多得了")
            {
                BotClient.SendMessageAsync(message.ChannelId, new SendMessageRequest
                {
                    Content = "😅",
                    MessageReference = new MessageReference()
                    {
                        IgnoreGetMessageError = true,
                        MessageId = message.Id,
                    },
                }).Wait();
                BotClient.OnMessageCreate -= Repeater;
            }
        }

        private static void Repeater(Message message)
        {
            BotClient.SendMessageAsync(message.ChannelId, new SendMessageRequest
            {
                Content = message.Content,
                MessageReference = new MessageReference()
                {
                    IgnoreGetMessageError = true,
                    MessageId = message.Id,
                },
            }).Wait();
        }
    }
}