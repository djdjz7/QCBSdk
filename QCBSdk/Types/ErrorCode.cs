﻿namespace QCBSdk.Types
{
    public enum ErrorCode
    {

        UnknownAccount = 10001,
        UnknownChannel = 10003,
        UnknownGuild = 10004,
        ErrorCheckAdminFailed = 11281,
        ErrorCheckAdminNotPass = 11282,
        ErrorWrongAppid = 11251,
        ErrorCheckAppPrivilegeFailed = 11252,
        ErrorCheckAppPrivilegeNotPass = 11253,
        ErrorInterfaceForbidden = 11254,
        ErrorWrongAppid2 = 11261,
        ErrorCheckRobot = 11262,
        ErrorCheckGuildAuth = 11263,
        ErrorGuildAuthNotPass = 11264,
        ErrorRobotHasBanned = 11265,
        ErrorWrongToken = 11241,
        ErrorCheckTokenFailed = 11242,
        ErrorCheckTokenNotPass = 11243,
        ErrorCheckUserAuth = 11273,
        ErrorUserAuthNotPass = 11274,
        ErrorWrongAppid3 = 11275,
        ErrorGetHTTPHeader = 11301,
        ErrorGetHeaderUIN = 11302,
        ErrorGetNick = 11303,
        ErrorGetAvatar = 11304,
        ErrorGetGuildID = 11305,
        ErrorGetGuildInfo = 11306,
        ReplaceIDFailed = 12001,
        RequestInvalid = 12002,
        ResponseInvalid = 12003,
        ChannelHitWriteRateLimit = 20028,
        CannotSendEmptyMessage = 50006,
        InvalidFormBody = 50035,
        InvalidMarkdownCombination = 50037,   // 带有 markdown 消息只支持 markdown 或者 keyboard 组合
        GuildIdMismatch = 50038,  //	非同频道同子频道
        ErrorGetMessages = 50039,  //	获取消息失败
        InvalidMessageTemplate = 50040,  //	消息模版类型错误
        EmptyMarkdown = 50041,  //	markdown 有空值
        MarkdownListExceededMaximum = 50042,  //	markdown 列表长达最大值
        ErrorParseGuildId = 50043,  //	guild_id 转换失败
        ReplySelf = 50045,  //	不能回复机器人自己产生的消息
        NotAnAtMessage = 50046,  //	非 at 机器人消息
        NotBotMessage = 50047,  //	非机器人产生的消息 或者 at 机器人消息
        InvalidMessageId = 50048,  //	message id 不能为空
        ModifyBeyondKeyboardMessage = 50049,  //	只能修改含有 keyboard 元素的消息
        InvalidKeyboard = 50050,  //	修改消息时，keyboard 元素不能为空
        ModifyBeyondBotMessage = 50051,  //	只能修改机器人自己发送的消息
        ErrorModifyMessage = 50053,  //	修改消息错误
        InvalidMarkdownTemplate = 50054,  //	markdown 模版参数错误
        InvalidMarkdownContent = 50055,  //	无效的 markdown content
        MarkdownContentNotAllowed = 50056,  //	不允许发送 markdown content
        MultipleMarkdownParams = 50057,  //	markdown 参数只支持原生语法或者模版二选一
        /*

        301000~301099	子频道权限错误
        301000	参数错误
        301001	查询频道信息错误
        301002	查询子频道权限错误
        301003	修改子频道权限错误
        301004	私密子频道关联的人数到达上限
        301005	调用 Rpc 服务失败
        301006	非群成员没有查询权限
        301007	参数超过数量限制
        302000	参数错误
        302001	查询频道信息错误
        302002	查询日程列表失败
        302003	查询日程失败
        302004	修改日程失败
        302005	删除日程失败
        302006	创建日程失败
        302007	获取创建者信息失败
        302008	子频道 ID 不能为空
        302009	频道系统错误，请联系客服
        302010	暂无修改日程权限
        302011	日程活动已被删除
        302012	每天只能创建 10 个日程，明天再来吧！
        302013	创建日程触发安全打击
        302014	日程持续时间超过 7 天，请重新选择
        302015	开始时间不能早于当前时间
        302016	结束时间不能早于开始时间
        302017	Schedule 对象为空
        302018	参数类型转换失败
        302019	调用下游失败，请联系客服
        302020	日程内容违规、账号违规
        302021	频道内当日新增活动达上限
        302022	不能绑定非当前频道的子频道
        302023	开始时跳转不可绑定日程子频道
        302024	绑定的子频道不存在
        304003	URL_NOT_ALLOWED url 未报备
        304004	ARK_NOT_ALLOWED 没有发 ark 消息权限
        304005	EMBED_LIMIT embed 长度超限
        304006	SERVER_CONFIG 后台配置错误
        304007	GET_GUILD 查询频道异常
        304008	GET_BOT 查询机器人异常
        304009	GET_CHENNAL 查询子频道异常
        304010	CHANGE_IMAGE_URL 图片转存错误
        304011	NO_TEMPLATE 模板不存在
        304012	GET_TEMPLATE 取模板错误
        304014	TEMPLATE_PRIVILEGE 没有模板权限
        304016	SEND_ERROR 发消息错误
        304017	UPLOAD_IMAGE 图片上传错误
        304018	SESSION_NOT_EXIST 机器人没连上 gateway
        304019	AT_EVERYONE_TIMES @全体成员 次数超限
        304020	FILE_SIZE 文件大小超限
        304021	GET_FILE 下载文件错误
        304022	PUSH_TIME 推送消息时间限制
        304023	PUSH_MSG_ASYNC_OK 推送消息异步调用成功, 等待人工审核
        304024	REPLY_MSG_ASYNC_OK 回复消息异步调用成功, 等待人工审核
        304025	BEAT 消息被打击
        304026	MSG_ID 回复的消息 id 错误
        304027	MSG_EXPIRE 回复的消息过期
        304028	MSG_PROTECT 非 At 当前用户的消息不允许回复
        304029	CORPUS_ERROR 调语料服务错误
        304030	CORPUS_NOT_MATCH 语料不匹配
        304031	私信已关闭
        304032	私信不存在
        304033	拉私信错误
        304034	不是私信成员
        304035	推送消息超过子频道数量限制
        304036	没有 markdown 模板的权限
        304037	没有发消息按钮组件的权限
        304038	消息按钮组件不存在
        304039	消息按钮组件解析错误
        304040	消息按钮组件消息内容错误
        304044	取消息设置错误
        304045	子频道主动消息数限频
        304046	不允许在此子频道发主动消息
        304047	主动消息推送超过限制的子频道数
        304048	不允许在此频道发主动消息
        304049	私信主动消息数限频
        304050	私信主动消息总量限频
        304051	消息设置引导请求构造错误
        304052	发消息设置引导超频
        306001	param invalid 撤回消息参数错误
        306002	msgid error 消息 id 错误
        306003	fail to get message 获取消息错误(可重试)
        306004	no permission to delete message 没有撤回此消息的权限
        306005	retract message error 消息撤回失败(可重试)
        306006	fail to get channel 获取子频道失败(可重试)
        501000~501999	公告错误
        501001	参数校验失败
        501002	创建子频道公告失败(可重试)
        501003	删除子频道公告失败(可重试)
        501004	获取频道信息失败(可重试)
        501005	MessageID 错误
        501006	创建频道全局公告失败(可重试)
        501007	删除频道全局公告失败(可重试)
        501008	MessageID 不存在
        501009	MessageID 解析失败
        501010	此条消息非子频道内消息
        501011	创建精华消息失败(可重试)
        501012	删除精华消息失败(可重试)
        501013	精华消息超过最大数量
        501014	安全打击
        501015	此消息不允许设置
        501016	频道公告子频道推荐超过最大数量
        501017	非频道主或管理员
        501018	推荐子频道 ID 无效
        501019	公告类型错误
        501020	创建推荐子频道类型频道公告失败
        502000~502099	禁言相关错误
        502001	频道 id 无效
        502002	频道 id 为空
        502003	用户 id 无效
        502004	用户 id 为空
        502005	timestamp 不合法
        502006	timestamp 无效
        502007	参数转换错误
        502008	rpc 调用失败
        502009	安全打击
        502010	请求头错误
        503001	频道 id 无效
        503002	频道 id 为空
        503003	获取子频道信息失败
        503004	超出发布帖子的频次限制
        503005	帖子标题为空
        503006	帖子内容为空
        503007	帖子ID为空
        503008	获取X-Uin失败
        503009	帖子ID无效或不合法
        503010	通过Uin获取TinyID失败
        503011	帖子ID里面的时间戳无效或不合法
        503012	帖子不存在或已删除
        503013	服务器内部错误
        503014	帖子JSON内容解析失败
        503015	帖子内容转换失败
        503016	链接数量超过限制
        503017	字数超过限制
        503018	图片数量超过限制
        503019	视频数量超过限制
        503020	标题长度超过限制
        504000~504999	消息频率相关错误
        504001	请求参数无效错误
        504002	获取 HTTP 头失败
        504003	获取 BOT UIN 错误
        504004	获取消息频率设置信息错误
        505000	获取子频道信息失败
        505001	获取机器人信息失败
        505002	非语音频道
        505003	非音频机器人
        505004	上麦失败
        505005	下麦失败
        610000-619999	频道权限错误 ~~
        610001	获取频道 ID 失败
        610002	获取 HTTP 头失败
        610003	获取机器人号码失败
        610004	获取机器人角色失败
        610005	获取机器人角色内部错误
        610006	拉取机器人权限列表失败
        610007	机器人不在频道内
        610008	无效参数
        610009	获取 API 接口详情失败
        610010	API 接口已授权
        610011	获取机器人信息失败
        610012	限频失败
        610013	已限频
        610014	api 授权链接发送失败
        620001-629999	表情表态错误
        620001	表情表态无效参数
        620002	已经达到表情反应的类型数量上限
        620003	已经设置过该表情表态
        620004	没有设置过该表情表态
        620005	没有权限设置表情表态
        620006	操作限频
        620007	表情表态操作失败，请重试
        620008	无效表情
        630001-639999	互动回调数据更新
        630001	互动回调数据更新无效参数
        630002	互动回调数据更新获取AppID失败
        630003	互动回调数据AppID不匹配
        630004	互动回调数据更新内部存储错误
        630005	互动回调数据更新内部存储读取错误
        630006	互动回调数据更新读取请求AppID失败
        630007	互动回调数据太大
        1000000~2999999	发消息错误
        1100100	安全打击：消息被限频
        1100101	安全打击：内容涉及敏感，请返回修改
        1100102	安全打击：抱歉，暂未获得新功能体验资格
        1100103	安全打击
        1100104	安全打击：该群已失效或当前群已不存在
        1100300	系统内部错误
        1100301	调用方不是群成员
        1100302	获取指定频道名称失败
        1100303	主页频道非管理员不允许发消息
        1100304	@次数鉴权失败
        1100305	TinyId 转换 Uin 失败
        1100306	非私有频道成员
        1100307	非白名单应用子频道
        1100308	触发频道内限频
        1100499	其他错误
        3000000~3999999	编辑消息错误
        3300006	安全打击*/
    }
}