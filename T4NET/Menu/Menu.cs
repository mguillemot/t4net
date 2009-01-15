using System;
using System.Collections.Generic;

namespace T4NET.Menu
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
        private readonly List<Entry> m_entries = new List<Entry>();
        private int m_selectedEntry;

        public event EventHandler<MenuEntrySelectedEventArgs> OnEntryChange;

        public List<Entry> Entries
        {
            get { return m_entries; }
        }

        public Entry SelectedEntry
        {
            get { return m_entries[m_selectedEntry]; }
        }

        public void AddEntry(Entry entry)
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

    public class Entry
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
