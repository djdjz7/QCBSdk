using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QCBSdk
{
    public class ResponseTypes
    {
        public class Guild
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("icon")]
            public string Icon { get; set; }
            [JsonPropertyName("owner_id")]
            public string OwnerId { get; set; }
            [JsonPropertyName("owner")]
            public bool Owner { get; set; }
            [JsonPropertyName("joined_at")]
            public DateTime JoinedAt { get; set; }
            [JsonPropertyName("member_count")]
            public int MemberCount { get; set; }
            [JsonPropertyName("max_members")]
            public int MaxMembers { get; set; }
            [JsonPropertyName("description")]
            public string Description { get; set; }
        }

        public class Channel
        {
            [JsonPropertyName("id")]
            public string Id { get; set; }
            [JsonPropertyName("guild_id")]
            public string GuildId { get; set; }
            [JsonPropertyName("name")]
            public string Name { get; set; }
            [JsonPropertyName("type")]
            public int Type { get; set; }
            [JsonPropertyName("position")]
            public int Position { get; set; }
            [JsonPropertyName("parent_id")]
            public string ParentId { get; set; }
            [JsonPropertyName("owner_id")]
            public string OwnerId { get; set; }
            [JsonPropertyName("sub_type")]
            public int SubType { get; set; }
            [JsonPropertyName("private_type")]
            public int PrivateType { get; set; }
        }
        public enum ChannelType
        {
            TextChannel = 0,
            Reserved1 = 1,
            VoiceChannel = 2,
            Reserved3 = 3,
            SubChannelGroup = 4,
            LiveSubChannel = 10005,
            ApplicationSubChannel = 10006,
            ForumSubChannel = 10007,
        }
        public enum ChannelSubType
        {
            Chat = 0,
            Notice = 1,
            Guide = 2,
            Gaming = 3, //开黑？不会翻
        }
    }
}
