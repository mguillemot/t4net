using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace T4NET.ZeGame
{
    public class Piece : ICloneable
    {
        private static readonly Random s_pieceGenerator = new Random();
        
        private readonly List<Point>[] m_blocks = new List<Point>[4];
        private readonly PieceType m_type;
        private readonly Block m_color;
        private int m_x;
        private int m_y;
        private int m_r;

        public enum PieceType : byte
        {
            I = 1,
            O = 2,
            L = 3,
            J = 4,
            S = 5,
            Z = 6,
            T = 7
        }

        public object Clone()
        {
            return new Piece(m_type) { X = X, Y = Y, m_r = m_r };
        }

        public static Piece RandomPiece()
        {
            int rand = s_pieceGenerator.Next(1, 8);
            return new Piece((PieceType) rand);
        }

        public Piece(PieceType type)
        {
            m_type = type;
            switch (type)
            {
                case PieceType.I:
                    m_blocks[0] = new List<Point>
                                      {
                                          new Point(0, -2),
                                          new Point(0, -1),
                                          new Point(0, 0),
                                          new Point(0, 1)
                                      };
                    m_blocks[1] = new List<Point>
                                      {
                                          new Point(-1, 0),
                                          new Point(0, 0),
                                          new Point(1, 0),
                                          new Point(2, 0)
                                      };
                    m_blocks[2] = new List<Point>
                                      {
                                          new Point(1, -2),
                                          new Point(1, -1),
                                          new Point(1, 0),
                                          new Point(1, 1)
                                      };
                    m_blocks[3] = new List<Point>
                                      {
                                          new Point(-1, -1),
                                          new Point(0, -1),
                                          new Point(1, -1),
                                          new Point(2, -1)
                                      };
                    m_x = 4;
                    m_y = 2;
                    m_color = Block.RED;
                    break;
                case PieceType.O:
                    m_blocks[0] = new List<Point>
                                      {
                                          new Point(0, 0),
                                          new Point(1, 0),
                                          new Point(0, 1),
                                          new Point(1, 1)
                                      };
                    m_blocks[1] = m_blocks[0];
                    m_blocks[2] = m_blocks[0];
                    m_blocks[3] = m_blocks[0];
                    m_x = 4;
                    m_y = 1;
                    m_color = Block.YELLOW;
                    break;
                case PieceType.L:
                    m_blocks[0] = new List<Point>
                                      {
                                          new Point(-1, -1),
                                          new Point(-1, 0),
                                          new Point(-1, 1),
                                          new Point(0, 1)
                                      };
                    m_blocks[1] = new List<Point>
                                      {
                                          new Point(-1, 1),
                                          new Point(0, 1),
                                          new Point(1, 1),
                                          new Point(1, 0)
                                      };
                    m_blocks[2] = new List<Point>
                                      {
                                          new Point(0, -1),
                                          new Point(1, -1),
                                          new Point(1, 0),
                                          new Point(1, 1)
                                      };
                    m_blocks[3] = new List<Point>
                                      {
                                          new Point(-1, 1),
                                          new Point(-1, 0),
                                          new Point(0, 0),
                                          new Point(1, 0)
                                      };
                    m_x = 5;
                    m_y = 1;
                    m_color = Block.ORANGE;
                    break;
                case PieceType.J:
                    m_blocks[0] = new List<Point>
                                      {
                                          new Point(0, 1),
                                          new Point(1, 1),
                                          new Point(1, 0),
                                          new Point(1, -1)
                                      };
                    m_blocks[1] = new List<Point>
                                      {
                                          new Point(-1, 0),
                                          new Point(0, 0),
                                          new Point(1, 0),
                                          new Point(1, 1)
                                      };
                    m_blocks[2] = new List<Point>
                                      {
                                          new Point(-1, -1),
                                          new Point(-1, 0),
                                          new Point(-1, 1),
                                          new Point(0, -1)
                                      };
                    m_blocks[3] = new List<Point>
                                      {
                                          new Point(-1, 0),
                                          new Point(-1, 1),
                                          new Point(0, 1),
                                          new Point(1, 1)
                                      };
                    m_x = 4;
                    m_y = 1;
                    m_color = Block.DARK_BLUE;
                    break;
                case PieceType.S:
                    m_blocks[0] = new List<Point>
                                      {
                                          new Point(0, 0),
                                          new Point(1, 0),
                                          new Point(-1, 1),
                                          new Point(0, 1)
                                      };
                    m_blocks[1] = new List<Point>
                                      {
                                          new Point(0, -1),
                                          new Point(0, 0),
                                          new Point(1, 0),
                                          new Point(1, 1)
                                      };
                    m_blocks[2] = m_blocks[0];
                    m_blocks[3] = m_blocks[1];
                    m_x = 4;
                    m_y = 1;
                    m_color = Block.VIOLET;
                    break;
                case PieceType.Z:
                    m_blocks[0] = new List<Point>
                                      {
                                          new Point(-1, 0),
                                          new Point(0, 0),
                                          new Point(0, 1),
                                          new Point(1, 1)
                                      };
                    m_blocks[1] = new List<Point>
                                      {
                                          new Point(0, 0),
                                          new Point(0, 1),
                                          new Point(1, -1),
                                          new Point(1, 0)
                                      };
                    m_blocks[2] = m_blocks[0];
                    m_blocks[3] = m_blocks[1];
                    m_x = 4;
                    m_y = 1;
                    m_color = Block.GREEN;
                    break;
                case PieceType.T:
                    m_blocks[0] = new List<Point>
                                      {
                                          new Point(-1, 0),
                                          new Point(0, 0),
                                          new Point(1, 0),
                                          new Point(0, 1)
                                      };
                    m_blocks[1] = new List<Point>
                                      {
                                          new Point(0, -1),
                                          new Point(0, 0),
                                          new Point(0, 1),
                                          new Point(1, 0)
                                      };
                    m_blocks[2] = new List<Point>
                                      {
                                          new Point(-1, 0),
                                          new Point(0, 0),
                                          new Point(1, 0),
                                          new Point(0, -1)
                                      };
                    m_blocks[3] = new List<Point>
                                      {
                                          new Point(0, -1),
                                          new Point(0, 0),
                                          new Point(0, 1),
                                          new Point(-1, 0)
                                      };
                    m_x = 4;
                    m_y = 1;
                    m_color = Block.LIGHT_BLUE;
                    break;
                default:
                    throw new InvalidDataException("Piece type can not be " + type);
            }
        }

        public int X
        {
            get { return m_x; }
            set { m_x = value; }
        }

        public int Y
        {
            get { return m_y; }
            set { m_y = value; }
        }

        public int R
        {
            get { return m_r; }
            set { m_r = value; }
        }

        public Block Color
        {
            get { return m_color; }
        }

        public PieceType Type
        {
            get { return m_type; }
        }

        public List<Point> CurrentBlocks
        {
            get { return m_blocks[m_r]; }
        }

        public void RotateL()
        {
            m_r = (m_r + 1)%4;
        }

        public void RotateR()
        {
            m_r = (m_r + 3)%4;
        }

        public void Shift(int dx, int dy)
        {
            m_x += dx;
            m_y += dy;
        }

        public static bool Serialize(PacketWriter writer, Piece piece)
        {
            writer.Write((byte)piece.Type);
            writer.Write((sbyte)piece.X);
            writer.Write((sbyte)piece.Y);
            writer.Write((sbyte)piece.R);
            return true;
        }

        public static Piece Unserialize(PacketReader reader)
        {
            return new Piece((PieceType)reader.ReadByte())
                             {
                                 X = reader.ReadSByte(),
                                 Y = reader.ReadSByte(),
                                 R = reader.ReadSByte()
                             };
        }
    }
}