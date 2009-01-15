using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace T4NET.Menu
{
    public abstract class Screen
    {
        private GraphicsDevice m_device;

        protected GraphicsDevice GraphicsDevice
        {
            get { return m_device; }
        }

        public virtual void Initialize(GraphicsDevice device)
        {
            m_device = device;
        }

        public abstract void Update(GameTime time);

        public abstract void Draw();
    }
}
