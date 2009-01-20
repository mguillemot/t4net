namespace T4NET.Network
{
    public interface IMessageProcessor
    {
        bool OnMessage(Message message);
    }
}
