using Microsoft.Xna.Framework;

namespace T4NET.ZeGame
{
    public class GameSessionComponent : GameComponent
    {
        private readonly GameSession m_gameSession = new GameSession();

        public GameSessionComponent(Game game) 
            : base(game)
        {
        }
    }
}
