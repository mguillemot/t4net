using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

namespace T4NET
{
    public class ContolsState
    {
        private static readonly Buttons[] ALL_BUTTONS = new[]
                                                   {
                                                       Buttons.A,
                                                       Buttons.B,
                                                       Buttons.Back,
                                                       Buttons.BigButton,
                                                       Buttons.DPadDown,
                                                       Buttons.DPadLeft,
                                                       Buttons.DPadRight,
                                                       Buttons.DPadUp,
                                                       Buttons.LeftShoulder,
                                                       Buttons.LeftStick,
                                                       Buttons.LeftThumbstickDown,
                                                       Buttons.LeftThumbstickLeft,
                                                       Buttons.LeftThumbstickRight,
                                                       Buttons.LeftThumbstickUp,
                                                       Buttons.LeftTrigger,
                                                       Buttons.RightShoulder,
                                                       Buttons.RightStick,
                                                       Buttons.RightThumbstickDown,
                                                       Buttons.RightThumbstickLeft,
                                                       Buttons.RightThumbstickRight,
                                                       Buttons.RightThumbstickUp,
                                                       Buttons.RightTrigger,
                                                       Buttons.Start,
                                                       Buttons.X,
                                                       Buttons.Y
                                                   };

        private readonly List<Buttons> m_pressedButtons = new List<Buttons>();

        private readonly List<Keys> m_pressedKeys = new List<Keys>();
        private readonly List<Buttons> m_previouslyPressedButtons = new List<Buttons>();
        private readonly List<Keys> m_previouslyPressedKeys = new List<Keys>();
        private readonly List<Buttons> m_releasedButtons = new List<Buttons>();
        private readonly List<Keys> m_releasedKeys = new List<Keys>();
        private KeyboardState m_keyboardState;
        private GamePadState m_padState;

        public KeyboardState KeyboardState
        {
            get { return m_keyboardState; }
        }

        public List<Keys> PressedKeys
        {
            get { return m_pressedKeys; }
        }

        public List<Keys> ReleasedKeys
        {
            get { return m_releasedKeys; }
        }

        public GamePadState PadState
        {
            get { return m_padState; }
        }

        public List<Buttons> PressedButtons
        {
            get { return m_pressedButtons; }
        }

        public List<Buttons> ReleasedButtons
        {
            get { return m_releasedButtons; }
        }

        public void ComputeState(KeyboardState keyboardState, GamePadState padState)
        {
            // Compute newly pressed & relased key set
            m_keyboardState = keyboardState;
            m_pressedKeys.Clear();
            m_releasedKeys.Clear();
            foreach (Keys key in keyboardState.GetPressedKeys())
            {
                if (!m_previouslyPressedKeys.Contains(key))
                {
                    m_pressedKeys.Add(key);
                }
            }
            foreach (Keys key in m_previouslyPressedKeys)
            {
                if (keyboardState.IsKeyUp(key))
                {
                    m_releasedKeys.Add(key);
                }
            }
            m_previouslyPressedKeys.Clear();
            m_previouslyPressedKeys.AddRange(keyboardState.GetPressedKeys());

            // Compute pad state
            m_padState = padState;
            m_pressedButtons.Clear();
            m_releasedButtons.Clear();
            if (padState.IsConnected)
            {
                foreach (Buttons button in ALL_BUTTONS)
                {
                    if (padState.IsButtonDown(button) && !m_previouslyPressedButtons.Contains(button))
                    {
                        m_pressedButtons.Add(button);
                    }
                    else if (padState.IsButtonUp(button) && m_previouslyPressedButtons.Contains(button))
                    {
                        m_releasedButtons.Add(button);
                    }
                }
                m_previouslyPressedButtons.Clear();
                foreach (Buttons button in ALL_BUTTONS)
                {
                    if (padState.IsButtonDown(button))
                    {
                        m_previouslyPressedButtons.Add(button);
                    }
                }
            }
        }
    }
}