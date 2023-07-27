using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static QCBSdk.Enums;

namespace QCBSdk
{
    public class BasicObjects
    {
        public class Guild
        {
            public string? Id { get; set; } = null;
            public string? Name { get; set; } = null;
            public string? Icon { get; set; } = null;
            public string? OwnerId { get; set; } = null;
            public bool? Owner { get; set; } = null;
            public DateTime? JoinedAt { get; set; } = null;
            public int? MemberCount { get; set; } = null;
            public int? MaxMembers { get; set; } = null;
            public string? Description { get; set; } = null;
        }
        public class Channel
        {
            public string? Id { get; set; } = null;
            public string? GuildId { get; set; } = null;
            public string? Name { get; set; } = null;
            public ChannelType? Type { get; set; } = null;
            public int? Position { get; set; } = null;
            public string? ParentId { get; set; } = null;
            public string? OwnerId { get; set; } = null;
            public ChannelSubType? SubType { get; set; } = null;
            public PrivateType? PrivateType { get; set; } = null;
            public SpeakPermission? SpeakPermission { get; set; } = null;
            public string? ApplicationId { get; set; } = null;
            public string? Permissions { get; set; } = null;
        }
        public class User
        {
            public string? Id { get; set; } = null;
            public string? Username { get; set; } = null;
            public string? Avatar { get; set; } = null;
            public string? UnionOpenid { get; set; } = null;
            public string? UnionUserAccount { get; set; } = null;
        }
    }
}
