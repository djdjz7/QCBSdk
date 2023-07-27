using static QCBSdk.Enums;
namespace QCBSdk
{
    public class ResponseTypes
    {
        public class GetChannelOnlineCountResponse
        {
            public int? OnlineNums { get; set; }
        }
        public class GetWssGatewayResponse
        {
            public string? Url { get; set; }
        }
    }
    
}
