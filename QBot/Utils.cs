using QCBSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
namespace QBot
{
    public static class Utils
    {
        static JsonSerializerOptions options = new JsonSerializerOptions()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,
            PropertyNamingPolicy = new SnakeCaseNamingPolicy(),
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
        };
        public static string SerializeObject(this object? obj)
        {
            return JsonSerializer.Serialize(obj, options);
        }
    }
}
