using T4NET.Screens;

namespace T4NET.Menus
{
    public class MenuEntry
    {
        private string m_title;
        private Screen m_nextScreen;

        public string Title
        {
            get { return m_title; }
            set { m_title = value; }
        }

        public Screen NextScreen
        {
            get { return m_nextScreen; }
            set { m_nextScreen = value; }
        }
    }
}