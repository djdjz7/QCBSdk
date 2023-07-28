using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static QCBSdk.Enums;
namespace QCBSdk.Types
{
    public class RequestTypes
    {
        /// <summary>
        /// 创建子频道所携带的请求<br/>
        /// <see href="https://bot.q.qq.com/wiki/develop/api/openapi/channel/post_channels.html">QQ 机器人文档</see>
        /// </summary>
        public class CreateChannelRequest
        {
            public string? Name { get; set; } = null;
            public ChannelType? Type { get; set; } = null;
            public int? Position { get; set; } = null;
            public string? ParentId { get; set; } = null;
            public string? OwnerId { get; set; } = null;
            public ChannelSubType? SubType { get; set; } = null;
            public PrivateType? PrivateType { get; set; } = null;
            public string[]? PrivateUserIds { get; set; } = null;
            public SpeakPermission? SpeakPermission { get; set; } = null;
            public string? ApplicationId { get; set; } = null;
        }

        /// <summary>
        /// 修改子频道所携带的请求<br/>
        /// <see href="https://bot.q.qq.com/wiki/develop/api/openapi/channel/patch_channels.html">QQ 机器人文档</see>
        /// </summary>
        public class EditChannelRequest
        {
            public string? Name { get; set; } = null;
            public string? Position { get; set; } = null;
            public string? ParentId { get; set; } = null;
            public PrivateType? PrivateType { get; set; } = null;
            public SpeakPermission? SpeakPermission { get; set; } = null;
        }

        public class SendMessageRequest
        {
            public string? Content { get; set; }
            public MessageEmbed? Embed { get; set; }
            public MessageArk? Ark { get; set; }
            public MessageReference? MessageReference { get; set; }
            public string? Image { get; set; }
            public string? MsgId { get; set; }
            public string? EventId { get; set; }
            public MessageMarkdown? Markdown { get; set; }
        }
    }
}
