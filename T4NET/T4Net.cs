using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using T4NET.Controls;
using T4NET.Graphic;
using T4NET.Leaderboards;
using T4NET.LocalPlayers;
using T4NET.Network;
using T4NET.Screens;
using T4NET.ZeGame;

namespace T4NET
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class T4Net : Game
    {
        public T4Net()
        {
            Components.Add(new ControlsComponent(this));
            Components.Add(new LocalPlayersComponent(this));
            Components.Add(new GamerServicesComponent(this));
            Components.Add(new NetworkComponent(this));
            Components.Add(new MessageDispatcherComponent(this));
            Components.Add(new GameSessionComponent(this));
            Components.Add(new GameScreen(this));
            Components.Add(new ConsoleComponent(this));

            var graphics = new GraphicsDeviceManager(this)
                               {
                                   PreferredBackBufferWidth = 1280,
                                   PreferredBackBufferHeight = 720
                               };
            graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            Content.RootDirectory = "Content";
        }

        public bool HasGamerService
        {
            get
            {
                foreach (IGameComponent component in Components)
                {
                    if (component is GamerServicesComponent)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        private static void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            /*foreach (var displayMode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Console.WriteLine("Possible resolution: {0}x{1} @ {2} Hz",
                                         displayMode.Width,
                                         displayMode.Height,
                                         displayMode.RefreshRate);
            }*/
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            Console.WriteLine("T4NET initialized...\nViewport=({0}:{1} {2}x{3})", GraphicsDevice.Viewport.X,
                              GraphicsDevice.Viewport.Y,
                              GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            Console.WriteLine("Tile-safe area: " +
                              GraphicsDevice.Viewport.TitleSafeArea.ToString().Replace('{', '[').Replace('}', ']'));

            if (HasGamerService)
            {
                Guide.BeginShowStorageDeviceSelector(OnDeviceGot, null);
            }
        }

        private static void OnDeviceGot(IAsyncResult result)
        {
            StorageDevice device = Guide.EndShowStorageDeviceSelector(result);
            if (device.IsConnected)
            {
                var leaderboard = new Leaderboard();
                leaderboard.Entries.Add(new LeaderboardEntry
                                            {
                                                Date = DateTime.Now.Ticks,
                                                Distance = 42,
                                                GamerTag = "Toto",
                                                Experience = 42000000
                                            });
                leaderboard.Save(device);
                System.Console.WriteLine("Saved");
            }
            else
            {
                System.Console.WriteLine("Save KO");
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            BoardDisplay.LoadContent(Content);
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
            KeyboardState keyboardState = Keyboard.GetState(PlayerIndex.One);
            if (keyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}