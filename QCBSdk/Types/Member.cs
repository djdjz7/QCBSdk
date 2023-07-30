namespace QCBSdk.Types
{
    /// <summary>
    /// 成员对象
    /// </summary>
    public class Member
    {
        /// <summary>
        /// 用户的频道基础信息，只有成员相关接口中会填充此信息
        /// </summary>
        public User User { get; set; }
        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string Nick { get; set; }
        /// <summary>
        /// 用户在频道内的身份组ID, 默认值可参考 <see cref="DefaultRoleIDs"/>
        /// </summary>
        public string[] Roles { get; set; }
        /// <summary>
        /// 用户加入频道的时间
        /// </summary>
        public DateTime JoinedAt { get; set; }
    }
    /// <summary>
    /// 成员对象
    /// </summary>
    public class MemberWithGuildId : Member
    {
        /// <summary>
        /// 频道 ID
        /// </summary>
        public string GuildId { get; set; }
    }
}
