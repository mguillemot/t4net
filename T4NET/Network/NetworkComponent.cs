using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using T4NET.Controls;
using T4NET.Network.Messages.Lobby;
using T4NET.Network.Messages.Session;

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
                    UnregisterNetworkSession();
                }
                if (Gamer.SignedInGamers.Count == 0)
                {
                    Console.WriteLine("ERROR: impossible to create network session without local gamers");
                }
                else
                {
                    try
                    {
                        NetworkSession session = NetworkSession.Create(NetworkSessionType.SystemLink,
                                                                       4,
                                                                       16,
                                                                       0,
                                                                       new NetworkSessionProperties());
                        Console.WriteLine("Session " + session.Host.Gamertag + " created");
                        session.AllowJoinInProgress = true;
                        session.AllowHostMigration = true;
                        registerNetworkSession(session);
                    }
                    catch (Exception e)
                    {
                        Dispatch(new SessionCreateErrorMessage());
                        Console.WriteLine("ERROR: " + e.GetType().Name + " raised while creating session.");
                    }
                }
            }
            if (controlsProvider.CurrentConfig.JustPressed(Function.DEBUG_JOIN_SESSION, controlsProvider.CurrentState))
            {
                if (m_networkSession != null)
                {
                    UnregisterNetworkSession();
                }
                AvailableNetworkSessionCollection sessions = NetworkSession.Find(NetworkSessionType.SystemLink,
                                                                                 Gamer.SignedInGamers,
                                                                                 new NetworkSessionProperties());
                Console.WriteLine("Enumerate sessions:");
                foreach (AvailableNetworkSession session in sessions)
                {
                    Console.WriteLine("> " + session.HostGamertag + " (" + session.CurrentGamerCount + " players)");
                    Console.WriteLine("~");
                }
                if (sessions.Count > 0)
                {
                    try
                    {
                        var session = NetworkSession.Join(sessions[0]);
                        registerNetworkSession(session);
                        Console.WriteLine("Joined session " + session.Host.Gamertag);
                        Dispatch(new SessionJoinedMessage
                                     {
                                         Session = session
                                     });
                    } 
                    catch (NetworkSessionJoinException e)
                    {
                        Dispatch(new SessionJoinErrorMessage
                                     {
                                         JoinError = e.JoinError
                                     });
                        Console.WriteLine("ERROR: network join error " + e.JoinError);
                    }
                    catch (Exception e)
                    {
                        Dispatch(new SessionJoinErrorMessage
                        {
                            JoinError = NetworkSessionJoinError.SessionNotJoinable
                        });
                        Console.WriteLine("ERROR: " + e.GetType().Name + " raised while joining session.");
                    }
                }
            }
            if (m_networkSession != null &&
                controlsProvider.CurrentConfig.JustPressed(Function.GAME_BONUS_SELF, controlsProvider.CurrentState))
            {
                var chat = new ChatContentNetMessage {Content = "coucou!!!"};
                var writer = new PacketWriter();
                chat.Encode(writer);
                m_networkSession.LocalGamers[0].SendData(writer, SendDataOptions.None);
                    // TODO gérer plusieurs local gamers
                Console.WriteLine("Sent to all {0} remote gamers in the session!", m_networkSession.RemoteGamers.Count);
            }

            if (m_networkSession != null && m_networkSession.LocalGamers.Count == 1 &&
                GamePad.GetState(PlayerIndex.Two).IsButtonDown(Buttons.RightStick))
            {
                foreach (SignedInGamer signedInGamer in Gamer.SignedInGamers)
                {
                    if (signedInGamer.PlayerIndex == PlayerIndex.Two)
                    {
                        m_networkSession.AddLocalGamer(signedInGamer);
                    }
                }
            }

            if (m_networkSession != null)
            {
                m_networkSession.Update();
                foreach (LocalNetworkGamer gamer in m_networkSession.LocalGamers)
                {
                    if (gamer.IsDataAvailable)
                    {
                        NetworkMessage message = m_decoder.Decode(gamer);
                        if (message != null)
                        {
                            Dispatch(message);
                        }
                    }
                }
            }
        }

        private void UnregisterNetworkSession()
        {
            string host = m_networkSession.Host.Gamertag;
            m_networkSession.GamerJoined -= OnGamerJoined;
            m_networkSession.GamerLeft -= OnGamerLeft;
            m_networkSession.GameStarted -= OnGameStarted;
            m_networkSession.GameEnded -= OnGameEnded;
            m_networkSession.HostChanged -= OnHostChanged;
            m_networkSession.SessionEnded -= OnSessionEnded;
            m_networkSession.Dispose();
            Console.WriteLine("Left session " + host);
            m_networkSession = null;
        }

        private void registerNetworkSession(NetworkSession session)
        {
            m_networkSession = session;
            m_networkSession.GamerJoined += OnGamerJoined;
            m_networkSession.GamerLeft += OnGamerLeft;
            m_networkSession.GameStarted += OnGameStarted;
            m_networkSession.GameEnded += OnGameEnded;
            m_networkSession.HostChanged += OnHostChanged;
            m_networkSession.SessionEnded += OnSessionEnded;
        }

        private void OnSessionEnded(object sender, NetworkSessionEndedEventArgs e)
        {
            Dispatch(new SessionEndedMessage
                         {
                             Session = m_networkSession,
                             EndReason = e.EndReason
                         });
        }

        private static void OnHostChanged(object sender, HostChangedEventArgs e)
        {
            Console.WriteLine("Host changed to " + e.NewHost.Gamertag);
        }

        private static void OnGameEnded(object sender, GameEndedEventArgs e)
        {
            Console.WriteLine("Game ended");
        }

        private static void OnGameStarted(object sender, GameStartedEventArgs e)
        {
            Console.WriteLine("Game started");
        }

        private void OnGamerLeft(object sender, GamerLeftEventArgs e)
        {
            Dispatch(new PlayerLeftSessionMessage
                         {
                             Gamer = e.Gamer
                         });
        }

        private void OnGamerJoined(object sender, GamerJoinedEventArgs e)
        {
            Dispatch(new PlayerJoinedSessionMessage
                         {
                             Gamer = e.Gamer
                         });
        }

        private void Dispatch(Message message)
        {
            var dispatcher = (IMessageDispatcher) Game.Services.GetService(typeof (IMessageDispatcher));
            dispatcher.DispatchMessage(message);
        }
    }
}