using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace T4NET.Graphic
{
    public class BlockFont
    {
        private readonly Dictionary<char, List<Point>> m_matrixes = new Dictionary<char, List<Point>>();

        public BlockFont()
        {
            m_matrixes['A'] = new List<Point>
                                  {
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['B'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4)
                                  };
            m_matrixes['C'] = new List<Point>
                                  {
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(1, 4),
                                      new Point(2, 4)
                                  };
            m_matrixes['D'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4)
                                  };
            m_matrixes['E'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(0, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['F'] = new List<Point>
                                  {
                                       new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(0, 3),
                                      new Point(0, 4)
                                  };
            m_matrixes['G'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(0, 2),
                                      new Point(2, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['H'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['I'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(1, 1),
                                      new Point(1, 2),
                                      new Point(1, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4)
                                  };
            m_matrixes['J'] = new List<Point>
                                  {
                                      new Point(2, 0),
                                      new Point(2, 1),
                                      new Point(2, 2),
                                      new Point(0, 3),
                                      new Point(2, 3),
                                      new Point(1, 4)
                                  };
            m_matrixes['K'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(2, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(0, 3),
                                      new Point(2, 3),
                                      new Point(0, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['L'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(0, 1),
                                      new Point(0, 2),
                                      new Point(0, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['M'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(4, 0),
                                      new Point(0, 1),
                                      new Point(1, 1),
                                      new Point(3, 1),
                                      new Point(4, 1),
                                      new Point(0, 2),
                                      new Point(2, 2),
                                      new Point(4, 2),
                                      new Point(0, 3),
                                      new Point(4, 3),
                                      new Point(0, 4),
                                      new Point(4, 4)
                                  };
            m_matrixes['N'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(4, 0),
                                      new Point(0, 1),
                                      new Point(1, 1),
                                      new Point(4, 1),
                                      new Point(0, 2),
                                      new Point(2, 2),
                                      new Point(4, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(4, 3),
                                      new Point(0, 4),
                                      new Point(4, 4)
                                  };
            m_matrixes['O'] = new List<Point>
                                  {
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(1, 4),
                                      new Point(2, 4)
                                  };
            m_matrixes['P'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(0, 3),
                                      new Point(0, 4)
                                  };
            m_matrixes['Q'] = new List<Point>
                                  {
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(2, 3),
                                      new Point(3, 3),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                      new Point(3, 4),
                                      new Point(4, 4)
                                  };
            m_matrixes['R'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(0, 3),
                                      new Point(2, 3),
                                      new Point(0, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['S'] = new List<Point>
                                  {
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4)
                                  };
            m_matrixes['T'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(4, 0),
                                      new Point(2, 1),
                                      new Point(2, 2),
                                      new Point(2, 3),
                                      new Point(2, 4)
                                  };
            m_matrixes['U'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(1, 4),
                                      new Point(2, 4)
                                  };
            m_matrixes['V'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(4, 0),
                                      new Point(0, 1),
                                      new Point(4, 1),
                                      new Point(0, 2),
                                      new Point(4, 2),
                                      new Point(1, 3),
                                      new Point(3, 3),
                                      new Point(2, 4)
                                  };
            m_matrixes['W'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(4, 0),
                                      new Point(0, 1),
                                      new Point(4, 1),
                                      new Point(0, 2),
                                      new Point(2, 2),
                                      new Point(4, 2),
                                      new Point(0, 3),
                                      new Point(2, 3),
                                      new Point(4, 3),
                                      new Point(1, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['X'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(4, 0),
                                      new Point(1, 1),
                                      new Point(3, 1),
                                      new Point(2, 2),
                                      new Point(1, 3),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(4, 4)
                                  };
            m_matrixes['Y'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(4, 0),
                                      new Point(1, 1),
                                      new Point(3, 1),
                                      new Point(2, 2),
                                      new Point(2, 3),
                                      new Point(2, 4)
                                  };
            m_matrixes['Z'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(4, 0),
                                      new Point(3, 1),
                                      new Point(2, 2),
                                      new Point(1, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                      new Point(3, 4),
                                      new Point(4, 4)
                                  };
            m_matrixes['0'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['1'] = new List<Point>
                                  {
                                      new Point(1, 0),
                                      new Point(0, 1),
                                      new Point(1, 1),
                                      new Point(1, 2),
                                      new Point(1, 3),
                                      new Point(1, 4)
                                  };
            m_matrixes['2'] = new List<Point>
                                  {
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(2, 2),
                                      new Point(1, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['3'] = new List<Point>
                                  {
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(2, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(1, 4),
                                      new Point(2, 4)
                                  };
            m_matrixes['4'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(0, 1),
                                      new Point(0, 2),
                                      new Point(2, 2),
                                      new Point(0, 3),
                                      new Point(1, 3),
                                      new Point(2, 3),
                                      new Point(3, 3),
                                      new Point(2, 4)
                                  };
            m_matrixes['5'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                  };
            m_matrixes['6'] = new List<Point>
                                  {
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4)
                                  };
            m_matrixes['7'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(2, 1),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(3, 2),
                                      new Point(2, 3),
                                      new Point(2, 4)
                                  };
            m_matrixes['8'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(3, 2),
                                      new Point(0, 3),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                      new Point(3, 4)
                                  };
            m_matrixes['9'] = new List<Point>
                                  {
                                      new Point(0, 0),
                                      new Point(1, 0),
                                      new Point(2, 0),
                                      new Point(3, 0),
                                      new Point(0, 1),
                                      new Point(3, 1),
                                      new Point(0, 2),
                                      new Point(1, 2),
                                      new Point(2, 2),
                                      new Point(3, 2),
                                      new Point(3, 3),
                                      new Point(0, 4),
                                      new Point(1, 4),
                                      new Point(2, 4),
                                      new Point(3, 4)
                                  };
        }

        public List<Point> GetMatrix(char c)
        {
            return m_matrixes[c];
        }
    }
}
