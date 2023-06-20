using IO.Ably.Realtime;
using Network.Chat;
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

        public Image connectionStatus;
        public Text connectionText;


        public GameObject publicChat;
        public GameObject privateChat;

        public GameObject publicChatScroll;
        public GameObject privateChatScroll;


        public short chatMessageId = 102;
        public short fetchChatRoomMessageId = 104;

        public InputField chatBox;
        public InputField publicChatBox;

        public AllChats allChats;
        public AllChatsPublic allPublicChats;

        public INetworkEventListener networkEventlistener;
        public MessageHandler messageHandler;

        private void Start()
        {
            Debug.Log("Called " + ChatManager.connectionColor);
            messageHandler = new MessageHandler();
            ChatManager.Init("_vRLkA.dtMWdw:vxlwHwwbRD6t_uP8Qu0b5ouI8xd63937moEWiuQhxSo");
            connectionStatus.color = ChatManager.connectionColor;
            connectionText.text = ChatManager.connectionStateText;

        }

        private void Update()
        {
            connectionStatus.color = ChatManager.connectionColor;
            connectionText.text = ChatManager.connectionStateText;
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
            ChatModel chatModel = new ChatModel { id = Guid.NewGuid(), msg = chatBox.text, chatroomid = GameManager.Instance.playerManager.currentChatroomid, senderid = GameManager.Instance.playerManager.userId, receiverid = "#2LUP9QJ8P0" };
            message = new ChatMessage
            {
                channelID = "frank",
                clientID = "joe",
                eventName = "chat:message",
                messageBody = chatModel
            };
            PushMessageToServer(chatMessageId, message, HandleLoadPrivateChatRoom);

            chatBox.text = "";

            LoadPrivateChatRoom();
        }

        public void LoadPrivateChatRoom()
        {
            privateChat.SetActive(true);
            publicChat.SetActive(false);

            privateChatScroll.SetActive(true);
            publicChatScroll.SetActive(false);
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

            PushMessageToServer(fetchChatRoomMessageId, message, HandleLoadPrivateChatRoom);

        }

        private void HandleLoadPrivateChatRoom(Datagram result)
        {

            Debug.Log("RESULT FROM MSG CALL "+result);
            ChatRoomBaseModelMessages data = SerializationHelper.Deserialize<ChatRoomBaseModelMessages>(result.body.ToString());
            Debug.Log("COUNT " + data.messageBody.chats.Count);

            allChats.ClearPreviousChats();

            if (data.messageBody.chats.Count > 0)
            {
                for (int i = 0; i < data.messageBody.chats.Count; i++)
                {
                    allChats.chats.Add(data.messageBody.chats[i]);
                    
                    Debug.Log(data.messageBody.chats + "data.chats");
                }
                allChats.SpawnChats();
            }
        }

        public async void LoadPublicChatRoom()
        {
            privateChat.SetActive(false);
            publicChat.SetActive(true);

            privateChatScroll.SetActive(false);
            publicChatScroll.SetActive(true);

            //load messages from ably history async

            ChatManager.SubscribeToChannel(GameManager.Instance.playerManager.currentChatroomid.ToString(), "chat:message");
            List<ChatMessageResponse> messages = await ChatManager.LoadChannelMessageHistory(GameManager.Instance.playerManager.currentChatroomid.ToString(), "chat:message");

            Debug.Log(messages + " MESSAGES LOAd");

            //publish message to public chat room
            allPublicChats.ClearPreviousChats();
            if (messages.Count > 0)
            {
                for (int i = 0; i < messages.Count; i++)
                {
                    allPublicChats.chats.Add(messages[i]);

                    Debug.Log("  " + messages[i].message + "   data.chats");
                }
                allPublicChats.SpawnChats();
            }
        }

        
        public void SendPublicMessage()
        {
            ChatManager.SubscribeToChannel(GameManager.Instance.playerManager.currentChatroomid.ToString(), "chat:message");
            ChatManager.PublishMessage(new Chat.ChatMessage {
                channelName =  GameManager.Instance.playerManager.currentChatroomid.ToString(),
                eventName=  "chat:message",
                message = publicChatBox.text 
            });
            Debug.Log(publicChatBox.text + " WAD MESSAGE SENTR");
            publicChatBox.text = "";
            LoadPublicChatRoom();
        }

        //{"type":4,
       // "key":null,
       // "body":
        //"{\"messageID\":102,
       // \"messageBody\":
           // {\"channelID\":\"0d494496-0329-4d3b-8fcc-8c9628a11fe6\",
        //\"chats\":[],
       // \"eventName\":\"chat:message\",\"messageBody\":{\"id\":\"0d494496-0329-4d3b-8fcc-8c9628a11fe6\",\"title\":\"Aisha\",\"topic\":null,\"description\":null,\"creatorid\":\"#2LUP9QJ8P0\",\"CreatedAt\":0.0,\"UpdatedAt\":0.0}}}","clientCallabckId":1}

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
