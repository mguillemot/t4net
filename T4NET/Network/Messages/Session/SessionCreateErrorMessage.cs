namespace T4NET.Network.Messages.Session
{
    public class SessionCreateErrorMessage : Message
    {
        public override Protocol MessageId
        {
            get { return Protocol.SESSION_CREATE_ERROR; }
        }
    }
}