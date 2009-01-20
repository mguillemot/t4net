using System;
using System.Collections.Generic;

namespace T4NET.Menus
{
    public class Menu
    {
        private readonly List<MenuEntry> m_entries = new List<MenuEntry>();
        private int m_selectedEntry;
        private bool m_active;

        public event EventHandler MenuClosed;

        public List<MenuEntry> Entries
        {
            get { return m_entries; }
        }

        public MenuEntry SelectedEntry
        {
            get { return m_entries[m_selectedEntry]; }
        }

        public bool Active
        {
            get { return m_active; }
            set { m_active = value; }
        }

        public void AddEntry(MenuEntry entry)
        {
            m_entries.Add(entry);
        }

        public void Next()
        {
            m_selectedEntry = (m_selectedEntry + 1)%m_entries.Count;
        }

        public void Previous()
        {
            m_selectedEntry = (m_selectedEntry + m_entries.Count - 1)%m_entries.Count;
        }

        public void ActivateEntry()
        {
            SelectedEntry.Activated();
        }

        public void CloseMenu()
        {
            if (MenuClosed != null)
            {
                MenuClosed(this, null);
            }
        }
    }
}
