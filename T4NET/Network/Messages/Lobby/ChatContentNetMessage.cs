using Microsoft.Xna.Framework.Net;

namespace T4NET.Network.Messages.Lobby
{
    public class ChatContentNetMessage : NetworkMessage
    {
        public string Content { get; set; }

        public override Protocol MessageId
        {
            get { return Protocol.NET_CHAT_CONTENT; }
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
