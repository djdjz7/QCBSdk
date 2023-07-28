namespace QCBSdk.Types
{
    public class Member
    {
        public User User { get; set; }
        public string Nick { get; set; }
        public string[] Roles { get; set; }
        public DateTime JoinedAt { get; set; }
    }
    public class MemberWithGuildId : Member
    {
        public string GuildId { get; set; }
    }
}
