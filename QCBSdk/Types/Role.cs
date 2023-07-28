using System.Text.Json;

namespace QCBSdk.Types
{
    public class Role
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public uint Color { get; set; }
        public uint Hoist { get; set; }
        public uint Number { get; set; }
        public uint NumberLimit { get; set; }
    }
    [Obsolete("值应当为 string", error: true)]
    public enum DefaultRoleIDs
    {
        All = 1,
        Administrator = 2,
        Owner = 4,
        ChannelAdministrator = 5,
    }
}
