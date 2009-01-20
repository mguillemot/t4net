using Microsoft.Xna.Framework.Net;
using T4NET.Network.Messages.Gameplay;
using T4NET.Network.Messages.Lobby;

namespace T4NET.Network
{
    public class NetworkMessageDecoder
    {
        private readonly PacketReader m_packetReader = new PacketReader();

        public NetworkMessage Decode(LocalNetworkGamer gamer)
        {
            NetworkGamer sender;
            gamer.ReceiveData(m_packetReader, out sender);
            if (m_packetReader.Length >= 2)
            {
                var messageType = (Protocol) m_packetReader.ReadUInt16();
                NetworkMessage msg = null;
                switch (messageType)
                {
                    // 1-999

                    // 1000-1999
                    case Protocol.CHAT_CONTENT:
                        msg = new ChatContentMessage();
                        break;

                    // 2000-2999
                    case Protocol.PLAYER_INCORPORATE_PIECE:
                        msg = new PlayerIncorporatePieceMessage();
                        break;
                }
                if (msg != null)
                {
                    if (msg.Decode(m_packetReader))
                    {
                        return msg;
                    }
                }
            }
            return null;
        }
    }
}