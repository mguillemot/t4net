namespace T4NET
{
    public interface IControlsProvider
    {
        ControlsState CurrentState { get; }

        ControlsConfig CurrentConfig { get;  }
    }
}
