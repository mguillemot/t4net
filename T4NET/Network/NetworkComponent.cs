using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using T4NET.Controls;

namespace T4NET.Network
{
    public class NetworkComponent : GameComponent
    {
        private bool m_active;
        private NetworkSession m_networkSession;

        public NetworkComponent(Game game) 
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            m_active = ((T4Net) Game).HasGamerService;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var controlsProvider = (IControlsProvider) Game.Services.GetService(typeof (IControlsProvider));
            if (m_active && m_networkSession == null && (controlsProvider.CurrentState.PressedKeys.Contains(Keys.F10) || controlsProvider.CurrentState.PressedButtons.Contains(Buttons.Start)))
            {
                m_networkSession = NetworkSession.Create(NetworkSessionType.SystemLink, 1, 8, 2, new NetworkSessionProperties());
                Console.WriteLine("Session " + m_networkSession.Host.Gamertag + " created");
                m_networkSession.AllowJoinInProgress = true;
                m_networkSession.GamerJoined += OnGamerJoined;
            }
            if (m_active && (controlsProvider.CurrentState.PressedKeys.Contains(Keys.F11) || controlsProvider.CurrentState.PressedButtons.Contains(Buttons.LeftShoulder)))
            {
                var sessions = NetworkSession.Find(NetworkSessionType.SystemLink, 1, new NetworkSessionProperties());
                foreach (var session in sessions)
                {
                    Console.WriteLine("======= " + session.HostGamertag + " has " + session.CurrentGamerCount);
                }
                Console.WriteLine("Enumerate sessions done");
            }

            if (m_networkSession != null)
            {
                m_networkSession.Update();
                System.Console.WriteLine(m_networkSession.AllGamers.Count);
            }
        }

        private void OnGamerJoined(object sender, GamerJoinedEventArgs e)
        {
            Console.WriteLine(e.Gamer.Gamertag + " joined");
        }
    }
}