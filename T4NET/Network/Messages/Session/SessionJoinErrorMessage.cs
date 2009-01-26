using Microsoft.Xna.Framework.Net;

namespace T4NET.Network.Messages.Session
{
    public class SessionJoinErrorMessage : Message
    {
        public NetworkSessionJoinError JoinError { get; set; }

        public override Protocol MessageId
        {
            get { return Protocol.SESSION_JOIN_ERROR; }
        }
    }
}