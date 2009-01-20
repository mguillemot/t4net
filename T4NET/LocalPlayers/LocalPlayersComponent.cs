using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using T4NET.Controls;

namespace T4NET.LocalPlayers
{
    public class LocalPlayersComponent : GameComponent
    {
        public LocalPlayersComponent(Game game) 
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            SignedInGamer.SignedIn += GamerSignedIn;
            SignedInGamer.SignedOut += GamerSignedOut;

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            var controlsProvider = (IControlsProvider) Game.Services.GetService(typeof (IControlsProvider));
            if (((T4Net) Game).HasGamerService && !Guide.IsVisible && controlsProvider.CurrentState.PressedKeys.Contains(Keys.F12))
            {
                Guide.ShowSignIn(1, false);
            }

        }

        private static void GamerSignedIn(object sender, SignedInEventArgs e)
        {
            Console.WriteLine("Local " + e.Gamer.Gamertag + " signed in");
        }

        private static void GamerSignedOut(object sender, SignedOutEventArgs e)
        {
            Console.WriteLine("Local " + e.Gamer.Gamertag + " signed out");
        }


    }
}
