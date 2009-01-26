using System;
using System.Collections.Generic;

namespace T4NET.ZeGame
{
    public class PieceGenerator
    {
        private static readonly Random s_random = new Random();

        private readonly List<PieceType> m_bag = new List<PieceType>(PieceTypes.ALL.Length);

        public PieceType NextPiece()
        {
            if (m_bag.Count == 0)
            {
                m_bag.Add(PieceTypes.ALL[0]);
                for (int i = 1; i < PieceTypes.ALL.Length; i++)
                {
                    int index = s_random.Next(m_bag.Count + 1);
                    m_bag.Insert(index, PieceTypes.ALL[i]);
                }
            }
            var piece = m_bag[m_bag.Count - 1];
            m_bag.RemoveAt(m_bag.Count - 1);
            return piece;
        }
    }
}
