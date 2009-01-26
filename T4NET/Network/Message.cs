using Microsoft.Xna.Framework.Net;

namespace T4NET.Network
{
    public abstract class Message
    {
        public abstract Protocol MessageId { get; }
    }
}
