using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Storage;

namespace T4NET.Leaderboards
{
    [Serializable]
    public class Leaderboard
    {
        private readonly List<LeaderboardEntry> m_entries = new List<LeaderboardEntry>();

        public List<LeaderboardEntry> Entries
        {
            get { return m_entries; }
        }

        public void Save(StorageDevice device)
        {
            using (StorageContainer container = device.OpenContainer("T4NET"))
            {
                string fileName = Path.Combine(container.Path, "leaderboard.xml");
                FileStream file = File.Open(fileName, FileMode.OpenOrCreate);
                try
                {
                    var serializer = new XmlSerializer(typeof (Leaderboard));
                    serializer.Serialize(file, this);
                }
                finally
                {
                    file.Close();
                }
            }

            
        }
    }
}