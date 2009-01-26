using T4NET.ZeGame.Pieces;

namespace T4NET.ZeGame
{
    public static class PieceFactory
    {
        public static Piece CreatePiece(PieceType type)
        {
            switch (type)
            {
                case PieceType.I:
                    return new IPiece();
                case PieceType.O:
                    return new OPiece();
                case PieceType.L:
                    return new LPiece();
                case PieceType.J:
                    return new JPiece();
                case PieceType.S:
                    return new SPiece();
                case PieceType.Z:
                    return new ZPiece();
                case PieceType.T:
                    return new TPiece();
            }
            return null;
        }

        public static Piece CreateInitialPositionPiece(Board board, PieceType type)
        {
            var piece = CreatePiece(type);
            piece.X = (board.HSize - piece.Size)/2;
            return piece;
        }
    }
}