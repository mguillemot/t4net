using Microsoft.Xna.Framework;

namespace T4NET.AIs
{
    public class BasicAi
    {
        private readonly Board m_board;

        public BasicAi(Board board)
        {
            m_board = board;
        }

        public void DoNextMove()
        {
            var hyp1 = BestEstimatedMove(m_board);
            Apply(hyp1, m_board);
        }

        private static Hypothesis BestEstimatedMove(Board position)
        {
            var bestHyp = new Hypothesis {Heuristic = double.MaxValue};
            for (int i = 0; i < position.HSize; i++)
            {
                for (int r = 0; r < 2; r++) // TODO vérifier r<3
                {
                    var hyp = new Hypothesis{Board = position, X = i, R = r};
                    ComputeHeuristic(hyp);
                    if (hyp.Heuristic < bestHyp.Heuristic)
                    {
                        bestHyp = hyp;
                    }
                }
            }
            return bestHyp;
        }

        private static void Apply(Hypothesis hyp, Board board)
        {
            board.CurrentPiece.X = hyp.X;
            for (int i = 0; i < hyp.R; i++)
            {
                board.RotateRight();
            }
            board.InstantDrop();
            board.Incorporate();
            board.DeleteCompleteLines();
            board.SwitchToNextPiece();
        }

        private static void ComputeHeuristic(Hypothesis hyp)
        {
            var afterBoard = (Board)hyp.Board.Clone();
            for (int i = 0; i < hyp.R; i++)
            {
                afterBoard.RotateRight();
            }
            afterBoard.CurrentPiece.X = hyp.X;
            if (!afterBoard.ValidPosition())
            {
                hyp.Heuristic = double.MaxValue;
            }
            else
            {
                afterBoard.InstantDrop();
                afterBoard.Incorporate();
                hyp.Heuristic = EstimatePosition(afterBoard.Content);
            }
        }

        private static double EstimatePosition(Block[][] state)
        {
            int WIDTH = state.Length;
            int HEIGHT = state[0].Length;
            double estimation = 0.0;
            int topmost = 0;
            for (int j = HEIGHT - 1; j >= 1; j--)
            {
                bool completeLine = true;
                for (int i = 0; i < WIDTH; i++)
                {
                    Block current = state[i][j];
                    Block top = state[i][j - 1];
                    if (current == Block.EMPTY && top != Block.EMPTY)
                    {
                        // hole
                        estimation += 1.0;
                    }
                    if (top != Block.EMPTY)
                    {
                        topmost = HEIGHT - j;
                    }
                    if (current == Block.EMPTY)
                    {
                        completeLine = false;
                    }
                }
                if (completeLine)
                {
                    estimation -= 10;
                }
            }
            estimation += topmost;
            return estimation;
        }

        private class Hypothesis
        {
            public Board Board;
            public int X;
            public int R;
            public double Heuristic;
        }
    }
}