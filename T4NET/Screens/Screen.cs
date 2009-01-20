using Microsoft.Xna.Framework;

namespace T4NET.Screens
{
    public abstract class Screen : DrawableGameComponent
    {
        protected Screen(Game game) 
            : base(game)
        {
        }
    }
}