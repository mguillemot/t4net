using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;
using T4NET.Controls;
using T4NET.Network.Messages.Lobby;

namespace T4NET.Network
{
    public class NetworkComponent : GameComponent
    {
        private readonly NetworkMessageDecoder m_decoder = new NetworkMessageDecoder();
        private NetworkSession m_networkSession;

        public NetworkComponent(Game game)
            : base(game)
        {
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var controlsProvider = (IControlsProvider) Game.Services.GetService(typeof (IControlsProvider));
            if (controlsProvider.CurrentConfig.JustPressed(Function.DEBUG_CREATE_SESSION, controlsProvider.CurrentState))
            {
                if (m_networkSession != null)
                {
                    m_networkSession.Dispose();
                    Console.WriteLine("Left session " + m_networkSession.Host.Gamertag);
                    m_networkSession = null;
                }
                m_networkSession = NetworkSession.Create(NetworkSessionType.SystemLink, 1, 8, 2,
                                                         new NetworkSessionProperties());
                Console.WriteLine("Session " + m_networkSession.Host.Gamertag + " created");
                m_networkSession.AllowJoinInProgress = true;
                m_networkSession.GamerJoined += OnGamerJoined;
            }
            if (controlsProvider.CurrentConfig.JustPressed(Function.DEBUG_JOIN_SESSION, controlsProvider.CurrentState))
            {
                if (m_networkSession != null)
                {
                    var host = m_networkSession.Host.Gamertag;
                    m_networkSession.Dispose();
                    Console.WriteLine("Left session " + host);
                    m_networkSession = null;
                }
                AvailableNetworkSessionCollection sessions = NetworkSession.Find(NetworkSessionType.SystemLink, 1,
                                                                                 new NetworkSessionProperties());
                Console.WriteLine("Enumerate sessions:");
                foreach (AvailableNetworkSession session in sessions)
                {
                    Console.WriteLine("> " + session.HostGamertag + " (" + session.CurrentGamerCount + " players)");
                    Console.WriteLine("~");
                }
                if (sessions.Count > 0)
                {
                    m_networkSession = NetworkSession.Join(sessions[0]);
                    Console.WriteLine("Joined session " + m_networkSession.Host.Gamertag);
                }
            }
            if (m_networkSession != null &&
                controlsProvider.CurrentConfig.JustPressed(Function.GAME_BONUS_SELF, controlsProvider.CurrentState))
            {
                var chat = new ChatContentMessage {Content = "coucou!!!"};
                var writer = new PacketWriter();
                chat.Encode(writer);
                m_networkSession.LocalGamers[0].SendData(writer, SendDataOptions.None); // TODO gérer plusieurs local gamers
                Console.WriteLine("Sent to all {0} remote gamers in the session!", m_networkSession.RemoteGamers.Count);
            }

            if (m_networkSession != null)
            {
                var messageDispatcher = (IMessageDispatcher) Game.Services.GetService(typeof (IMessageDispatcher));
                m_networkSession.Update();
                foreach (LocalNetworkGamer gamer in m_networkSession.LocalGamers)
                {
                    if (gamer.IsDataAvailable)
                    {
                        NetworkMessage message = m_decoder.Decode(gamer);
                        if (message != null)
                        {
                            messageDispatcher.DispatchMessage(message);
                        }
                    }
                }
            }
        }

        private static void OnGamerJoined(object sender, GamerJoinedEventArgs e)
        {
            Console.WriteLine(e.Gamer.Gamertag + " joined my session");
        }
    }
}