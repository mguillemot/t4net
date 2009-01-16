using System.Collections.Generic;

namespace T4NET.Controls
{
    public class ControlsConfig
    {
        private readonly Dictionary<Function, FunctionConfig> m_controls = new Dictionary<Function, FunctionConfig>();

        public ControlsConfig()
        {
            foreach (Function function in Functions.All)
            {
                m_controls[function] = new FunctionConfig();
            }
        }

        public FunctionConfig GetFunctionConfig(Function function)
        {
            return m_controls[function];
        }

        public bool IsPressed(Function function, ControlsState state)
        {
            return m_controls[function].IsPressed(state);
        }

        public bool JustPressed(Function function, ControlsState state)
        {
            return m_controls[function].JustPressed(state);
        }

        public bool JustReleased(Function function, ControlsState state)
        {
            return m_controls[function].JustReleased(state);
        }
    }
}