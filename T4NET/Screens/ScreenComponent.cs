using Microsoft.Xna.Framework;

namespace T4NET.Screens
{
    public class ScreenComponent : DrawableGameComponent
    {
        private Screen m_screen;

        public ScreenComponent(Game game) 
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            if (m_screen != null)
            {
                m_screen.Update(gameTime, Game.Services);
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_screen = new GameScreen();
            m_screen.Initialize(GraphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            if (m_screen != null)
            {
                m_screen.Draw();
            }
        }
    }
}
