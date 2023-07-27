using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace QCBSdk
{
    public class Enums
    {
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
            /*            
            值   描述
            0	文字子频道
            1	保留，不可用
            2	语音子频道
            3	保留，不可用
            4	子频道分组
            10005	直播子频道
            10006	应用子频道
            10007	论坛子频道*/
        }
        public enum ChannelSubType
        {
            Chat = 0,
            Notice = 1,
            Guide = 2,
            Gaming = 3,
            /*
            值	描述
            0	闲聊
            1	公告
            2	攻略
            3	开黑*/
        }
        public enum PrivateType
        {
            Public = 0,
            OwnerAdmin = 1,
            OwnerAdminSpecified = 2,
            /*
            值   描述
            0	公开频道
            1	群主管理员可见
            2	群主管理员+指定成员，可使用 修改子频道权限接口 指定成员*/
        }
        public enum SpeakPermission
        {
            Invalid = 0,
            Everyone = 1,
            OwnerAdminSpecified = 2,
            /*
            值	描述
            0	无效类型
            1	所有人
            2	群主管理员+指定成员，可使用 修改子频道权限接口 指定成员*/
        }

        public enum Opcode
        {
            Dispatch = 0,
            Heartbeat = 1,
            Identity = 2,
            Resume = 6,
            Reconnect = 7,
            InvalidSession = 9,
            Hello = 10,
            HeartbeatACK = 11,
            HttpCallbackACK = 12,
        }
    }
}
