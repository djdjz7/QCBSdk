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
        public delegate void MessageEventHandler(Message message);
        public event MessageEventHandler? OnAtMessageCreate;
        public event MessageEventHandler? OnMessageCreate;

        public delegate void DefaultEventHandler(JsonElement data);
        public event DefaultEventHandler? OnUnknownMessageCreate;
    }
}
