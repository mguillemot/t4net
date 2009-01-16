using T4NET.Controls;

namespace T4NET.Controls
{
    public interface IControlsProvider
    {
        ControlsState CurrentState { get; }

        ControlsConfig CurrentConfig { get;  }
    }
}