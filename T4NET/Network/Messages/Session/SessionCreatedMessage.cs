using Microsoft.Xna.Framework.Net;

namespace T4NET.Network.Messages.Session
{
    public class SessionCreatedMessage : Message
    {
        public NetworkSession Session { get; set; }

        public override Protocol MessageId
        {
            get { return Protocol.SESSION_CREATED; }
        }
    }
}
