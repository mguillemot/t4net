using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Net;

namespace T4NET.ZeGame
{
    public class Player
    {
        private readonly GameSession m_session;
        private readonly byte m_id;
        private readonly Board m_board = new Board(Board.DEFAULT_WIDTH, Board.DEFAULT_HEIGHT);

        public Player(GameSession session, byte id)
        {
            m_session = session;
            m_id = id;
        }

        public byte Id
        {
            get { return m_id; }
        }

        public Team Team { get; internal set; }

        public NetworkGamer Gamer
        {
            get
            {
                foreach (var gamer in m_session.NetworkSession.AllGamers)
                {
                    if (gamer.Id == m_id)
                    {
                        return gamer;
                    }
                }
                return null;
            }
        }

        public static bool Serialize(PacketWriter writer, Player player)
        {
            writer.Write(player.Id);
            return true;
        }

        public static Player Unserialize(PacketReader reader, GameSession session)
        {
            var id = reader.ReadByte();
            var player = new Player(session, id);
            var team = (Team) reader.ReadByte();
            player.Team = team;
            return player;
        }
    }
}
