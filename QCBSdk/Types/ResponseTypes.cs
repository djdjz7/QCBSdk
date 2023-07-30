namespace QCBSdk.Types
{
    public class ResponseTypes
    {
        internal class GetChannelOnlineCountResponse
        {
            public int? OnlineNums { get; set; }
        }
        internal class GetWssGatewayResponse
        {
            public string? Url { get; set; }
        }

        /// <summary>
        /// 请求错误响应
        /// </summary>
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
