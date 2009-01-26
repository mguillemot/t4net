﻿using Microsoft.Xna.Framework.Net;

namespace T4NET.Network.Messages.Session
{
    public class SessionJoinedMessage : Message
    {
        public NetworkSession Session { get; set; }

        public override Protocol MessageId
        {
            get { return Protocol.SESSION_JOINED; }
        }
    }
}
