using Microsoft.Xna.Framework.Net;

namespace T4NET.Network
{
    public abstract class Message
    {
        public NetworkGamer Sender { get; set; }

        public abstract ushort MessageId { get; }

        public abstract bool Decode(PacketReader reader);

        public bool Encode(PacketWriter writer)
        {
            writer.Write(MessageId);
            return EncodeContent(writer);
        }

        protected abstract bool EncodeContent(PacketWriter writer);
    }
}
