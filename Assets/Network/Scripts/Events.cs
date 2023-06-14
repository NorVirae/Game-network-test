using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Network
{
    public interface IEvent
    {
        EventType type { get; }
    }

    public struct LoginEvent : IEvent
    {
        [JsonIgnore]public EventType type => EventType.Login;
        public string token;
        public string userId;
    }

    public struct PongEvent : IEvent
    {
        [JsonIgnore] public EventType type => EventType.Pong;
    }


    public struct MessageEvent : IEvent
    {
        [JsonIgnore] public EventType type => EventType.Message;
        public object body;
        public object id;
    }
}
