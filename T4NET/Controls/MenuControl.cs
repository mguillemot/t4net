using Microsoft.Xna.Framework;
using T4NET.Menus;

namespace T4NET.Controls
{
    public class MenuControl
    {
        private readonly Menu m_menu;

        public MenuControl(Menu menu)
        {
            m_menu = menu;
        }

        public void Update(GameTime gameTime, IControlsProvider controlsProvider)
        {
            if (m_menu.Active)
            {
                if (controlsProvider.CurrentConfig.JustPressed(Function.MENU_DOWN, controlsProvider.CurrentState))
                {
                    m_menu.Next();
                }
                else if (controlsProvider.CurrentConfig.JustPressed(Function.MENU_UP, controlsProvider.CurrentState))
                {
                    m_menu.Previous();
                }
                else if (controlsProvider.CurrentConfig.JustPressed(Function.MENU_VALIDATE, controlsProvider.CurrentState))
                {
                    m_menu.ActivateEntry();
                }
                else if (controlsProvider.CurrentConfig.JustPressed(Function.MENU_CANCEL, controlsProvider.CurrentState))
                {
                    m_menu.CloseMenu();
                }
            }
        }
    }
}
