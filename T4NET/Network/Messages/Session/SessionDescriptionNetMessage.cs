using Microsoft.Xna.Framework.Net;
using T4NET.ZeGame;

namespace T4NET.Network.Messages.Session
{
    public class SessionDescriptionNetMessage : NetworkMessage
    {
        public override Protocol MessageId
        {
            get { return Protocol.NET_SESSION_DESCRIPTION; }
        }

        public GameSession Session { get; set; }

        public override bool Decode(PacketReader reader)
        {
            Session = GameSession.Unserialize(reader);
            return Session != null;
        }

        protected override bool EncodeContent(PacketWriter writer)
        {
            return GameSession.Serialize(writer, Session);
        }
    }
}
