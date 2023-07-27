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
}
