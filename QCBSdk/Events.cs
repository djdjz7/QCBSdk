using QCBSdk.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace QCBSdk
{
    public partial class QCBClient
    {
        /// <summary>
        /// 接收到消息时的委托
        /// </summary>
        /// <param name="message">接受到的消息</param>
        public delegate void MessageEventHandler(Message message);
        /// <summary>
        /// 接收到 @机器人 消息时事件
        /// </summary>
        public event MessageEventHandler? OnAtMessageCreate;
        /// <summary>
        /// 接收到消息事件，代表频道内的全部消息，而不只是 @机器人 的消息。<br/>
        /// 仅 <b>私域</b> 机器人
        /// </summary>
        public event MessageEventHandler? OnMessageCreate;

        /// <summary>
        /// 未实现的其他 WebSocket 委托
        /// </summary>
        /// <param name="type"><see cref="WebSocketMessage.t"/> 字段</param>
        /// <param name="data"><see cref="WebSocketMessage.d"/> 字段</param>
        public delegate void UnknownEventHandler(string type, JsonElement data);

        /// <summary>
        /// 接收到未实现的 WebSocket 消息时事件
        /// </summary>
        public event UnknownEventHandler? OnUnknownWebSocketMessageReceived;
    }
}
