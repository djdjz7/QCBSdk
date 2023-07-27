using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static QCBSdk.Enums;
namespace QCBSdk
{
    public class RequestTypes
    {
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
        public class EditChannelRequest
        {
            public string? Name { get; set; } = null;
            public string? Position { get; set; } = null;
            public string? ParentId { get; set; } = null;
            public PrivateType? PrivateType { get; set; } = null;
            public SpeakPermission? SpeakPermission { get; set; } = null;
        }
    }
}
