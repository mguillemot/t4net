using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using T4NET.Graphic;

namespace T4NET
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class T4Net : Game
    {
        private readonly Board m_board;
        private readonly BoardDisplay m_boardDisplay;
        private readonly BoardControl m_boardControl;

        private readonly Board m_enemyBoard;
        private readonly BoardDisplay m_enemyBoardDisplay;

        private readonly ContolsState m_controlsState = new ContolsState();
        private readonly ControlsConfig m_controlsConfig;

        private NetworkSession m_networkSession;

        private GraphicsDeviceManager graphics;
        private BasicEffect basicEffect;
        private SpriteBatch spriteBatch;

        private ConsoleDisplay m_console;

        private Matrix projectionMatrix;
        private Matrix viewMatrix;
        private Matrix worldMatrix;

        private bool m_gamerServiceInitialized = false;

        public T4Net()
        {
            //Components.Add(new GamerServicesComponent(this));
            //m_gamerServiceInitialized = true;
            SignedInGamer.SignedIn += GamerSignedIn;

            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            Content.RootDirectory = "Content";

            m_board = new Board(10, 20);
            m_boardDisplay = new BoardDisplay(m_board);
            m_boardControl = new BoardControl(m_board);

            m_enemyBoard = new Board(10, 20);
            m_enemyBoardDisplay = new BoardDisplay(m_enemyBoard);

            m_controlsConfig = new ControlsConfig();
            m_controlsConfig.GetControls(Function.LEFT).AddKey(Keys.Left);
            m_controlsConfig.GetControls(Function.RIGHT).AddKey(Keys.Right);
            m_controlsConfig.GetControls(Function.DOWN).AddKey(Keys.Down);
            m_controlsConfig.GetControls(Function.UP).AddKey(Keys.Up);
            m_controlsConfig.GetControls(Function.ROTATE_L).AddKey(Keys.W);
            m_controlsConfig.GetControls(Function.ROTATE_R).AddKey(Keys.X);
            m_controlsConfig.GetControls(Function.REGEN_PIECE).AddKey(Keys.C);

            m_console = new ConsoleDisplay();
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            foreach (var displayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                if (displayMode.Width == 1280 && displayMode.Height == 720)
                {
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferFormat = displayMode.Format;
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferHeight = displayMode.Height;
                    e.GraphicsDeviceInformation.PresentationParameters.BackBufferWidth = displayMode.Width;
                    //e.GraphicsDeviceInformation.PresentationParameters.FullScreenRefreshRateInHz = displayMode.RefreshRate;
                    //e.GraphicsDeviceInformation.PresentationParameters.IsFullScreen = true;
                    return;
                }
            }

        }

        private void GamerSignedIn(object sender, SignedInEventArgs e)
        {
            Console.WriteLine("gamer signed in");
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            basicEffect = new BasicEffect(GraphicsDevice, null);
            basicEffect.VertexColorEnabled = true;
            worldMatrix = Matrix.CreateTranslation(100.0f, 50.0f, 0.0f);
            viewMatrix = Matrix.CreateLookAt(new Vector3(0.0f, 0.0f, 1.0f), Vector3.Zero, Vector3.Up);
            projectionMatrix = Matrix.CreateOrthographicOffCenter(0, GraphicsDevice.Viewport.Width,
                                                                  GraphicsDevice.Viewport.Height, 0, 1.0f,
                                                                  1000.0f);
            basicEffect.World = worldMatrix;
            basicEffect.View = viewMatrix;
            basicEffect.Projection = projectionMatrix;

            m_boardDisplay.Initialize(GraphicsDevice, basicEffect);
            m_enemyBoardDisplay.Initialize(GraphicsDevice, basicEffect);
            m_console.Initialize(GraphicsDevice, basicEffect);
            Console.LineWidth = m_console.CharacterWidth;

            base.Initialize();

            Console.WriteLine("T4NET initialized...");
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            BoardDisplay.LoadContent(Content);
            ConsoleDisplay.LoadContent(Content);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            var keyboardState = Keyboard.GetState(PlayerIndex.One);
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (m_gamerServiceInitialized && !Guide.IsVisible && keyboardState.IsKeyDown(Keys.F12))
            {
                Guide.ShowSignIn(1, false);
            }
            if (m_gamerServiceInitialized && m_networkSession == null && keyboardState.IsKeyDown(Keys.F10))
            {
                m_networkSession = NetworkSession.Create(NetworkSessionType.Local, 2, 2);
                m_networkSession.GamerJoined += OnGamerJoined;
            }

            if (m_networkSession != null)
            {
                System.Console.WriteLine(m_networkSession.AllGamers.Count);
            }

            // Update board according to player actions
            var padState = GamePad.GetState(PlayerIndex.One);
            m_controlsState.ComputeState(keyboardState, padState);
            m_boardControl.UpdateBoard(gameTime, m_controlsConfig, m_controlsState);

            base.Update(gameTime);
        }

        private void OnGamerJoined(object sender, GamerJoinedEventArgs e)
        {
            Console.WriteLine("Gamer joined: " + e.Gamer.Gamertag);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            m_boardDisplay.Draw(new Point(100, 60), 1f);
            m_enemyBoardDisplay.Draw(new Point(500, 100), 0.8f);
            m_console.Draw();
            base.Draw(gameTime);
        }
    }
}