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
        private Piece m_currentPiece;

        public Board(int hSize, int vSize)
        {
            m_hSize = hSize;
            m_vSize = vSize + 1;
            m_board = new int[hSize][];
            for (int i = 0; i < m_hSize; i++)
            {
                m_board[i] = new int[m_vSize];
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

        public Piece CurrentPiece
        {
            get { return m_currentPiece; }
            set { m_currentPiece = value; }
        }

        public bool CanMoveDown()
        {
            m_currentPiece.Shift(0, 1);
            bool result = ValidPosition();
            m_currentPiece.Shift(0, -1);
            return result;
        }

        public bool CanMoveLeft()
        {
            m_currentPiece.Shift(-1, 0);
            bool result = ValidPosition();
            m_currentPiece.Shift(1, 0);
            return result;
        }

        public bool CanMoveRight()
        {
            m_currentPiece.Shift(1, 0);
            bool result = ValidPosition();
            m_currentPiece.Shift(-1, 0);
            return result;
        }

        public void RotateRight()
        {
            m_currentPiece.RotateR();
            if (ValidPosition()) return;
            int x = m_currentPiece.X;
            int y = m_currentPiece.Y;
            foreach (Point alt in ALTERNATIVE_POSITIONS)
            {
                m_currentPiece.X = x + alt.X;
                m_currentPiece.Y = y + alt.Y;
                if (ValidPosition())
                {
                    return;
                }
            }
            m_currentPiece.RotateL();
            m_currentPiece.X = x;
            m_currentPiece.Y = y;
        }

        public void RotateLeft()
        {
            m_currentPiece.RotateL();
            if (ValidPosition()) return;
            int x = m_currentPiece.X;
            int y = m_currentPiece.Y;
            foreach (Point alt in ALTERNATIVE_POSITIONS)
            {
                m_currentPiece.X = x + alt.X;
                m_currentPiece.Y = y + alt.Y;
                if (ValidPosition())
                {
                    return;
                }
            }
            m_currentPiece.RotateR();
            m_currentPiece.X = x;
            m_currentPiece.Y = y;
        }

        public int InstantDrop()
        {
            int initialPosition = m_currentPiece.Y;
            while (ValidPosition())
            {
                m_currentPiece.Shift(0, 1);
            }
            m_currentPiece.Shift(0, -1);
            return m_currentPiece.Y - initialPosition;
        }

        public bool ValidPosition()
        {
            foreach (Point block in m_currentPiece.CurrentBlocks)
            {
                int x = m_currentPiece.X + block.X;
                int y = m_currentPiece.Y + block.Y;
                if (x < 0 || x >= m_hSize || y < 0 || y >= m_vSize || m_board[x][y] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public void Incorporate()
        {
            foreach (Point block in m_currentPiece.CurrentBlocks)
            {
                m_board[m_currentPiece.X + block.X][m_currentPiece.Y + block.Y] = m_currentPiece.Color;
            }
        }

        public bool CheckLines()
        {
            bool lineFound = false;
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
                    lineFound = true;
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
            return lineFound;
        }
    }
}