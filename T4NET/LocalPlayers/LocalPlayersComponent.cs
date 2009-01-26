using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.GamerServices;

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
