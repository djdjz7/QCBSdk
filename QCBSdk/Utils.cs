using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace QCBSdk
{
    public class SnakeCaseNamingPolicy : JsonNamingPolicy
    {
        readonly Regex pascalSplittingRegex = new Regex(@"(?<!^)(?=[A-Z])");
        public override string ConvertName(string name)
        {
            return string.Join('_', pascalSplittingRegex.Split(name)).ToLower();
        }
    }

    public static class Utils
    {
        public static byte[] ConcatByteArray(byte[] array1, byte[] array2)
        {
            var result = new byte[array1.Length + array2.Length];
            array1.CopyTo(result, 0);
            array2.CopyTo(result, array1.Length);
            return result;
        }
    }
}
