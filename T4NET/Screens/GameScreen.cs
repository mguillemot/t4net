using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using T4NET.AIs;
using T4NET.Controls;
using T4NET.Graphic;
using T4NET.Menus;
using T4NET.ZeGame;

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
        private AiComponent m_leftAi;
        private Board m_rightEnemyBoard;
        private BoardDisplay m_rightEnemyBoardDisplay;
        //private AiComponent m_rightAi;

        private Menu m_menu;
        private MenuDisplay m_menuDisplay;
        private MenuControl m_menuControl;

        public GameScreen(Game game) 
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            m_board = new Board(10, 20);
            m_boardDisplay = new BoardDisplay(m_board);
            m_boardControl = new BoardControl(m_board);

            m_leftEnemyBoard = new Board(10, 20);
            m_leftEnemyBoard.SwitchToNextPiece();
            m_leftEnemyBoardDisplay = new BoardDisplay(m_leftEnemyBoard);
            m_leftAi = new AiComponent(Game, m_leftEnemyBoard);
            Game.Components.Add(m_leftAi);
            m_rightEnemyBoard = new Board(10, 20);
            m_rightEnemyBoardDisplay = new BoardDisplay(m_rightEnemyBoard);
            //m_rightAi = new AiComponent(Game, m_rightEnemyBoard);
            //Game.Components.Add(m_rightAi);

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
            m_menu.MenuClosed += OnCloseMenu;
            var coucouEntry = new MenuEntry {Title = "Coucou !"};
            coucouEntry.EntryActivated += OnCoucou;
            m_menu.AddEntry(coucouEntry);
            var backToGameEntry = new MenuEntry {Title = "Back to game"};
            backToGameEntry.EntryActivated += OnCloseMenu;
            m_menu.AddEntry(backToGameEntry);
            m_menu.AddEntry(new MenuEntry {Title = "Return to menu"});
            m_menuDisplay = new MenuDisplay(m_menu);
            m_menuDisplay.Initialize(Game.GraphicsDevice);
            m_menuControl = new MenuControl(m_menu);
        }

        private void OnCloseMenu(object sender, EventArgs e)
        {
            m_menu.Active = false;
        }

        private static void OnCoucou(object sender, EventArgs e)
        {
            Console.WriteLine("Coucou!!");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                Game.Components.Remove(m_leftAi);
                //Game.Components.Remove(m_rightAi);
            }
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var controlsProvider = (IControlsProvider)Game.Services.GetService(typeof(IControlsProvider));
            if (controlsProvider.CurrentConfig.JustPressed(Function.GAME_MENU, controlsProvider.CurrentState))
            {
                m_menu.Active = !m_menu.Active;
            }

            if (m_menu.Active)
            {
                m_menuControl.Update(gameTime, controlsProvider);
            }
            else
            {
                m_boardControl.Update(gameTime, controlsProvider);
                if (controlsProvider.CurrentConfig.JustPressed(Function.GAME_BONUS_LEFT, controlsProvider.CurrentState))
                {
                    if (m_leftEnemyBoard.ApplyBonus(m_board.ActiveBonus))
                    {
                        m_board.CollectedBonuses.Pop();
                    }
                }
                if (controlsProvider.CurrentConfig.JustPressed(Function.GAME_BONUS_RIGHT, controlsProvider.CurrentState))
                {
                    if (m_rightEnemyBoard.ApplyBonus(m_board.ActiveBonus))
                    {
                        m_board.CollectedBonuses.Pop();
                    }
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            m_boardDisplay.Draw(new Point(530, 140), 1f);
            m_leftEnemyBoardDisplay.Draw(new Point(100, 180), 0.8f);
            m_rightEnemyBoardDisplay.Draw(new Point(1000, 180), 0.8f);
            m_menuDisplay.Draw();
        }
    }
}