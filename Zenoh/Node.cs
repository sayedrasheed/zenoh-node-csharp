using Google.Protobuf;

namespace Zenoh
{
    /// <summary>
    /// Class <c>Node</c> wraps a Zenoh session with an easy publisher and subscriber interface
    /// </summary>
    public class Node : IDisposable
    {
        internal Session session;
        private bool disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="Node"/> class.
        /// </summary>
        /// <param name="config">Zenoh config</param>
        public Node(Config config)
        {
            session = new Session(config);
        }

        /// <summary>
        /// Returns a new instance of the <see cref="Publisher"/> class.
        /// </summary>
        /// <param name="topic">Topic name for the publisher</param>
        public Publisher<T> NewPublisher<T>(string topic) where T : IMessage<T>
        {
            return new Publisher<T>(topic, session);
        }

        /// <summary>
        /// Returns a new instance of the <see cref="Subscriber"/> class.
        /// </summary>
        public Subscriber NewSubscriber()
        {
            return new Subscriber(session);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                session.Dispose();
                disposed = true;
            }
            
        }

    }
}
