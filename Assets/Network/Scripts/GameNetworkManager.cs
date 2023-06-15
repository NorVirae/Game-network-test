using IO.Ably.Realtime;
using QNetLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Network
{
    public class GameNetworkManager : Singleton<GameNetworkManager>
    {
        public string IP = "127.0.0.1";
        public int port = 1137;
        public Client client;

        public short messageId = 102;
        public ChatMessage message;


        public INetworkEventListener networkEventlistener;
        public MessageHandler messageHandler;

        private void Start()
        {
            messageHandler = new MessageHandler();
            message = new ChatMessage()
            {
                channelID = "frank",
                clientID = "joe",
                eventName = "chat:message",
                messageBody = new {colos = "hash"}
            };
        }

        public void ConnectToServer()
        {
            ConnectToServer(IP, port);
        }

        public void ConnectToServer(string _ip, int _port)
        {
            client = new Client();
            NetDebug.NetDebugLog += NetLogCallback;
            client.Connect(_ip, _port);
        }

        public void PushMessageToServer(short messageId, Message message, Action<object> callback)
        {
            Debug.Log("Message going ooo");
            messageHandler.SendMessageToServer(102, message, callback);
        }

        public void PushMessageToServer()
        {
            PushMessageToServer(messageId, message, HandlePushMessageToServer);
        }

        private void HandlePushMessageToServer(object result)
        {
            Debug.Log(result);
        }

        public void Disconnect()
        {
            client?.Disconnect();
        }

        private void OnDestroy()
        {
            client?.Disconnect();
            
        }
        private void OnDisable()
        {
            client?.Disconnect();
        }


        internal void OnConnected()
        {
            Debug.Log("Was connected");
            networkEventlistener.OnConnected();
        }

        internal void OnDisconnted()
        {
            networkEventlistener?.OnDisconnected();
        }

        internal void OnNetworkMessage(MessageProxy message)
        {
            networkEventlistener?.OnNetworkMessage(message.messageID, message.messageBody);
        }


        public void NetLogCallback(string message, DebugLevel level)
        {
            Debug.Log(message);
        }
    }

}
