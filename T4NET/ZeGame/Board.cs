using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace T4NET.ZeGame
{
    public class PieceLockEventArgs : EventArgs
    {
        private readonly Board m_board;
        private readonly Piece m_piece;

        public PieceLockEventArgs(Board board, Piece piece)
        {
            m_board = board;
            m_piece = piece;
        }

        public Board Board
        {
            get { return m_board; }
        }

        public Piece Piece
        {
            get { return m_piece; }
        }
    }

    public class LineClearedEventArgs : EventArgs
    {
        private readonly Board m_board;
        private readonly List<int> m_lines;

        public LineClearedEventArgs(Board board, List<int> lines)
        {
            m_board = board;
            m_lines = lines;
        }

        public Board Board
        {
            get { return m_board; }
        }

        public List<int> Lines
        {
            get { return m_lines; }
        }
    }

    public class BonusAppearEventArgs : EventArgs
    {
        private readonly Board m_board;
        private readonly Block m_bonus;
        private readonly Point m_posision;

        public BonusAppearEventArgs(Board board, Point posision, Block bonus)
        {
            m_board = board;
            m_posision = posision;
            m_bonus = bonus;
        }

        public Board Board
        {
            get { return m_board; }
        }

        public Point Posision
        {
            get { return m_posision; }
        }

        public Block Bonus
        {
            get { return m_bonus; }
        }
    }

    public class Board : ICloneable
    {
        public const int DEFAULT_WIDTH = 10;
        public const int DEFAULT_HEIGHT = 20;
        public const int NEXT_PIECES = 3;
        
        private static readonly Random s_random = new Random();

        private readonly Block[][] m_board;
        private readonly Stack<Block> m_collectedBonuses = new Stack<Block>();
        private readonly int m_hSize;
        private readonly int m_vSize;
        private Piece m_currentPiece;
        private bool m_gameOver;
        private readonly Queue<Piece> m_nextPieces = new Queue<Piece>(NEXT_PIECES);
        private readonly PieceGenerator m_pieceGenerator = new PieceGenerator();

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
        }

        public void InitializeForLocalPlayer()
        {
            m_nextPieces.Clear();
            for (int i = 0; i < NEXT_PIECES; i++)
            {
                m_nextPieces.Enqueue(PieceFactory.CreateInitialPositionPiece(this, m_pieceGenerator.NextPiece()));
            }
            SwitchToNextPiece();
        }

        public bool IsGameOver
        {
            get { return m_gameOver; }
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

        public Queue<Piece> NextPieces
        {
            get { return m_nextPieces; }
        }

        public Stack<Block> CollectedBonuses
        {
            get { return m_collectedBonuses; }
        }

        public Block ActiveBonus
        {
            get { return m_collectedBonuses.Count > 0 ? m_collectedBonuses.Peek() : Block.EMPTY; }
        }

        public static bool Serialize(PacketWriter writer, Board board)
        {
            writer.Write((sbyte)board.HSize);
            writer.Write((sbyte)board.VSize);
            for (int j = 0; j < board.VSize; j++)
            {
                for (int i = 0; i < board.HSize; i++)
                {
                    writer.Write((byte)board.Content[i][j]);
                }
            }
            writer.Write((byte)board.CollectedBonuses.Count);
            foreach (var bonus in board.CollectedBonuses)
            {
                writer.Write((byte)bonus);
            }
            Piece.Serialize(writer, board.CurrentPiece);
            writer.Write((byte) board.NextPieces.Count);
            foreach (var piece in board.NextPieces)
            {
                Piece.Serialize(writer, piece);
            }
            return true;
        }

        public static Board Unserialize(PacketReader reader)
        {
            var width = reader.ReadSByte();
            var height = reader.ReadSByte();
            var result = new Board(width, height);
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    result.Content[i][j] = (Block)reader.ReadByte();
                }
            }
            var nSpecials = reader.ReadByte();
            for (int i = 0; i < nSpecials; i++)
            {
                result.CollectedBonuses.Push((Block)reader.ReadByte());
            }
            result.m_currentPiece = Piece.Unserialize(reader);
            int nextPiecesCount = reader.ReadByte();
            for (int i = 0; i < nextPiecesCount; i++)
            {
                result.NextPieces.Enqueue(Piece.Unserialize(reader));
            }
            return result;
        }

        #region ICloneable Members

        public object Clone()
        {
            var copy = new Board(m_hSize, m_vSize)
                           {
                               m_currentPiece = m_currentPiece != null ? (Piece) m_currentPiece.Clone() : null,
                           };
            foreach (var piece in m_nextPieces)
            {
                copy.NextPieces.Enqueue((Piece) piece.Clone());
            }
            for (int i = 0; i < m_hSize; i++)
            {
                for (int j = 0; j < m_vSize; j++)
                {
                    copy.m_board[i][j] = m_board[i][j];
                }
            }
            foreach (var block in m_collectedBonuses)
            {
                copy.CollectedBonuses.Push(block);
            }
            // TODO vérifier que les bonus sont dans l'ordre
            return copy;
        }

        #endregion

        public event EventHandler<PieceLockEventArgs> PieceLock;
        public event EventHandler<LineClearedEventArgs> LineCleared;
        public event EventHandler<BonusAppearEventArgs> BonusAppear;
        public event EventHandler GameOver;

        public void Loose()
        {
            if (!m_gameOver)
            {
                m_gameOver = true;
                m_currentPiece = null;
                for (int i = 0; i < m_hSize; i++)
                {
                    m_board[i][0] = Block.EMPTY;
                }
                for (int j = 1; j < m_vSize; j++)
                {
                    for (int i = 0; i < m_hSize; i++)
                    {
                        m_board[i][j] = Blocks.STANDARD_BLOCKS[s_random.Next(Blocks.STANDARD_BLOCKS.Count)];
                    }
                }
                if (GameOver != null)
                {
                    GameOver(this, null);
                }
            }
        }

        public bool CanMoveDown()
        {
            if (!m_gameOver)
            {
                m_currentPiece.Shift(0, 1);
                bool result = ValidPosition();
                m_currentPiece.Shift(0, -1);
                return result;
            }
            return false;
        }

        public bool CanMoveLeft()
        {
            if (!m_gameOver)
            {
                m_currentPiece.Shift(-1, 0);
                bool result = ValidPosition();
                m_currentPiece.Shift(1, 0);
                return result;
            }
            return false;
        }

        public bool CanMoveRight()
        {
            if (!m_gameOver)
            {
                m_currentPiece.Shift(1, 0);
                bool result = ValidPosition();
                m_currentPiece.Shift(-1, 0);
                return result;
            }
            return false;
        }

        public void SwitchToNextPiece()
        {
            if (!m_gameOver)
            {
                m_currentPiece = m_nextPieces.Count > 0 ? m_nextPieces.Dequeue() : null;
                m_nextPieces.Enqueue(PieceFactory.CreateInitialPositionPiece(this, m_pieceGenerator.NextPiece()));
            }
        }

        public void RotateRight()
        {
            if (!m_gameOver)
            {
                m_currentPiece.RotateR();
                if (ValidPosition()) return;
                int x = m_currentPiece.X;
                int y = m_currentPiece.Y;
                foreach (Point alt in m_currentPiece.RightWallKicks)
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
        }

        public void RotateLeft()
        {
            if (!m_gameOver)
            {
                m_currentPiece.RotateL();
                if (ValidPosition()) return;
                int x = m_currentPiece.X;
                int y = m_currentPiece.Y;
                foreach (Point alt in m_currentPiece.LeftWallKicks)
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
        }

        public int InstantDrop()
        {
            if (!m_gameOver)
            {
                int initialPosition = m_currentPiece.Y;
                while (ValidPosition())
                {
                    m_currentPiece.Shift(0, 1);
                }
                m_currentPiece.Shift(0, -1);
                return m_currentPiece.Y - initialPosition;
            }
            return 0;
        }

        public bool ValidPosition()
        {
            if (!m_gameOver)
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
            return false;
        }

        /// <summary>
        /// Incorporates current piece into the board.
        /// </summary>
        /// <returns>-1 if game over, 0 if no lines cleared, > 0 nomber of cleared lines</returns>
        public int Incorporate()
        {
            if (!m_gameOver)
            {
                if (ValidPosition())
                {
                    foreach (Point block in m_currentPiece.CurrentBlocks)
                    {
                        m_board[m_currentPiece.X + block.X][m_currentPiece.Y + block.Y] = m_currentPiece.Color;
                    }
                    if (PieceLock != null)
                    {
                        PieceLock(this, new PieceLockEventArgs(this, m_currentPiece));
                    }
                    m_currentPiece = null;
                    List<int> lines = CheckCompleteLines();
                    if (lines.Count > 0)
                    {
                        DeleteCompleteLines();
                        if (LineCleared != null)
                        {
                            LineCleared(this, new LineClearedEventArgs(this, lines));
                        }
                        TransformRandomBlocksToSpecial(lines.Count);
                        return lines.Count;
                    }
                    return 0;
                }
                Loose();
                return -1;
            }
            return -1;
        }

        public List<int> CheckCompleteLines()
        {
            var result = new List<int>();
            if (!m_gameOver)
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
                        result.Add(j);
                    }
                }
            }
            return result;
        }

        public void DeleteCompleteLines()
        {
            if (!m_gameOver)
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
                        CollapseLine(j);
                        j++; // rechecks same line
                    }
                }
            }
        }

        private void CollapseLine(int line)
        {
            if (line > 0)
            {
                for (int j = line; j > 0; j--)
                {
                    for (int i = 0; i < m_hSize; i++)
                    {
                        m_board[i][j] = m_board[i][j - 1];
                    }
                }
                for (int i = 0; i < m_hSize; i++)
                {
                    m_board[i][0] = Block.EMPTY;
                }
            }
        }

        public void TransformRandomBlocksToSpecial(int nBlocks)
        {
            if (!m_gameOver)
            {
                var allStandardBlocks = new List<Point>();
                for (int i = 0; i < m_hSize; i++)
                {
                    for (int j = 0; j < m_vSize; j++)
                    {
                        if (Blocks.STANDARD_BLOCKS.Contains(m_board[i][j]))
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
                    Block blockType = Blocks.SPECIAL_BLOCKS[s_random.Next(Blocks.SPECIAL_BLOCKS.Count)];
                    m_board[selectedBlock.X][selectedBlock.Y] = blockType;
                    nBlocks--;
                    if (BonusAppear != null)
                    {
                        BonusAppear(this, new BonusAppearEventArgs(this, selectedBlock, blockType));
                    }
                }
            }
        }

        public void Clear()
        {
            for (int i = 0; i < m_hSize; i++)
            {
                for (int j = 0; j < m_vSize; j++)
                {
                    m_board[i][j] = Block.EMPTY;
                }
            }
        }

        public void ClearLine()
        {
            if (!m_gameOver)
            {
                CollapseLine(m_vSize - 1);
                if (!ValidPosition())
                {
                    m_currentPiece.Y = m_currentPiece.Y + 1;
                }
            }
        }

        public void AddLine()
        {
            if (!m_gameOver)
            {
                for (int j = 0; j < (m_vSize - 1); j++)
                {
                    for (int i = 0; i < m_hSize; i++)
                    {
                        m_board[i][j] = m_board[i][j + 1];
                    }
                }
                int randomHole = s_random.Next(m_hSize);
                for (int i = 0; i < m_hSize; i++)
                {
                    if (i == randomHole)
                    {
                        m_board[i][m_vSize - 1] = Block.EMPTY;
                    }
                    else
                    {
                        m_board[i][m_vSize - 1] = Blocks.STANDARD_BLOCKS[s_random.Next(Blocks.STANDARD_BLOCKS.Count)];
                    }
                }
                if (!ValidPosition())
                {
                    m_currentPiece.Y = m_currentPiece.Y - 1;
                }
            }
        }

        public bool ApplyBonus(Block bonus)
        {
            if (!m_gameOver)
            {
                switch (bonus)
                {
                    case Block.BONUS_C:
                        ClearLine();
                        return true;
                    case Block.BONUS_N:
                        Clear();
                        return true;
                    case Block.MALUS_A:
                        AddLine();
                        return true;
                }
            }
            return false;
        }
    }
}