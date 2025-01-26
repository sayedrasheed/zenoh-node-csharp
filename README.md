# Zenoh Node C#
C# port of zenoh-node-rs. This library is integrated into my ninja-trader-server application to enable communication with TradeBot Rust application via the Zenoh protocol. NinjaTrader only offers a .NET DLL to use their API so I chose to write a C# implementation of Zenoh Node to communicate between the two applications.

# Installation
The Zenoh C API version 0.10.0-rc needs to be installed. 
1. Download the release here: [zenohc-0.10.0-rc](https://github.com/eclipse-zenoh/zenoh-c/releases/download/0.10.0-rc/zenoh-c-0.10.0-rc-x86_64-pc-windows-msvc.zip)
2. Extract zip
3. There should be a DLL in the extracted zip. Add that path to your PATH environment variable
4. You should now be able to build the library using Visual Studio

# Usage
NinjaTraderServer integration of this library can be found here:
https://github.com/sayedrasheed/ninja-trader-server

Minimal example
```
﻿using Zenoh;

 // Create node using some network string
string network = "'224.0.0.224:9000'";
Config config = new Config(network);
var node = new Node(config);

// Create subscriber using created node
Subscriber subscriber = node.NewSubscriber();

// Create my subscriber object
MySubscriber mySubscriber = new MySubscriber();

// Subscribe to MyProtobufMessage
subscriber.subscribe<MyProtobufMessage>("my_protobuf_message_topic", mySubscriber);

// Create publisher of MyProtobufMessage
Publisher<MyProtobufMessage> publisher = node.NewPublisher<MyProtobufMessage>("my_protobuf_message_topic");

// Publish message
var msg = new MyProtobufMessage();
publisher.Publish(msg);

class MySubscriber : ISubscribeCallback<MyProtobufMessage>
{
   public SubscriberCallback<MyProtobufMessage> OnData { get; set; }

   public MySubscriber()
   {
     OnData = OnDataCallback;
   }
   public void OnDataCallback(MyProtobufMessage message)
   {
     Console.WriteLine("MyProtobufMessage message received");
   }
}
```
