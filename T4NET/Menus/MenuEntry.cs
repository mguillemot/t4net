using System;

namespace T4NET.Menus
{
    public class MenuEntry
    {
        public event EventHandler EntryActivated;

        public string Title { get; set; }

        internal void Activated()
        {
            if (EntryActivated != null)
            {
                EntryActivated(this, null);
            }
        }
    }
}