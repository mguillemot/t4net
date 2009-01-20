using System;

namespace T4NET.Leaderboards
{
    [Serializable]
    public struct LeaderboardEntry
    {
        public string GamerTag;
        public ulong Experience;
        public long Date;
        public ushort Distance;
    }
}