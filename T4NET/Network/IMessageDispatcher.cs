namespace T4NET.Network
{
    public interface IMessageDispatcher
    {
        void DispatchMessage(Message message);

        void RegisterProcessor(Protocol interetedMessage, IMessageProcessor processor);

        void UnregisterProcessor(Protocol interetedMessage, IMessageProcessor processor);

        void UnregisterProcessor(IMessageProcessor processor);
    }
}
