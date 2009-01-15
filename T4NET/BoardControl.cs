using Microsoft.Xna.Framework;

namespace T4NET
{
    public class BoardControl
    {
        private readonly Board m_board;

        private BoardFunction m_currentFunction = BoardFunction.KEY_FUNCTION;
        private double m_currentFunctionStart;
        private double m_currentFunctionSub;
        private Function m_currentKeyFunction;
        private double m_lastAutoDrop;
        private double m_totalSeconds;

        public BoardControl(Board board)
        {
            m_board = board;
            AutoDropTime = 0.4;
        }

        private double AutoDropTime { get; set; }

        public void Update(GameTime gameTime, IControlsProvider controlsProvider)
        {
            m_totalSeconds = gameTime.TotalGameTime.TotalSeconds;
            double timeSinceStart = m_totalSeconds - m_currentFunctionStart;
            double timeSinceSub = m_totalSeconds - m_currentFunctionSub;
            var config = controlsProvider.CurrentConfig;
            var state = controlsProvider.CurrentState;

            if (config.JustPressed(Function.REGEN_PIECE, state) || m_board.CurrentPiece == null)
            {
                RegenPiece();
            }
            
            switch (m_currentFunction)
            {
                case BoardFunction.PIECE_LOCKING:
                    if (timeSinceStart > .3)
                    {
                        m_board.Incorporate();
                        var completeLines = m_board.CheckCompleteLines();
                        if (completeLines.Count > 0)
                        {
                            // If lines are found
                            m_currentFunction = BoardFunction.LINE_VANISHING;
                            m_currentFunctionStart = m_totalSeconds;
                        }
                        else
                        {
                            m_currentFunction = BoardFunction.NONE;
                            m_currentFunctionStart = m_totalSeconds;
                            RegenPiece();
                        }
                    }
                    break;
                case BoardFunction.LINE_VANISHING:
                    if (timeSinceStart > .3)
                    {
                        m_board.DeleteCompleteLines();
                        m_currentFunction = BoardFunction.NONE;
                        m_currentFunctionStart = m_totalSeconds;
                        RegenPiece();

                    }
                    break;
                case BoardFunction.NONE:
                    if (config.IsPressed(Function.LEFT, state))
                    {
                        m_currentKeyFunction = Function.LEFT;
                        m_currentFunctionStart = -1;
                    } 
                    else if (config.IsPressed(Function.RIGHT, state))
                    {
                        m_currentKeyFunction = Function.RIGHT;
                        m_currentFunctionStart = -1;
                    }
                    else if (config.IsPressed(Function.UP, state))
                    {
                        m_currentKeyFunction = Function.UP;
                        m_currentFunctionStart = -1;
                    }
                    else if (config.IsPressed(Function.DOWN, state))
                    {
                        m_currentKeyFunction = Function.DOWN;
                        m_currentFunctionStart = -1;
                    }
                    m_currentFunction = BoardFunction.KEY_FUNCTION;
                    break;
                case BoardFunction.KEY_FUNCTION:
                    if (config.JustPressed(Function.RIGHT, state))
                    {
                        m_currentKeyFunction = Function.RIGHT;
                        m_currentFunctionStart = m_totalSeconds;
                        MoveRight();
                    }
                    else if (config.JustPressed(Function.LEFT, state))
                    {
                        m_currentKeyFunction = Function.LEFT;
                        m_currentFunctionStart = m_totalSeconds;
                        MoveLeft();
                    }
                    else if (config.JustPressed(Function.DOWN, state))
                    {
                        m_currentKeyFunction = Function.DOWN;
                        m_currentFunctionStart = m_totalSeconds;
                        MoveDown();
                    }
                    else if (config.JustPressed(Function.UP, state))
                    {
                        m_currentKeyFunction = Function.UP;
                        m_currentFunctionStart = m_totalSeconds;
                        MoveUp();
                    }
                    else if (config.JustPressed(Function.ROTATE_R, state))
                    {
                        m_board.RotateRight();
                    }
                    else if (config.JustPressed(Function.ROTATE_L, state))
                    {
                        m_board.RotateLeft();
                    }
                    else if (m_currentKeyFunction == Function.RIGHT && timeSinceStart > 0.3 && timeSinceSub > 0.05 &&
                             config.IsPressed(Function.RIGHT, state))
                    {
                        MoveRight();
                    }
                    else if (m_currentKeyFunction == Function.LEFT && timeSinceStart > 0.3 && timeSinceSub > 0.05 &&
                             config.IsPressed(Function.LEFT, state))
                    {
                        MoveLeft();
                    }
                    else if (m_currentKeyFunction == Function.DOWN && timeSinceSub > 0.02 &&
                             config.IsPressed(Function.DOWN, state))
                    {
                        MoveDown();
                    }
                    else if (m_currentKeyFunction == Function.UP &&
                             config.IsPressed(Function.UP, state))
                    {
                        MoveUp();
                    }

                    if ((m_totalSeconds - m_lastAutoDrop) > AutoDropTime)
                    {
                        MoveDown();
                    }
                    break;
            }
        }

        private void MoveRight()
        {
            if (m_board.CanMoveRight())
            {
                m_board.CurrentPiece.Shift(1, 0);
                m_currentFunctionSub = m_totalSeconds;
            }
        }

        private void MoveLeft()
        {
            if (m_board.CanMoveLeft())
            {
                m_board.CurrentPiece.Shift(-1, 0);
                m_currentFunctionSub = m_totalSeconds;
            }
        }

        private void MoveDown()
        {
            if (m_board.CanMoveDown())
            {
                m_board.CurrentPiece.Shift(0, 1);
                m_lastAutoDrop = m_totalSeconds;
                m_currentFunctionSub = m_totalSeconds;
            }
            else
            {
                LockPiece();
            }
        }

        private void MoveUp()
        {
            // Scheme #1: piece doesn't instantly lock
            //if (m_board.InstantDrop() > 0)
            //{
            //    m_lastAutoDrop = m_totalSeconds;
            //}

            // Scheme #2: piece does instantly lock
            m_board.InstantDrop();
            LockPiece();
        }

        private void RegenPiece()
        {
            m_board.CurrentPiece = Piece.RandomPiece();
        }

        private void LockPiece()
        {
            m_currentFunction = BoardFunction.PIECE_LOCKING;
            m_currentFunctionStart = m_totalSeconds;
        }

        #region Nested type: BoardFunction

        private enum BoardFunction
        {
            NONE,
            KEY_FUNCTION,
            PIECE_LOCKING,
            LINE_VANISHING
        }

        #endregion
    }
}