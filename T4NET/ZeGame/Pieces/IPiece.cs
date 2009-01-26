﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace T4NET.ZeGame.Pieces
{
    public class IPiece : Piece
    {
        private static readonly List<Point>[] s_blocks = new List<Point>[4];
        private static readonly List<Point>[] s_leftWallKicks = new List<Point>[4];
        private static readonly List<Point>[] s_rightWallKicks = new List<Point>[4];

        static IPiece()
        {
            s_blocks[0] = new List<Point>
                              {
                                  new Point(0, 1),
                                  new Point(1, 1),
                                  new Point(2, 1),
                                  new Point(3, 1)
                              };
            s_blocks[1] = new List<Point>
                              {
                                  new Point(2, 0),
                                  new Point(2, 1),
                                  new Point(2, 2),
                                  new Point(2, 3)
                              };
            s_blocks[2] = new List<Point>
                              {
                                  new Point(0, 2),
                                  new Point(1, 2),
                                  new Point(2, 2),
                                  new Point(3, 2)
                              };
            s_blocks[3] = new List<Point>
                              {
                                  new Point(1, 0),
                                  new Point(1, 1),
                                  new Point(1, 2),
                                  new Point(1, 3)
                              };
            s_leftWallKicks[0] = new List<Point>
                                 {
                                     new Point(2, 0),
                                     new Point(-1, 0),
                                     new Point(2, -1),
                                     new Point(-1, 2)
                                 };
            s_rightWallKicks[0] = new List<Point>
                                 {
                                     new Point(-2, 0),
                                     new Point(1, 0),
                                     new Point(-2, -1),
                                     new Point(1, 2)
                                 };
            s_leftWallKicks[1] = new List<Point>
                                 {
                                     new Point(-2, 0),
                                     new Point(1, 0),
                                     new Point(-2, -1),
                                     new Point(1, 1)
                                 };
            s_rightWallKicks[1] = new List<Point>
                                 {
                                     new Point(-2, 0),
                                     new Point(1, 0),
                                     new Point(1, -2),
                                     new Point(-2, 1)
                                 };
            s_leftWallKicks[2] = new List<Point>
                                 {
                                     new Point(1, 0),
                                     new Point(-2, 0),
                                     new Point(1, -2),
                                     new Point(-2, 1)
                                 };
            s_rightWallKicks[2] = new List<Point>
                                 {
                                     new Point(-1, 0),
                                     new Point(2, 0),
                                     new Point(-1, -2),
                                     new Point(2, 1)
                                 };
            s_leftWallKicks[3] = new List<Point>
                                 {
                                     new Point(2, 0),
                                     new Point(-1, 0),
                                     new Point(-1, -2),
                                     new Point(2, 1)
                                 };
            s_rightWallKicks[3] = new List<Point>
                                 {
                                     new Point(2, 0),
                                     new Point(-1, 0),
                                     new Point(2, -1),
                                     new Point(-1, 1)
                                 };
        }

        public override int Size
        {
            get { return 4; }
        }

        public override Block Color
        {
            get { return Block.LIGHT_BLUE; }
        }

        public override PieceType Type
        {
            get { return PieceType.I; }
        }

        public override List<Point> CurrentBlocks
        {
            get { return s_blocks[R]; }
        }

        public override List<Point> LeftWallKicks
        {
            get { return s_leftWallKicks[R]; }
        }

        public override List<Point> RightWallKicks
        {
            get { return s_rightWallKicks[R]; }
        }

        protected override Piece NewInstance()
        {
            return new IPiece();
        }
    }
}