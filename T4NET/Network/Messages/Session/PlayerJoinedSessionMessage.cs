using Microsoft.Xna.Framework.Net;

namespace T4NET.Network.Messages.Session
{
    public class PlayerJoinedSessionMessage : Message
    {
        public override Protocol MessageId
        {
            get { return Protocol.PLAYER_JOINED_SESSION; }
        }

        public NetworkGamer Gamer { get; set; }
    }
}
