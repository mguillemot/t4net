using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace T4NET
{
    public class Board : ICloneable
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

        private static readonly Random s_random = new Random();

        private readonly Block[][] m_board;
        private readonly Stack<Block> m_collectedBonuses = new Stack<Block>();
        private readonly int m_hSize;
        private readonly int m_vSize;
        private Piece m_currentPiece;
        private Piece m_nextPiece;

        public Board(int hSize, int vSize)
        {
            m_hSize = hSize;
            m_vSize = vSize + 1;
            m_board = new Block[hSize][];
            for (int i = 0; i < m_hSize; i++)
            {
                m_board[i] = new Block[m_vSize];
                for (int j = 0; j < m_vSize; j++)
                {
                    m_board[i][j] = Block.EMPTY;
                }
            }
            m_nextPiece = Piece.RandomPiece();
        }

        public object Clone()
        {
            var copy = new Board(m_hSize, m_vSize)
                           {
                               m_currentPiece = ((Piece) m_currentPiece.Clone()),
                               m_nextPiece = ((Piece) m_nextPiece.Clone())
                           };
            for (int i = 0; i < m_hSize; i++)
            {
                for (int j = 0; j < m_vSize; j++)
                {
                    copy.m_board[i][j] = m_board[i][j];
                }
            }
            // TODO collected bonuses
            return copy;
        }

        public int VSize
        {
            get { return m_vSize; }
        }

        public int HSize
        {
            get { return m_hSize; }
        }

        public Block[][] Content
        {
            get { return m_board; }
        }

        public Piece CurrentPiece
        {
            get { return m_currentPiece; }
        }

        public Piece NextPiece
        {
            get { return m_nextPiece; }
        }

        public Stack<Block> CollectedBonuses
        {
            get { return m_collectedBonuses; }
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

        public void SwitchToNextPiece()
        {
            m_currentPiece = m_nextPiece;
            m_nextPiece = Piece.RandomPiece();
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

        public List<int> CheckCompleteLines()
        {
            var result = new List<int>();
            for (int j = m_vSize - 1; j >= 0; j--)
            {
                bool complete = true;
                for (int i = 0; i < m_hSize; i++)
                {
                    if (m_board[i][j] == Block.EMPTY)
                    {
                        complete = false;
                        break;
                    }
                }
                if (complete)
                {
                    result.Add(j);
                }
            }
            return result;
        }

        public void DeleteCompleteLines()
        {
            for (int j = m_vSize - 1; j >= 0; j--)
            {
                bool complete = true;
                for (int i = 0; i < m_hSize; i++)
                {
                    if (m_board[i][j] == Block.EMPTY)
                    {
                        complete = false;
                        break;
                    }
                }
                if (complete)
                {
                    for (int i = 0; i < m_hSize; i++)
                    {
                        if (Blocks.SPECIAL_BLOCKS.Contains(m_board[i][j]))
                        {
                            m_collectedBonuses.Push(m_board[i][j]);
                        }
                    }
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
                        m_board[i][0] = Block.EMPTY;
                    }
                    j++; // rechecks same line
                }
            }
        }

        public void TransformRandomBlocksToSpecial(int nBlocks, List<int> excludedLines)
        {
            var allStandardBlocks = new List<Point>();
            for (int i = 0; i < m_hSize; i++)
            {
                for (int j = 0; j < m_vSize; j++)
                {
                    if (m_board[i][j] != Block.EMPTY)
                    {
                        allStandardBlocks.Add(new Point(i, j));
                    }
                }
            }
            while (nBlocks > 0 && allStandardBlocks.Count > 0)
            {
                int selectedBlockIndex = s_random.Next(allStandardBlocks.Count);
                Point selectedBlock = allStandardBlocks[selectedBlockIndex];
                allStandardBlocks.RemoveAt(selectedBlockIndex);
                if (excludedLines.Contains(selectedBlock.Y))
                {
                    continue;
                }
                Block blockType = Blocks.SPECIAL_BLOCKS[s_random.Next(Blocks.SPECIAL_BLOCKS.Count)];
                m_board[selectedBlock.X][selectedBlock.Y] = blockType;
                nBlocks--;
            }
        }
    }
}