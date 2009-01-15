using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using T4NET.Graphic;

namespace T4NET.Menu.Screens
{
    public class TitleScreen : Screen
    {
        private Menu m_menu;
        private MenuDisplay m_menuDisplay;

        public override void Update(GameTime time)
        {
            m_menuDisplay.Update(time);
        }

        public override void Draw()
        {
            m_menuDisplay.Draw();
        }

        public override void Initialize(GraphicsDevice device)
        {
            base.Initialize(device);
            m_menu = new Menu();
            m_menu.AddEntry(new Entry{ Title = "Rien", NextScreen = this});
            m_menu.AddEntry(new Entry { Title = "Rien non plus", NextScreen = this });
            m_menuDisplay = new MenuDisplay(m_menu);
            m_menuDisplay.Initialize(device);
        }
    }
}
