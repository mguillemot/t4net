using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace T4NET.ZeGame.Pieces
{
    public class OPiece : Piece
    {
        private static readonly List<Point> s_blocks = new List<Point>();
        private static readonly List<Point> s_leftWallKicks = new List<Point>();
        private static readonly List<Point> s_rightWallKicks = new List<Point>();

        static OPiece()
        {
            s_blocks = new List<Point>
                           {
                               new Point(0, 0),
                               new Point(0, 1),
                               new Point(1, 0),
                               new Point(1, 1)
                           };
        }

        public override int Size
        {
            get { return 2; }
        }

        public override Block Color
        {
            get { return Block.YELLOW; }
        }

        public override PieceType Type
        {
            get { return PieceType.O; }
        }

        public override List<Point> CurrentBlocks
        {
            get { return s_blocks; }
        }

        public override List<Point> LeftWallKicks
        {
            get { return s_leftWallKicks; }
        }

        public override List<Point> RightWallKicks
        {
            get { return s_rightWallKicks; }
        }

        protected override Piece NewInstance()
        {
            return new OPiece();
        }
    }
}