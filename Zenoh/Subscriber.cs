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
    /// Class <c>Subscriber</c> is for subscriber to Protobuf messages using Zenoh protocol
    /// </summary>
    public class Subscriber : IDisposable
    {
        Session _session;
        private bool disposed;
        internal unsafe List<IDisposable> subscribes;

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscriber"/> class.
        /// </summary>
        /// <param name="session">Zenoh session for this subscriber</param>
        public Subscriber(Session session) 
        {
            _session = session;
            subscribes = new List<IDisposable>();

        }

        /// <summary>
        /// Subscribes to a message with the topic name
        /// </summary>
        /// <param name="topic">Topic name for this message</param>
        /// <param name="subscribeCallback">Object that implements the subscribe interface for this message type</param>
        public unsafe void subscribe<T>(string topic, ISubscribeCallback<T> 
            subscribeCallback) where T : IMessage<T>, new()
        {
            Subscribe<T> subscriber = new Subscribe<T>(topic, _session, subscribeCallback);
            subscribes.Add(subscriber);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;
                for (int i = 0; i < subscribes.Count; i++) { subscribes[i].Dispose(); }
            }
            
        }
    }

    /// <summary>
    /// Class <c>Subscribe</c> is for subscriber a single Protobuf message. It wraps the Zenoh C API subscriber
    /// functionality
    /// </summary>
    class Subscribe<T> : IDisposable where T : IMessage<T>, new()
    {
        internal readonly unsafe ZOwnedClosureSample* closureSample;
        internal unsafe ZOwnedSubscriber* ownedSubscriber;
        private readonly ZOwnedClosureSample ownedClosureSample;
        private GCHandle callbackGcHandle;
        private bool disposed;
        private ISubscribeCallback<T> subscriber;

        /// <summary>
        /// Initializes a new instance of the <see cref="Subscribe"/> class.
        /// </summary>
        /// <param name="topic">Topic name for this message</param>
        /// <param name="session">Zenoh session for this subscriber</param>
        /// <param name="callback">Callback object to execute when receiving this message</param>
        public unsafe Subscribe(string topic, Session session, ISubscribeCallback<T> callback)
        {
            callbackGcHandle = GCHandle.Alloc(callback.OnData, GCHandleType.Normal);
            ownedClosureSample = new ZOwnedClosureSample
            {
                context = (void*)GCHandle.ToIntPtr(callbackGcHandle),
                call = ExecuteCallback,
                drop = null,
            };

            nint p = Marshal.AllocHGlobal(Marshal.SizeOf(ownedClosureSample));
            Marshal.StructureToPtr(ownedClosureSample, p, false);
            closureSample = (ZOwnedClosureSample*)p;

            ZSession zSession = ZenohC.z_session_loan(session.innerSession);
            nint pTopic = Marshal.StringToHGlobalAnsi(topic);
            ZKeyexpr keyexpr = ZenohC.z_keyexpr((byte*)pTopic);

            ZOwnedSubscriber sub =
                ZenohC.z_declare_subscriber(zSession, keyexpr, closureSample, null);
            Marshal.FreeHGlobal(pTopic);


            nint pOwnedSubscriber = Marshal.AllocHGlobal(Marshal.SizeOf<ZOwnedSubscriber>());
            Marshal.StructureToPtr(sub, pOwnedSubscriber, false);
            ownedSubscriber = (ZOwnedSubscriber*)pOwnedSubscriber;
            subscriber = callback;
        }

        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposed) return;

            unsafe
            {
                Marshal.FreeHGlobal((nint)closureSample);
                Marshal.FreeHGlobal((nint)ownedSubscriber);
            }

            callbackGcHandle.Free();
            disposed = true;
        }

        /// <summary>
        /// Callback to execute when receiving a message with the Zenoh C API.
        /// </summary>
        /// <param name="sample">Zenoh message payload</param>
        /// <param name="context">Subscriber callback context</param>
        private static unsafe void ExecuteCallback(ZSample* sample, void* context)
        {
            var gch = GCHandle.FromIntPtr((nint)context);
            var callback = (SubscriberCallback<T>?)gch.Target;
            
            if (callback != null)
            {
                var len = sample->payload.len;
                byte[] data = new byte[len];
                Marshal.Copy((nint)sample->payload.start, data, 0, (int)len);

                T message = new T();
                using (var stream = new MemoryStream(data))
                {
                    message.MergeFrom(stream);
                }

                callback(message);
            }
        }
    }


}
