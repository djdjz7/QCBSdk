﻿using QCBSdk.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QCBSdk
{
    public class WebSocketMessage
    {
        public Opcode op { get; set; }
        public object d { get; set; }
        public int? s { get; set; }
        public string t { get; set; }
    }

    public class ConnectResponseD
    {
        public int heartbeat_interval { get; set; }
    }
    public class ReadyD
    {
        public int version { get; set; }
        public string session_id { get; set; }
        public User user { get; set; }
    }
}
