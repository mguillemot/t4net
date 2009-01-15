using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace T4NET
{
    public class ControlsComponent : GameComponent, IControlsProvider
    {
        private readonly ControlsConfig m_controlsConfig = new ControlsConfig();
        private readonly ControlsState m_controlsState = new ControlsState();

        public ControlsComponent(Game game) 
            : base(game)
        {
        }

        public override void Initialize()
        {
            // Default controls config
            m_controlsConfig.GetControls(Function.LEFT).AddKey(Keys.Left);
            m_controlsConfig.GetControls(Function.LEFT).AddButton(Buttons.LeftThumbstickLeft);
            m_controlsConfig.GetControls(Function.RIGHT).AddKey(Keys.Right);
            m_controlsConfig.GetControls(Function.RIGHT).AddButton(Buttons.LeftThumbstickRight);
            m_controlsConfig.GetControls(Function.DOWN).AddKey(Keys.Down);
            m_controlsConfig.GetControls(Function.DOWN).AddButton(Buttons.LeftThumbstickDown);
            m_controlsConfig.GetControls(Function.UP).AddKey(Keys.Up);
            m_controlsConfig.GetControls(Function.UP).AddButton(Buttons.LeftThumbstickUp);
            m_controlsConfig.GetControls(Function.ROTATE_L).AddKey(Keys.W);
            m_controlsConfig.GetControls(Function.ROTATE_L).AddButton(Buttons.A);
            m_controlsConfig.GetControls(Function.ROTATE_R).AddKey(Keys.X);
            m_controlsConfig.GetControls(Function.ROTATE_R).AddButton(Buttons.B);
            m_controlsConfig.GetControls(Function.REGEN_PIECE).AddKey(Keys.C);
            m_controlsConfig.GetControls(Function.REGEN_PIECE).AddButton(Buttons.Y);

            // Register as a service
            Game.Services.AddService(typeof(IControlsProvider), this);
        }

        public override void Update(GameTime gameTime)
        {
            var padState = GamePad.GetState(PlayerIndex.One);
            var keyboardState = Keyboard.GetState(PlayerIndex.One);
            m_controlsState.ComputeState(keyboardState, padState);
        }

        public ControlsState CurrentState
        {
            get { return m_controlsState; }
        }

        public ControlsConfig CurrentConfig
        {
            get { return m_controlsConfig; }
        }
    }
}
