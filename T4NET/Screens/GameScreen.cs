using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using T4NET.AIs;
using T4NET.Controls;
using T4NET.Graphic;
using T4NET.Menus;

namespace T4NET.Screens
{
    public class GameScreen : Screen
    {
        private BasicEffect m_basicEffect;
        private Board m_board;
        private BoardControl m_boardControl;
        private BoardDisplay m_boardDisplay;

        private Board m_leftEnemyBoard;
        private BoardDisplay m_leftEnemyBoardDisplay;
        private BasicAi m_leftAi;
        private Board m_rightEnemyBoard;
        private BoardDisplay m_rightEnemyBoardDisplay;

        private Menu m_menu;
        private MenuDisplay m_menuDisplay;

        public override void Initialize(GraphicsDevice device)
        {
            base.Initialize(device);

            m_board = new Board(10, 20);
            m_boardDisplay = new BoardDisplay(m_board);
            m_boardControl = new BoardControl(m_board);

            m_leftEnemyBoard = new Board(10, 20);
            m_leftEnemyBoard.SwitchToNextPiece();
            m_leftEnemyBoardDisplay = new BoardDisplay(m_leftEnemyBoard);
            m_leftAi = new BasicAi(m_leftEnemyBoard);
            m_rightEnemyBoard = new Board(10, 20);
            m_rightEnemyBoardDisplay = new BoardDisplay(m_rightEnemyBoard);

            m_basicEffect = new BasicEffect(GraphicsDevice, null)
                                {
                                    VertexColorEnabled = true,
                                    World = Matrix.CreateTranslation(100.0f, 50.0f, 0.0f),
                                    View = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up),
                                    Projection = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,
                                                                                    GraphicsDevice.Viewport.Height, 0,
                                                                                    1.0f,
                                                                                    1000.0f)
                                };
            m_boardDisplay.Initialize(GraphicsDevice, m_basicEffect);
            m_leftEnemyBoardDisplay.Initialize(GraphicsDevice, m_basicEffect);
            m_rightEnemyBoardDisplay.Initialize(GraphicsDevice, m_basicEffect);

            m_menu = new Menu();
            m_menu.AddEntry(new MenuEntry {Title = "Pause game"});
            m_menu.AddEntry(new MenuEntry {Title = "Resume game"});
            m_menu.AddEntry(new MenuEntry {Title = "Return to menu"});
            m_menuDisplay = new MenuDisplay(m_menu);
            m_menuDisplay.Initialize(device);
        }

        public override void Update(GameTime time, GameServiceContainer services)
        {
            var controlsProvider = (IControlsProvider) services.GetService(typeof (IControlsProvider));
            m_boardControl.Update(time, controlsProvider);
            if (controlsProvider.CurrentState.PressedKeys.Contains(Keys.F1))
            {
                m_menuDisplay.Active = !m_menuDisplay.Active;
            }
            m_menuDisplay.Update(time, services);

            if (controlsProvider.CurrentState.PressedKeys.Contains(Keys.P))
            {
                m_leftAi.DoNextMove();
            }
        }

        public override void Draw()
        {
            m_boardDisplay.Draw(new Point(550, 60), 1f);
            m_leftEnemyBoardDisplay.Draw(new Point(150, 100), 0.8f);
            m_rightEnemyBoardDisplay.Draw(new Point(1000, 100), 0.8f);
            m_menuDisplay.Draw();
        }
    }
}