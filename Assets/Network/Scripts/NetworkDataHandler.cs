using Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using EventType = Network.EventType;

namespace Network
{
    public static class NetworkDataHandler
    {
        public static void HandleNetworkEvent(Datagram datagram)
        {
            EventType type = (EventType)Convert.ToInt32(datagram.type);
            
            switch (type)
            {
                case EventType.Connection:
                    Debug.Log(type);
                    Debug.Log(SerializationHelper.Serialize(datagram.body));
                    GameNetworkManager.Instance.OnConnected();
                    break;
                case EventType.Ping:
                case EventType.Pong:
                    GameNetworkManager.Instance.messageHandler.SendDataToServerInternal(EventType.Ping, 0);
                    break;
                case EventType.Message:
                     GameNetworkManager.Instance.messageHandler.HandleNetworkMessage(datagram);
                    break;
                case EventType.Disconnection:
                    GameNetworkManager.Instance.OnDisconnted();
                    break;
            }
        }
    }
}
