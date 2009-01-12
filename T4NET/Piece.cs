using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;

namespace T4NET
{
    public class Piece
    {
        private static readonly Random s_pieceGenerator = new Random();
        
        private readonly List<Point>[] m_blocks = new List<Point>[4];
        private readonly PieceType m_type;
        private readonly int m_color;
        private int m_currentRotation;
        private int m_x;
        private int m_y;

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
                    m_color = 5; // red
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
                    m_color = 7; // yellow
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
                    m_color = 4; // orange
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
                    m_color = 1; // dark blue
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
                    m_color = 6; // violet
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
                    m_color = 2; // green
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
                    m_color = 3; // light blue
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

        public int Color
        {
            get { return m_color; }
        }

        public PieceType Type
        {
            get { return m_type; }
        }

        public List<Point> CurrentBlocks
        {
            get { return m_blocks[m_currentRotation]; }
        }

        public void RotateL()
        {
            m_currentRotation = (m_currentRotation + 1)%4;
        }

        public void RotateR()
        {
            m_currentRotation = (m_currentRotation + 3)%4;
        }

        public void Shift(int dx, int dy)
        {
            m_x += dx;
            m_y += dy;
        }
    }
}