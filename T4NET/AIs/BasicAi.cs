using T4NET.ZeGame;

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
            if (!m_board.IsGameOver)
            {
                Hypothesis hyp1 = BestEstimatedMove(m_board);
                Apply(hyp1, m_board);
            }
        }

        private static Hypothesis BestEstimatedMove(Board position)
        {
            var bestHyp = new Hypothesis {Heuristic = double.MaxValue};
            for (int i = 0; i < position.HSize; i++)
            {
                for (int r = 0; r < 3; r++)
                {
                    var hyp = new Hypothesis {Board = position, X = i, R = r};
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
            var afterBoard = (Board) hyp.Board.Clone();
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
                afterBoard.CheckCompleteLines();
                hyp.Heuristic = EstimatePosition(afterBoard.Content);
            }
        }

        private static double EstimatePosition(Block[][] state)
        {
            int WIDTH = state.Length;
            int HEIGHT = state[0].Length;
            double estimation = 0.0;
            int topmost = 0;
            for (int j = HEIGHT - 1; j >= 0; j--)
            {
                bool completeLine = true;
                for (int i = 0; i < WIDTH; i++)
                {
                    Block current = state[i][j];
                    if (current == Block.EMPTY)
                    {
                        completeLine = false;
                    }
                    else
                    {
                        topmost = j;
                    }
                }
                if (completeLine) // TODO ne peut pas arriver puisqu'on vire les lignes juste au dessus
                {
                    estimation -= 10;
                }
            }
            for (int i = 0; i < WIDTH; i++)
            {
                int nHoles = 0;
                int nBlocks = 0;
                int lastBlock = 0;
                for (int j = HEIGHT - 1; j >= 0; j--)
                {
                    Block current = state[i][j];
                    if (current == Block.EMPTY)
                    {
                        nHoles++;
                    }
                    else
                    {
                        lastBlock = j;
                        nBlocks++;
                    }
                }
                if (nBlocks > 0)
                {
                    nHoles -= lastBlock;
                }
                else
                {
                    nHoles = 0;
                }
                estimation += nHoles;
            }
            estimation += (HEIGHT - topmost)/2.0;
            return estimation;
        }

        #region Nested type: Hypothesis

        private class Hypothesis
        {
            public Board Board;
            public double Heuristic;
            public int R;
            public int X;
        }

        #endregion
    }
}