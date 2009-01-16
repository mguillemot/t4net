using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using T4NET.Graphic;
using T4NET.Menus;

namespace T4NET.Screens
{
    public class TitleScreen : Screen
    {
        private Menu m_menu;
        private MenuDisplay m_menuDisplay;

        public override void Update(GameTime time, GameServiceContainer services)
        {
            m_menuDisplay.Update(time, services);
        }

        public override void Draw()
        {
            m_menuDisplay.Draw();
        }

        public override void Initialize(GraphicsDevice device)
        {
            base.Initialize(device);

            m_menu = new Menu();
            m_menu.AddEntry(new MenuEntry{ Title = "Rien", NextScreen = this});
            m_menu.AddEntry(new MenuEntry { Title = "Rien non plus", NextScreen = this });
            m_menuDisplay = new MenuDisplay(m_menu);
            m_menuDisplay.Initialize(device);
        }
    }
}
