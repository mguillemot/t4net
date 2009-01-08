using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace T4NET
{
    public class Piece
    {
        private const string ALL_PIECES = "IOLJSZT";
        private static readonly Random s_pieceGenerator = new Random();
        private readonly List<Point>[] m_blocks = new List<Point>[4];

        private readonly Color m_color;
        private int m_currentRotation;
        private int m_x;
        private int m_y;

        public Piece(char type)
        {
            switch (type)
            {
                case 'I':
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
                    m_color = new Color(40, 255, 244);
                    break;
                case 'O':
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
                    m_color = new Color(94, 113, 255);
                    break;
                case 'L':
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
                    m_color = new Color(40, 255, 244);
                    break;
                case 'J':
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
                    m_color = new Color(40, 255, 244);
                    break;
                case 'S':
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
                    /*m_blocks[2] = new List<Point>
                                      {
                                          new Point(0, 0),
                                          new Point(1, 0),
                                          new Point(-1, 1),
                                          new Point(0, 1)
                                      };
                    m_blocks[3] = new List<Point>
                                      {
                                          new Point(0, 0),
                                          new Point(0, 1),
                                          new Point(-1, -1),
                                          new Point(-1, 0)
                                      };*/
                    m_blocks[2] = m_blocks[0];
                    m_blocks[3] = m_blocks[1];
                    m_color = new Color(40, 255, 244);
                    break;
                case 'Z':
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
                    /*m_blocks[2] = new List<Point>
                                      {
                                          new Point(-1, 0),
                                          new Point(0, 0),
                                          new Point(0, 1),
                                          new Point(1, 1)
                                      };
                    m_blocks[3] = new List<Point>
                                      {
                                          new Point(0, 0),
                                          new Point(0, 1),
                                          new Point(1, -1),
                                          new Point(1, 0)
                                      };*/
                    m_blocks[2] = m_blocks[0];
                    m_blocks[3] = m_blocks[1];
                    m_color = new Color(40, 255, 244);
                    break;
                case 'T':
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
                    m_color = new Color(40, 255, 244);
                    break;
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

        public Color Color
        {
            get { return m_color; }
        }

        public List<Point> CurrentBlocks
        {
            get { return m_blocks[m_currentRotation]; }
        }

        public static Piece RandomPiece()
        {
            int rand = s_pieceGenerator.Next(ALL_PIECES.Length);
            char pieceType = ALL_PIECES[rand];
            return new Piece(pieceType);
        }

        public void RotateL()
        {
            m_currentRotation = (m_currentRotation + 1)%4;
        }

        public void RotateR()
        {
            m_currentRotation = (m_currentRotation + 3)%4;
        }
    }
}