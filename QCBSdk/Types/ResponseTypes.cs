namespace QCBSdk.Types
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
        public class RequestErrorResponse
        {
                public int Code { get; set; }
                public string Message { get; set; }
                public object Data { get; set; }
        }

        public class MessageAuditData
        {
            public MessageAudit MessageAudit { get; set; }
        }

        
    }

}
