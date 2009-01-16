using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using T4NET.Controls;
using T4NET.Graphic;
using T4NET.Network;
using T4NET.Screens;

namespace T4NET
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class T4Net : Game
    {
        public T4Net()
        {
            //Components.Add(new GamerServicesComponent(this));
            Components.Add(new ControlsComponent(this));
            Components.Add(new ConsoleComponent(this));
            Components.Add(new ScreenComponent(this));
            Components.Add(new NetworkComponent(this));

            SignedInGamer.SignedIn += GamerSignedIn;
            SignedInGamer.SignedOut += GamerSignedOut;

            new GraphicsDeviceManager(this)
               {
                   PreferredBackBufferWidth = 1280,
                   PreferredBackBufferHeight = 720
               };
            //graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            Content.RootDirectory = "Content";
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

        private static void GamerSignedIn(object sender, SignedInEventArgs e)
        {
            Console.WriteLine(e.Gamer.Gamertag + " signed in");
        }

        private static void GamerSignedOut(object sender, SignedOutEventArgs e)
        {
            Console.WriteLine(e.Gamer.Gamertag + " signed out");
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
            Console.WriteLine("T4NET initialized...");
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

        public bool HasGamerService
        {
            get
            {
                foreach (var component in Components)
                {
                    if (component is GamerServicesComponent)
                    {
                        return true;
                    }
                }
                return false;
            }
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
            if (HasGamerService && !Guide.IsVisible && keyboardState.IsKeyDown(Keys.F12))
            {
                Guide.ShowSignIn(1, false);
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
