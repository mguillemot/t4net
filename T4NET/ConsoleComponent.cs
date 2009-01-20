using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using T4NET.Controls;
using T4NET.Graphic;
using T4NET.Network;
using T4NET.Network.Messages.Lobby;

namespace T4NET
{
    public class ConsoleComponent : DrawableGameComponent, IMessageProcessor
    {
        private readonly ConsoleDisplay m_consoleDisplay = new ConsoleDisplay();
        private bool m_consoleVisible = false;

        public ConsoleComponent(Game game) 
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            ConsoleDisplay.LoadContent(Game.Content);
        }

        public override void Initialize()
        {
            base.Initialize();
            m_consoleDisplay.Initialize(GraphicsDevice);
            Console.LineWidth = m_consoleDisplay.CharacterWidth;
            var messageDispatcher = (IMessageDispatcher) Game.Services.GetService(typeof (IMessageDispatcher));
            messageDispatcher.RegisterProcessor(Protocol.CHAT_CONTENT, this);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (m_consoleVisible)
            {
                m_consoleDisplay.Draw();
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var controlsProvider = (IControlsProvider) Game.Services.GetService(typeof (IControlsProvider));
            if (controlsProvider.CurrentConfig.JustPressed(Function.DEBUG_CONSOLE, controlsProvider.CurrentState))
            {
                m_consoleVisible = !m_consoleVisible;
            }
        }

        public bool OnMessage(Message message)
        {
            switch (message.MessageId)
            {
                case Protocol.CHAT_CONTENT:
                    var msg = (ChatContentMessage)message;
                    Console.WriteLine("Chat> " + msg.Content);
                    return false;
            }
            return true;
        }
    }
}
