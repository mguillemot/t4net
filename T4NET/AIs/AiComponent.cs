using Microsoft.Xna.Framework;
using T4NET.ZeGame;

namespace T4NET.AIs
{
    public class AiComponent : GameComponent
    {
        private readonly BasicAi m_ai;

        private double m_lastUpdate;

        public AiComponent(Game game, Board board) 
            : base(game)
        {
            m_ai = new BasicAi(board);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            double elapsed = gameTime.TotalGameTime.TotalSeconds - m_lastUpdate;
            if (elapsed > 1)
            {
                m_ai.DoNextMove();
                m_lastUpdate = gameTime.TotalGameTime.TotalSeconds;
            }
        }
    }
}
