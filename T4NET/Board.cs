using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace T4NET
{
    public class Board
    {
        private static readonly List<Point> ALTERNATIVE_POSITIONS = new List<Point>
                                           {
                                               new Point(-1, 0),
                                               new Point(1, 0),
                                               new Point(-2, 0),
                                               new Point(2, 0),
                                               new Point(0, -1),
                                               new Point(-1, -1),
                                               new Point(1, -1)
                                           };

        private readonly int[][] m_board;
        private readonly int m_hSize;
        private readonly int m_vSize;

        public Board(int hSize, int vSize)
        {
            m_hSize = hSize;
            m_vSize = vSize;
            m_board = new int[hSize][];
            for (int i = 0; i < hSize; i++)
            {
                m_board[i] = new int[vSize];
            }
        }

        public int VSize
        {
            get { return m_vSize; }
        }

        public int HSize
        {
            get { return m_hSize; }
        }

        public int[][] Content
        {
            get { return m_board; }
        }

        public bool CanMoveDown(Piece p)
        {
            p.Y = p.Y + 1;
            bool result = ValidPosition(p);
            p.Y = p.Y - 1;
            return result;
        }

        public bool CanMoveLeft(Piece p)
        {
            p.X = p.X - 1;
            bool result = ValidPosition(p);
            p.X = p.X + 1;
            return result;
        }

        public bool CanMoveRight(Piece p)
        {
            p.X = p.X + 1;
            bool result = ValidPosition(p);
            p.X = p.X - 1;
            return result;
        }

        public void RotateRight(Piece p)
        {
            p.RotateR();
            if (ValidPosition(p)) return;
            int x = p.X;
            int y = p.Y;
            foreach (var alt in ALTERNATIVE_POSITIONS)
            {
                p.X = x + alt.X;
                p.Y = y + alt.Y;
                if (ValidPosition(p))
                {
                    return;
                }
            }
            p.RotateL();
            p.X = x;
            p.Y = y;
        }

        public void RotateLeft(Piece p)
        {
            p.RotateL();
            if (ValidPosition(p)) return;
            int x = p.X;
            int y = p.Y;
            foreach (var alt in ALTERNATIVE_POSITIONS)
            {
                p.X = x + alt.X;
                p.Y = y + alt.Y;
                if (ValidPosition(p))
                {
                    return;
                }
            }
            p.RotateR();
            p.X = x;
            p.Y = y;

        }

        public bool ValidPosition(Piece p)
        {
            foreach (var block in p.CurrentBlocks)
            {
                int x = p.X + block.X;
                int y = p.Y + block.Y;
                if (x < 0 || x >= m_hSize || y < 0 || y >= m_vSize || m_board[x][y] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void Incorporate(Piece p)
        {
            foreach (var block in p.CurrentBlocks)
            {
                m_board[p.X + block.X][p.Y + block.Y] = 1;
            }
            CheckLines();
        }

        public void CheckLines()
        {
            for (int j = m_vSize - 1; j >= 0; j--)
            {
                bool complete = true;
                for (int i = 0; i < m_hSize; i++)
                {
                    if (m_board[i][j] == 0)
                    {
                        complete = false;
                        break;
                    }
                }
                if (complete)
                {
                    // Collapse
                    if (j > 0)
                    {
                        for (int jj = j; jj > 0; jj--)
                        {
                            for (int i = 0; i < m_hSize; i++)
                            {
                                m_board[i][jj] = m_board[i][jj - 1];
                            }
                        }
                    }
                    for (int i = 0; i < m_hSize; i++)
                    {
                        m_board[i][0] = 0;
                    }
                    j++; // rechecks same line
                }
            }
        }
    }
}