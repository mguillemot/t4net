using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace T4NET
{
    public class ControlsConfig
    {
        private readonly Dictionary<Function, Controls> m_controls = new Dictionary<Function, Controls>();

        public ControlsConfig()
        {
            foreach (Function function in Functions.All)
            {
                m_controls[function] = new Controls();
            }
        }

        public Controls GetControls(Function function)
        {
            return m_controls[function];
        }

        public bool IsPressed(Function function, ContolsState state)
        {
            return m_controls[function].IsPressed(state);
        }

        public bool JustPressed(Function function, ContolsState state)
        {
            return m_controls[function].JustPressed(state);
        }

        public bool JustReleased(Function function, ContolsState state)
        {
            return m_controls[function].JustReleased(state);
        }
    }

    public enum Function
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        ROTATE_R,
        ROTATE_L,
        REGEN_PIECE
    }

    public static class Functions
    {
        public static readonly Function[] All =
            new[]
                {
                    Function.LEFT, Function.RIGHT, Function.UP, Function.DOWN, Function.ROTATE_R, Function.ROTATE_L,
                    Function.REGEN_PIECE
                };
    }

    public class Controls
    {
        private readonly List<Keys> m_keys = new List<Keys>();
        private readonly List<Buttons> m_buttons = new List<Buttons>();

        public void AddKey(Keys key)
        {
            if (!m_keys.Contains(key))
            {
                m_keys.Add(key);
            }
        }

        public void RemoveKey(Keys key)
        {
            m_keys.Remove(key);
        }

        public void AddButton(Buttons button)
        {
            if (!m_buttons.Contains(button))
            {
                m_buttons.Add(button);
            }
        }

        public void RemoveButton(Buttons button)
        {
            m_buttons.Remove(button);
        }

        internal bool IsPressed(ContolsState state)
        {
            foreach (var key in m_keys)
            {
                if (state.KeyboardState.IsKeyDown(key))
                {
                    return true;
                }
            }
            foreach (var button in m_buttons)
            {
                if (state.PadState.IsButtonDown(button))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool JustPressed(ContolsState state)
        {
            foreach (var key in m_keys)
            {
                if (state.PressedKeys.Contains(key))
                {
                    return true;
                }
            }
            foreach (var button in m_buttons)
            {
                if (state.PressedButtons.Contains(button))
                {
                    return true;
                }
            }
            return false;
        }

        internal bool JustReleased(ContolsState state)
        {
            foreach (var key in m_keys)
            {
                if (state.ReleasedKeys.Contains(key))
                {
                    return true;
                }
            }
            foreach (var button in m_buttons)
            {
                if (state.ReleasedButtons.Contains(button))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
