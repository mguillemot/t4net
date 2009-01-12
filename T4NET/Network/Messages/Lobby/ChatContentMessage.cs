using Microsoft.Xna.Framework.Net;

namespace T4NET.Network.Messages.Lobby
{
    public class ChatContentMessage:Message
    {
        public string Content { get; set; }

        public override ushort MessageId
        {
            get { return Protocol.CHAT_CONTENT; }
        }

        public override bool Decode(PacketReader reader)
        {
            Content = reader.ReadString();
            return true;
        }

        protected override bool EncodeContent(PacketWriter writer)
        {
            writer.Write(Content);
            return true;
        }
    }
}
