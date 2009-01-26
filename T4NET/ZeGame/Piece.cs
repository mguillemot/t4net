using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Net;

namespace T4NET.ZeGame
{
    public abstract class Piece : ICloneable
    {
        public int X { get; set; }

        public int Y { get; set; }

        public int R { get; set; }

        public abstract int Size { get; }

        public abstract Block Color { get; }

        public abstract PieceType Type { get; }

        public abstract List<Point> CurrentBlocks { get; }

        public abstract List<Point> LeftWallKicks { get; }

        public abstract List<Point> RightWallKicks { get; }

        protected abstract Piece NewInstance();

        public object Clone()
        {
            var clone = NewInstance();
            clone.X = X;
            clone.Y = Y;
            clone.R = R;
            return clone;
        }

        public void RotateL()
        {
            R = (R + 3)%4;
        }

        public void RotateR()
        {
            R = (R + 1)%4;
        }

        public void Shift(int dx, int dy)
        {
            X += dx;
            Y += dy;
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
            var piece = PieceFactory.CreatePiece((PieceType)reader.ReadByte());
            piece.X = reader.ReadSByte();
            piece.Y = reader.ReadSByte();
            piece.R = reader.ReadSByte();
            return piece;
        }
    }
}