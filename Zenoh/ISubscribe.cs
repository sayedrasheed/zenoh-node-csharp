using Google.Protobuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenoh
{
    /// <summary>
    /// Represents the callback method when receiving the Protobuf message
    /// </summary>
    /// <param name="message">Received Protobuf message</param>
    public delegate void SubscriberCallback<T>(T message) where T : IMessage<T>;

    /// <summary>
    /// Defines a subscriber callback that performs an action when receiving of the message
    /// </summary>
    public interface ISubscribeCallback<T> where T : IMessage<T>
    {
        /// <summary>
        /// Callback for receiving of message
        /// </summary>
        SubscriberCallback<T> OnData { get; set; }
    }
}
