using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Google.Protobuf;

namespace Zenoh
{
    /// <summary>
    /// Class <c>Publisher</c> is for publishing Protobuf messages using Zenoh protocol
    /// </summary>
    public class Publisher<T> where T : IMessage<T>
    {
        public Session _session;
        internal string topic;

        /// <summary>
        /// Initializes a new instance of the <see cref="Publisher"/> class.
        /// </summary>
        /// <param name="topic">Topic name for this publisher</param>
        /// <param name="session">Zenoh session for this publisher</param>
        public Publisher(string topic, Session session)
        {
            this._session = session;
            this.topic = topic;
        }

        /// <summary>
        /// Publishes a Protobuf message
        /// </summary>
        /// <param name="message">Protobuf message to publish</param>
        public bool Publish(T message)
        {
            using (var memoryStream = new System.IO.MemoryStream())
            {
                message.WriteTo(memoryStream);
                byte[] serializedData = memoryStream.ToArray();

                unsafe
                {
                    int r;

                    fixed (byte* data = serializedData)
                    {
                        nuint len = (nuint)serializedData.Length;
                        nint pTopic = Marshal.StringToHGlobalAnsi(topic);
                        ZSession session = ZenohC.z_session_loan(_session.innerSession);
                        ZKeyexpr keyexpr = ZenohC.z_keyexpr((byte*)pTopic);
                        ZPutOptions options = ZenohC.z_put_options_default();
                        r = ZenohC.z_put(session, keyexpr, data, len, &options);

                        Marshal.FreeHGlobal(pTopic);
                    }

                    return r == 0;
                }
            }
        }
    }
}
