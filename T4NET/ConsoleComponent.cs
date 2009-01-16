using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using T4NET.Controls;
using T4NET.Graphic;

namespace T4NET
{
    public class ConsoleComponent : DrawableGameComponent
    {
        private readonly ConsoleDisplay m_consoleDisplay = new ConsoleDisplay();
        private bool m_consoleVisible = true;

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
            if (controlsProvider.CurrentState.PressedKeys.Contains(Keys.Tab))
            {
                m_consoleVisible = !m_consoleVisible;
            }
        }
    }
}
