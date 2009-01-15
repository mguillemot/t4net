using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using T4NET.Graphic;
using T4NET.Menu;
using T4NET.Menu.Screens;

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

        private Screen m_currentScreen;

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
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            //graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            Content.RootDirectory = "Content";

            m_board = new Board(10, 20);
            m_boardDisplay = new BoardDisplay(m_board);
            m_boardControl = new BoardControl(m_board);

            m_enemyBoard = new Board(10, 20);
            m_enemyBoardDisplay = new BoardDisplay(m_enemyBoard);

            m_currentScreen = new TitleScreen();

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
            Console.WriteLine(e.Gamer.Gamertag + " signed in");
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

            m_currentScreen.Initialize(GraphicsDevice);

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
            MenuDisplay.LoadContent(Content);
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
            if (keyboardState.IsKeyDown(Keys.Escape) || GamePad.GetState(0).IsButtonDown(Buttons.Back))
            {
                Exit();
            }
            if (m_gamerServiceInitialized && !Guide.IsVisible && keyboardState.IsKeyDown(Keys.F12))
            {
                Guide.ShowSignIn(1, false);
            }
            if (m_gamerServiceInitialized && m_networkSession == null && (keyboardState.IsKeyDown(Keys.F10) || GamePad.GetState(0).IsButtonDown(Buttons.Start)))
            {
                m_networkSession = NetworkSession.Create(NetworkSessionType.SystemLink, 1, 8, 2, new NetworkSessionProperties());
                Console.WriteLine("Session " + m_networkSession.Host.Gamertag + " created");
                m_networkSession.AllowJoinInProgress = true;
                m_networkSession.GamerJoined += OnGamerJoined;
            }
            if (m_gamerServiceInitialized && (keyboardState.IsKeyDown(Keys.F11) || GamePad.GetState(0).IsButtonDown(Buttons.LeftShoulder)))
            {
                var sessions = NetworkSession.Find(NetworkSessionType.SystemLink, 1, new NetworkSessionProperties());
                foreach (var session in sessions)
                {
                    Console.WriteLine("======= " + session.HostGamertag + " has " + session.CurrentGamerCount);
                }
                Console.WriteLine("Enumerate sessions done");
            }

            if (m_networkSession != null)
            {
                m_networkSession.Update();
                System.Console.WriteLine(m_networkSession.AllGamers.Count);
            }

            var controlsProvider = (IControlsProvider) Services.GetService(typeof(IControlsProvider));
            m_boardControl.Update(gameTime, controlsProvider);

            // Update current screen
            m_currentScreen.Update(gameTime);

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
            m_currentScreen.Draw();
            base.Draw(gameTime);
        }
    }
}