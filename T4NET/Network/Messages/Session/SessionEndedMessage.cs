using Microsoft.Xna.Framework.Net;

namespace T4NET.Network.Messages.Session
{
    public class SessionEndedMessage : Message
    {
        public NetworkSession Session { get; set; }
        public NetworkSessionEndReason EndReason { get; set; }

        public override Protocol MessageId
        {
            get { return Protocol.SESSION_ENDED; }
        }
    }
}
