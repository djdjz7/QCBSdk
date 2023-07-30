namespace QCBSdk.Types
{
    public enum ChannelType
    {
        /// <summary>
        /// 文字子频道
        /// </summary>
        TextChannel = 0,
        /// <summary>
        /// 保留，不可用
        /// </summary>
        Reserved1 = 1,
        /// <summary>
        /// 语音子频道
        /// </summary>
        VoiceChannel = 2,
        /// <summary>
        /// 保留，不可用
        /// </summary>
        Reserved3 = 3,
        /// <summary>
        /// 子频道分组
        /// </summary>
        SubChannelGroup = 4,
        /// <summary>
        /// 直播子频道
        /// </summary>
        LiveSubChannel = 10005,
        /// <summary>
        /// 应用子频道
        /// </summary>
        ApplicationSubChannel = 10006,
        /// <summary>
        /// 论坛子频道
        /// </summary>
        ForumSubChannel = 10007,
    }
    public enum ChannelSubType
    {
        /// <summary>
        /// 闲聊
        /// </summary>
        Chat = 0,
        /// <summary>
        /// 公告
        /// </summary>
        Notice = 1,
        /// <summary>
        /// 攻略
        /// </summary>
        Guide = 2,
        /// <summary>
        /// 开黑
        /// </summary>
        Gaming = 3,
    }
    public enum PrivateType
    {
        /// <summary>
        /// 公开频道
        /// </summary>
        Public = 0,
        /// <summary>
        /// 群主管理员可见
        /// </summary>
        OwnerAdmin = 1,
        /// <summary>
        /// 群主管理员+指定成员，可使用 修改子频道权限接口 指定成员
        /// </summary>
        OwnerAdminSpecified = 2,
    }
    public enum SpeakPermission
    {
        /// <summary>
        /// 无效类型
        /// </summary>
        Invalid = 0,
        /// <summary>
        /// 所有人
        /// </summary>
        Everyone = 1,
        /// <summary>
        /// 群主管理员+指定成员，可使用 修改子频道权限接口 指定成员
        /// </summary>
        OwnerAdminSpecified = 2,
    }

    public enum Opcode
    {
        /// <summary>
        /// Receive 服务端进行消息推送
        /// </summary>
        Dispatch = 0,
        /// <summary>
        /// Send/Receive 客户端或服务端发送心跳
        /// </summary>
        Heartbeat = 1,
        /// <summary>
        /// Send 客户端发送鉴权
        /// </summary>
        Identity = 2,
        /// <summary>
        /// Send 客户端恢复连接
        /// </summary>
        Resume = 6,
        /// <summary>
        /// Receive 服务端通知客户端重新连接
        /// </summary>
        Reconnect = 7,
        /// <summary>
        /// Receive 当 identify 或 resume 的时候，如果参数有错，服务端会返回该消息
        /// </summary>
        InvalidSession = 9,
        /// <summary>
        /// Receive 当客户端与网关建立ws连接之后，网关下发的第一条消息
        /// </summary>
        Hello = 10,
        /// <summary>
        /// Receive/Reply 当发送心跳成功之后，就会收到该消息
        /// </summary>
        HeartbeatACK = 11,
        /// <summary>
        /// Reply 仅用于 http 回调模式的回包，代表机器人收到了平台推送的数据
        /// </summary>
        HttpCallbackACK = 12,
    }

}
