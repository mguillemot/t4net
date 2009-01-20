using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace T4NET.Network
{
    public class MessageDispatcherComponent : GameComponent, IMessageDispatcher
    {
        private readonly Queue<Message> m_incomingMessages = new Queue<Message>();
        private readonly Dictionary<Protocol, List<IMessageProcessor>> m_processorsByMessage = new Dictionary<Protocol, List<IMessageProcessor>>();
        private readonly Dictionary<IMessageProcessor, List<Protocol>> m_messagesByProcessor = new Dictionary<IMessageProcessor, List<Protocol>>();

        public MessageDispatcherComponent(Game game) 
            : base(game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            Game.Services.AddService(typeof(IMessageDispatcher), this);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            while (m_incomingMessages.Count > 0)
            {
                var message = m_incomingMessages.Dequeue();
                Dispatch(message);
            }
        }

        public void DispatchMessage(Message message)
        {
            m_incomingMessages.Enqueue(message);
        }

        private void Dispatch(Message message)
        {
            List<IMessageProcessor> processors;
            if (m_processorsByMessage.TryGetValue(message.MessageId, out processors))
            {
                foreach (var processor in processors)
                {
                    if (!processor.OnMessage(message))
                    {
                        break;
                    }
                }
            }
        }

        public void RegisterProcessor(Protocol interetedMessage, IMessageProcessor processor)
        {
            List<IMessageProcessor> processors;
            if (!m_processorsByMessage.TryGetValue(interetedMessage, out processors))
            {
                processors = new List<IMessageProcessor>();
                m_processorsByMessage[interetedMessage] = processors;
            }
            processors.Add(processor);
            //
            List<Protocol> messages;
            if (!m_messagesByProcessor.TryGetValue(processor, out messages))
            {
                messages = new List<Protocol>();
                m_messagesByProcessor[processor] = messages;
            }
            messages.Add(interetedMessage);
        }

        public void UnregisterProcessor(Protocol interetedMessage, IMessageProcessor processor)
        {
            List<IMessageProcessor> processors;
            if (m_processorsByMessage.TryGetValue(interetedMessage, out processors))
            {
                processors.Remove(processor);
            }
            //
            List<Protocol> messages;
            if (m_messagesByProcessor.TryGetValue(processor, out messages))
            {
                messages.Remove(interetedMessage);
            }
        }

        public void UnregisterProcessor(IMessageProcessor processor)
        {
            List<Protocol> messages;
            if (m_messagesByProcessor.TryGetValue(processor, out messages))
            {
                foreach (var message in messages)
                {
                    var processorsByMessage = m_processorsByMessage[message];
                    processorsByMessage.RemoveAll(messageProcessor => messageProcessor == processor);
                    if (processorsByMessage.Count == 0)
                    {
                        m_processorsByMessage.Remove(message);
                    }
                }
                m_messagesByProcessor.Remove(processor);
            }
        }
    }
}
