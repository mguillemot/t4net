using System;
using System.Collections.Generic;

namespace T4NET.Menus
{
    public class MenuEntrySelectedEventArgs : EventArgs
    {
        private readonly Menu m_menu;

        public MenuEntrySelectedEventArgs(Menu menu)
        {
            m_menu = menu;
        }

        public Menu Menu
        {
            get { return m_menu; }
        }
    }

    public class Menu
    {
        private readonly List<MenuEntry> m_entries = new List<MenuEntry>();
        private int m_selectedEntry;

        public event EventHandler<MenuEntrySelectedEventArgs> OnEntryChange;

        public List<MenuEntry> Entries
        {
            get { return m_entries; }
        }

        public MenuEntry SelectedEntry
        {
            get { return m_entries[m_selectedEntry]; }
        }

        public void AddEntry(MenuEntry entry)
        {
            m_entries.Add(entry);
        }

        public void Next()
        {
            m_selectedEntry = (m_selectedEntry + 1)%m_entries.Count;
            if (OnEntryChange != null)
            {
                OnEntryChange(this, new MenuEntrySelectedEventArgs(this));
            }
        }

        public void Previous()
        {
            m_selectedEntry = (m_selectedEntry + m_entries.Count - 1)%m_entries.Count;
            if (OnEntryChange != null)
            {
                OnEntryChange(this, new MenuEntrySelectedEventArgs(this));
            }
        }
    }
}
