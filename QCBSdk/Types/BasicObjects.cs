using static QCBSdk.Enums;

namespace QCBSdk.Types
{
    /// <summary>
    /// 频道对象 <br/>
    /// <see href="https://bot.q.qq.com/wiki/develop/api/openapi/guild/model.html">QQ 机器人文档</see>
    /// </summary>
    public class Guild
    {
        /// <summary>
        /// 频道 ID
        /// </summary>
        public string? Id { get; set; } = null;
        /// <summary>
        /// 频道名称
        /// </summary>
        public string? Name { get; set; } = null;
        /// <summary>
        /// 频道头像地址
        /// </summary>
        public string? Icon { get; set; } = null;
        /// <summary>
        /// 创建人用户 ID
        /// </summary>
        public string? OwnerId { get; set; } = null;
        /// <summary>
        /// 当前人是否是创建人
        /// </summary>
        public bool? Owner { get; set; } = null;
        /// <summary>
        /// 加入时间
        /// </summary>
        public DateTime? JoinedAt { get; set; } = null;
        /// <summary>
        /// 成员数
        /// </summary>
        public int? MemberCount { get; set; } = null;
        /// <summary>
        /// 最大成员数
        /// </summary>
        public int? MaxMembers { get; set; } = null;
        /// <summary>
        /// 描述
        /// </summary>
        public string? Description { get; set; } = null;
    }
    /// <summary>
    /// 子频道对象 <br/>
    /// <see href="https://bot.q.qq.com/wiki/develop/api/openapi/channel/model.html">QQ 机器人文档</see>
    /// </summary>
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

    /// <summary>
    /// 用户对象 <br/>
    /// <see href="https://bot.q.qq.com/wiki/develop/api/openapi/user/model.html">QQ 机器人文档</see>
    /// </summary>
    public class User
    {
        public string? Id { get; set; } = null;
        public string? Username { get; set; } = null;
        public string? Avatar { get; set; } = null;
        public string? UnionOpenid { get; set; } = null;
        public string? UnionUserAccount { get; set; } = null;
    }

    
}
