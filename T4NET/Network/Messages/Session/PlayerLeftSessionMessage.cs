using Microsoft.Xna.Framework.Net;

namespace T4NET.Network.Messages.Session
{
    public class PlayerLeftSessionMessage : Message
    {
        public override Protocol MessageId
        {
            get { return Protocol.PLAYER_LEFT_SESSION; }
        }

        public NetworkGamer Gamer { get; set; }
    }
}
