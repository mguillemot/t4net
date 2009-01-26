using System.Collections.Generic;
using Microsoft.Xna.Framework.Net;

namespace T4NET.ZeGame
{
    public class GameSession
    {
        private readonly Dictionary<byte, Player> m_playersById = new Dictionary<byte, Player>();
        private readonly Dictionary<Team, List<Player>> m_playersByTeam = new Dictionary<Team, List<Player>>();
        private NetworkSession m_networkSession;
        //private bool m_paused;

        public void InitializeLocalSession()
        {
            foreach (var team in ZeGame.Teams.ALL)
            {
                m_playersByTeam[team] = new List<Player>();
            }
        }

        public void LinkToNetworkSession(NetworkSession networkSession)
        {
            m_networkSession = networkSession;
        }

        public NetworkSession NetworkSession
        {
            get { return m_networkSession; }
        }

        public int MaxPlayers
        {
            get { return 16; }
        }

        public int MaxPlayersByTeam
        {
            get { return 4; }
        }

        public int MaxTeams
        {
            get { return 4; }
        }

        public void RemovePlayer(byte id)
        {
            var player = GetPlayer(id);
            m_playersById.Remove(id);
            m_playersByTeam[player.Team].Remove(player);
        }

        public ICollection<Player> Players
        {
            get { return m_playersById.Values; }
        }

        public ICollection<Team> Teams
        {
            get { return m_playersByTeam.Keys; }
        }

        public Player GetPlayer(byte id)
        {
            return m_playersById[id];
        }

        public List<Player> GetPlayers(Team team)
        {
            return m_playersByTeam[team];
        }

        public void PlayerChangeTeam(Player player, Team newTeam)
        {
            if (player.Team != newTeam)
            {
                List<Player> players = GetPlayers(player.Team);
                players.Remove(player);
                player.Team = newTeam;
                RegisterPlayer(player);
            }
        }

        public static bool Serialize(PacketWriter writer, GameSession session)
        {
            ICollection<Player> players = session.Players;
            writer.Write((byte) players.Count);
            foreach (Player player in players)
            {
                Player.Serialize(writer, player);
            }
            return true;
        }

        public static GameSession Unserialize(PacketReader reader)
        {
            var session = new GameSession();
            int nPlayers = reader.ReadByte();
            for (int i = 0; i < nPlayers; i++)
            {
                var player = Player.Unserialize(reader, session);
                if (player != null)
                {
                    session.RegisterPlayer(player);
                }
            }
            return session;
        }

        /*
        public void Pause()
        {
            if (!m_paused)
            {

                m_paused = true;
            }
        }

        public void Unpause()
        {
            if (m_paused)
            {

                m_paused = false;
            }
        }*/

        public void RegisterPlayer(Player player)
        {
            m_playersById[player.Id] = player;
            List<Player> players;
            if (!m_playersByTeam.TryGetValue(player.Team, out players))
            {
                players = new List<Player>();
                m_playersByTeam[player.Team] = players;
            }
            players.Add(player);
        }
    }
}