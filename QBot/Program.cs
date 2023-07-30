using OpenAI;
using OpenAI.Managers;
using OpenAI.ObjectModels;
using OpenAI.ObjectModels.RequestModels;
using QCBSdk;
using QCBSdk.Types;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using static QBot.Secrets;

namespace QBot
{
    internal class Program
    {
        private static JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNamingPolicy = new JsonSnakeCaseLowerNamingPolicy(),
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };

        internal static QCBClient BotClient = new QCBClient(BotAppId, BotToken, BotSecret, true);

        static OpenAIService openAiService = new OpenAIService(new OpenAiOptions()
        {
            ApiKey = OpenAiApiKey,
        });

        static Dictionary<string, List<ChatMessage>> memberChatMessageListDict = new();
        static Regex atRemoveRegex = new("<@![0-9]+> ");
        static HttpClient imageClient = new HttpClient();
        static async Task Main(string[] args)
        {

            
            BotClient.OnAtMessageCreate += BotClient_OnAtMessageCreate;
            await BotClient.InitializeAsync(0 | 1 << 9 | 1 << 27 | 1 << 30);

            var guilds = await BotClient.GetBotGuildListAsync();
            var channels = await BotClient.GetChannelListAsync(guilds[0].Id);
            Console.WriteLine(channels.SerializeObject());
            MessageReference a = new MessageReference()
            {
                IgnoreGetMessageError = true,
                MessageId = "123456787656",
            };
            Console.WriteLine(JsonSerializer.Serialize(a, jsonSerializerOptions));
            while (true)
            {
                Console.WriteLine(BotClient.latestS);
                await Task.Delay(10000);
            }
            

        }

        private static async void BotClient_OnAtMessageCreate(Message message)
        {
            string inputContent = atRemoveRegex.Replace(message.Content, "", 1);
            string outputContent = "";
            Console.WriteLine(inputContent);
            if (inputContent.StartsWith("/image"))
            {
                var imageResult = await openAiService.Image.CreateImage(new ImageCreateRequest
                {
                    Prompt = inputContent.Replace("/image", ""),
                    N = 2,
                    Size = StaticValues.ImageStatics.Size.Size256,
                    ResponseFormat = StaticValues.ImageStatics.ResponseFormat.Url,
                    User = "TestUser"
                });
                if (imageResult.Successful)
                {
                    foreach (var item in imageResult.Results)
                    {
                        string path = Guid.NewGuid().ToString() + ".png";
                        Console.WriteLine(item.Url);
                        File.WriteAllBytes(path, await imageClient.GetByteArrayAsync(item.Url));

                        using FileStream fs = new FileStream(path, FileMode.Open);
                        await BotClient.SendMessageAsync(message.ChannelId, new RequestTypes.SendMessageRequest()
                        {
                            MsgId = message.Id,
                            /*
                            MessageReference = new MessageReference()
                            {
                                IgnoreGetMessageError = true,
                                MessageId = message.Id,
                            },
                            */
                        }, fs, path);
                    }
                }
                else //handle errors
                {
                    if (imageResult.Error == null)
                    {
                        throw new Exception("Unknown Error");
                    }
                    Console.WriteLine($"{imageResult.Error.Code}");
                }
                return;
            }

            
            if (!memberChatMessageListDict.ContainsKey(message.Author.Id))
            {
                memberChatMessageListDict.Add(message.Author.Id, new List<ChatMessage>
                {
                    ChatMessage.FromSystem("你是一只会说瞎话的机器人，叫做Q宝。"),
                });
            }
            memberChatMessageListDict[message.Author.Id].Add(ChatMessage.FromUser(inputContent));
            var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = memberChatMessageListDict[message.Author.Id],
                Model = Models.ChatGpt3_5Turbo,
            });
            if (completionResult.Successful)
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (var item in completionResult.Choices)
                {
                    stringBuilder.AppendLine(item.Message.Content);
                }
                outputContent = stringBuilder.ToString();
                memberChatMessageListDict[message.Author.Id].Add(ChatMessage.FromAssistant(outputContent));

                Console.WriteLine(outputContent);
            }
            else
            {
                outputContent = completionResult.Error?.ToString() ?? "出错了...";
            }
            await BotClient.SendMessageAsync(message.ChannelId, new RequestTypes.SendMessageRequest()
            {
                Content = outputContent,
                MsgId = message.Id,
                MessageReference = new MessageReference()
                {
                    IgnoreGetMessageError = true,
                    MessageId = message.Id,
                },
            });
        }
    }
}