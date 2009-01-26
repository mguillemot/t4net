using Microsoft.Xna.Framework.Net;
using T4NET.ZeGame;

namespace T4NET.Network.Messages.Gameplay
{
    public class BoadFullContentNetMessage : NetworkMessage
    {
        public Board Board { get; set; }

        public override Protocol MessageId
        {
            get { return Protocol.NET_BOARD_FULL_CONTENT; }
        }

        public override bool Decode(PacketReader reader)
        {
            Board = Board.Unserialize(reader);
            return Board != null;
        }

        protected override bool EncodeContent(PacketWriter writer)
        {
            return Board.Serialize(writer, Board);
        }
    }
}