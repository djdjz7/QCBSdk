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
            while (true) { }
        }
        
    }
}