using IO.Ably.Realtime;
using QNetLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Network
{
    public class GameNetworkManager : Singleton<GameNetworkManager>
    {
        public string IP = "127.0.0.1";
        public int port = 1137;
        public Client client;

        public short chatMessageId = 102;
        public short fetchChatRoomMessageId = 104;

        public InputField chatBox;
        public AllChats allChats;

        public INetworkEventListener networkEventlistener;
        public MessageHandler messageHandler;

        private void Start()
        {
            messageHandler = new MessageHandler();
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

        public void PushMessageToServer(short messageId, MessageProxy message, Action<Datagram> callback)
        {
            messageHandler.SendMessageToServer(messageId, message, callback);
        }

        //public void CreateChatRoom()
        

        public void PushMessageToServer()
        {
            ChatMessage message;
            ChatModel chatModel = new ChatModel { id = Guid.NewGuid(), msg = chatBox.text, chatroomid = Guid.NewGuid(), senderid = Guid.NewGuid(), receiverid = Guid.NewGuid() };
            message = new ChatMessage
            {
                channelID = "frank",
                clientID = "joe",
                eventName = "chat:message",
                messageBody = chatModel
            };
            PushMessageToServer(chatMessageId, message, HandleLoadChatRoom);

            chatBox.text = "";
        }

        public void LoadChatRoom()
        {
            ChatRoomMessage message;
            //Debug.Log(GameManager.Instance.playerManager.playfabId + " CREATOR " + GameManager.Instance.playerManager.currentChatroomid);
            ChatRoomModel chatRoomModel = new ChatRoomModel()
            {
                id = GameManager.Instance.playerManager.currentChatroomid,
                creatorid = GameManager.Instance.playerManager.userId,
                title = "Aisha"
            };
            message = new ChatRoomMessage
            {
                channelID = "frank",
                clientID = "joe",
                eventName = "chat:message",
                messageBody = chatRoomModel
            };

            PushMessageToServer(fetchChatRoomMessageId, message, HandleLoadChatRoom);

        }

        private void HandleLoadChatRoom(Datagram result)
        {

            Debug.Log(JsonUtility.ToJson(result));
            Debug.Log("RESULT FROM MSG CALL "+result.ToString() + result);
            ChatRoomMessage data = SerializationHelper.Deserialize<ChatRoomMessage>(result.body.ToString());


            allChats.chats = data.chats;
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
