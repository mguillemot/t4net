using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace T4NET.Controls
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
            base.Initialize();

            // Default controls config
            m_controlsConfig.GetFunctionConfig(Function.GAME_LEFT).AddKey(Keys.Left);
            m_controlsConfig.GetFunctionConfig(Function.GAME_LEFT).AddButton(Buttons.LeftThumbstickLeft);
            m_controlsConfig.GetFunctionConfig(Function.GAME_RIGHT).AddKey(Keys.Right);
            m_controlsConfig.GetFunctionConfig(Function.GAME_RIGHT).AddButton(Buttons.LeftThumbstickRight);
            m_controlsConfig.GetFunctionConfig(Function.GAME_DOWN).AddKey(Keys.Down);
            m_controlsConfig.GetFunctionConfig(Function.GAME_DOWN).AddButton(Buttons.LeftThumbstickDown);
            m_controlsConfig.GetFunctionConfig(Function.GAME_UP).AddKey(Keys.Up);
            m_controlsConfig.GetFunctionConfig(Function.GAME_UP).AddButton(Buttons.LeftThumbstickUp);
            m_controlsConfig.GetFunctionConfig(Function.GAME_ROTATE_L).AddKey(Keys.W);
            m_controlsConfig.GetFunctionConfig(Function.GAME_ROTATE_L).AddButton(Buttons.A);
            m_controlsConfig.GetFunctionConfig(Function.GAME_ROTATE_R).AddKey(Keys.X);
            m_controlsConfig.GetFunctionConfig(Function.GAME_ROTATE_R).AddButton(Buttons.B);
            m_controlsConfig.GetFunctionConfig(Function.GAME_MENU).AddKey(Keys.Enter);
            m_controlsConfig.GetFunctionConfig(Function.GAME_MENU).AddButton(Buttons.Start);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_LEFT).AddKey(Keys.Q);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_LEFT).AddButton(Buttons.DPadLeft);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_LEFT).AddButton(Buttons.LeftShoulder);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_LEFT).AddButton(Buttons.LeftTrigger);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_SELF).AddKey(Keys.S);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_SELF).AddButton(Buttons.DPadUp);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_SELF).AddButton(Buttons.DPadDown);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_RIGHT).AddKey(Keys.D);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_RIGHT).AddButton(Buttons.DPadRight);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_RIGHT).AddButton(Buttons.RightShoulder);
            m_controlsConfig.GetFunctionConfig(Function.GAME_BONUS_RIGHT).AddButton(Buttons.RightTrigger);

            m_controlsConfig.GetFunctionConfig(Function.MENU_LEFT).AddKey(Keys.Left);
            m_controlsConfig.GetFunctionConfig(Function.MENU_LEFT).AddButton(Buttons.LeftThumbstickLeft);
            m_controlsConfig.GetFunctionConfig(Function.MENU_RIGHT).AddKey(Keys.Right);
            m_controlsConfig.GetFunctionConfig(Function.MENU_RIGHT).AddButton(Buttons.LeftThumbstickRight);
            m_controlsConfig.GetFunctionConfig(Function.MENU_UP).AddKey(Keys.Up);
            m_controlsConfig.GetFunctionConfig(Function.MENU_UP).AddButton(Buttons.LeftThumbstickUp);
            m_controlsConfig.GetFunctionConfig(Function.MENU_DOWN).AddKey(Keys.Down);
            m_controlsConfig.GetFunctionConfig(Function.MENU_DOWN).AddButton(Buttons.LeftThumbstickDown);
            m_controlsConfig.GetFunctionConfig(Function.MENU_VALIDATE).AddKey(Keys.W);
            m_controlsConfig.GetFunctionConfig(Function.MENU_VALIDATE).AddButton(Buttons.A);
            m_controlsConfig.GetFunctionConfig(Function.MENU_CANCEL).AddKey(Keys.X);
            m_controlsConfig.GetFunctionConfig(Function.MENU_CANCEL).AddButton(Buttons.B);
            
            m_controlsConfig.GetFunctionConfig(Function.CHEAT_RESET_BOARD).AddKey(Keys.C);
            m_controlsConfig.GetFunctionConfig(Function.CHEAT_RESET_BOARD).AddButton(Buttons.Y);

            m_controlsConfig.GetFunctionConfig(Function.DEBUG_CONSOLE).AddKey(Keys.Tab);
            m_controlsConfig.GetFunctionConfig(Function.DEBUG_CONSOLE).AddButton(Buttons.Back);
            m_controlsConfig.GetFunctionConfig(Function.DEBUG_CREATE_SESSION).AddKey(Keys.F11);
            m_controlsConfig.GetFunctionConfig(Function.DEBUG_CREATE_SESSION).AddButton(Buttons.LeftStick);
            m_controlsConfig.GetFunctionConfig(Function.DEBUG_JOIN_SESSION).AddKey(Keys.F12);
            m_controlsConfig.GetFunctionConfig(Function.DEBUG_JOIN_SESSION).AddButton(Buttons.RightStick);

            // Register as a service
            Game.Services.AddService(typeof(IControlsProvider), this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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
