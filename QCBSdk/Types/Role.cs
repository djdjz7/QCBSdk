using System.Text.Json;

namespace QCBSdk.Types
{
    /// <summary>
    /// 
    /// </summary>
    public class Role
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint Color { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint Hoist { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint Number { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public uint NumberLimit { get; set; }
    }

    /// <summary>
    /// 系统默认生成下列身份组 ID
    /// </summary>
    [Obsolete("值应当为 string", error: true)]
    public enum DefaultRoleIDs
    {
        /// <summary>
        /// 全体成员
        /// </summary>
        All = 1,
        /// <summary>
        /// 管理员
        /// </summary>
        Administrator = 2,
        /// <summary>
        /// 群主/创建者
        /// </summary>
        Owner = 4,
        /// <summary>
        /// 子频道管理员
        /// </summary>
        ChannelAdministrator = 5,
    }
}
