using Microsoft.Xna.Framework.Net;
using T4NET.ZeGame;

namespace T4NET.Network.Messages.Gameplay
{
    public class PlayerIncorporatePieceNetMessage : NetworkMessage
    {
        public PieceType Type;
        public sbyte X;
        public sbyte Y;

        public override Protocol MessageId
        {
            get { return Protocol.NET_PLAYER_INCORPORATE_PIECE; }
        }

        public override bool Decode(PacketReader reader)
        {
            Type = (PieceType) reader.ReadByte();
            X = reader.ReadSByte();
            Y = reader.ReadSByte();
            return true;
        }

        protected override bool EncodeContent(PacketWriter writer)
        {
            writer.Write((byte) Type);
            writer.Write(X);
            writer.Write(Y);
            return true;
        }
    }
}
