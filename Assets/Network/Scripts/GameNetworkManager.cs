using IO.Ably;
using IO.Ably.Realtime;
using Network.Chat;
using QNetLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using static Network.PlayfabApiHander;

namespace Network
{
    public class GameNetworkManager : Singleton<GameNetworkManager>
    {
        public string IP = "127.0.0.1";
        public int port = 1137;
        public Client client;

        public Image connectionStatus;
        public Text connectionText;

        public Image connectionStatusAuth;
        public Text connectionTextAuth;


        public GameObject publicChat;
        public GameObject privateChat;

        public GameObject publicChatInput;
        public GameObject privateChatInput;


        public InputField chatBox; 
        public InputField publicChatBox;


        public AllChats allChats;
        public AllChatsPublic allPublicChats;

        public INetworkEventListener networkEventlistener;
        public MessageHandler messageHandler;



        private void Start()
        {
            ChatManager.OnMesasage += HandleChatMessage;
            messageHandler = new MessageHandler();
            ChatManager.Init("_vRLkA.dtMWdw:vxlwHwwbRD6t_uP8Qu0b5ouI8xd63937moEWiuQhxSo");
            connectionStatus.color = ChatManager.connectionColor;
            connectionText.text = ChatManager.connectionStateText;

            connectionStatusAuth.color = ChatManager.connectionColor;
            connectionTextAuth.text = ChatManager.connectionStateText;

            //privateChat.SetActive(false);
            //publicChat.SetActive(true);

           //privateChatScroll.SetActive(false);
            //publicChatScroll.SetActive(true);

        }

        private void Update()
        {
            connectionStatus.color = ChatManager.connectionColor;
            connectionText.text = ChatManager.connectionStateText;

            connectionStatusAuth.color = ChatManager.connectionColor;
            connectionTextAuth.text = ChatManager.connectionStateText;
        }

        private void HandleChatMessage(ChatMessageResponse chatMessage)
        {
            // Handle the received chat message here
            string message = chatMessage.message;
            string clientId = chatMessage.clientId;
            LoadPublicChatRoom();

            //allPublicChats.SpawnSingleChat(chatMessage);
            Debug.Log("Spawned chat");
            // Perform any necessary actions with the chat message
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

        public void PushMessageToServer(short messageId, Message messageBody, Action<MessageProxy> callback)
        {
            messageHandler.SendMessageToServer(messageId, messageBody, callback);
        }

        //public void CreateChatRoom()
        

        public void SendPrivateChatToFriend(string friendPlayfabId)
        {
            if (!string.IsNullOrEmpty(chatBox.text))
            {
                Debug.Log("Its not empty");
                ChatMessage message;
                message = new ChatMessage
                {
                    Content = chatBox.text,
                    ChatRoomId = GameManager.Instance.playerManager.currentChatroomid,
                    SenderPlayfabId = GameManager.Instance.playerManager.playfabId,
                    ReceiverPlayfabId = friendPlayfabId
                };
                PushMessageToServer(MessageEvents.CHAT_MESSAGE, message, HandleLoadPrivateChatRoomMessages);

                chatBox.text = "";

                LoadPrivateChatRoom(friendPlayfabId);
            }
            
        }

        //Send public chat to room
        public void SendPublicMessageToChatRoom()
        {
            ChatManager.SubscribeToChannel(GameManager.Instance.playerManager.currentChatroomid.ToString(), "chat:message");
            ChatManager.PublishMessage(new Chat.ChatMessage
            {
                channelName = GameManager.Instance.playerManager.currentChatroomid.ToString(),
                eventName = "chat:message",
                message = publicChatBox.text
            });
            Debug.Log(publicChatBox.text + " WAD MESSAGE SENTR");
            publicChatBox.text = "";
            LoadPublicChatRoom();
        }

        public void CreatePublicChatRoom()
        {
            ChatRoomMessage message;


            message = new ChatRoomMessage
            {
                Id = GameManager.Instance.playerManager.currentChatroomid,
                SenderPlayfabId = GameManager.Instance.playerManager.playfabId,
                Name = "Zogo Group"
            };

            PushMessageToServer(MessageEvents.JOIN_PUBLIC_CHAT, message, ChatMessageHandlers.HandleCreatePublicChatRoom);

        }

        public void JoinPublicChatRoom()
        {
            ChatRoomMessage message = new ChatRoomMessage
            {
                Id = GameManager.Instance.playerManager.currentChatroomid,
                SenderPlayfabId = GameManager.Instance.playerManager.playfabId,
                Name = "Zogo Group",
            };

            PushMessageToServer(MessageEvents.JOIN_PUBLIC_CHAT, message, ChatMessageHandlers.HandleJoinPublicChatRoom);

        }

        public void AcceptPlayfabFriend( string playFabId)
        {
            FriendRequestMessage message = new FriendRequestMessage { PlayfabId = playFabId};
            AddFriend(FriendIdType.PlayFabId, playFabId, (success) =>
            {
                if (success)
                {
                    PushMessageToServer(MessageEvents.PLAYFAB_ACCEPT_FRIEND, message, HandleAcceptPlayfabFriend);
                    Debug.Log("user was added successfully");
                }
                else
                {
                    Debug.Log("Unable to accept friend request");
                }
            });
            
        }

        public void SendPlayfabFriendRequest(string playfabId)
        {
            FriendRequestMessage message;
            message = new FriendRequestMessage
            {
                PlayfabId = playfabId
            };
            Debug.Log(message + playfabId + " IDS");
            AddFriend(FriendIdType.PlayFabId, playfabId, (success) =>
            {
                if (success)
                {
                    PushMessageToServer(MessageEvents.PLAYFAB_ADD_FRIEND, message, ChatMessageHandlers.HandleSendPlayfabFriendRequest);
                    Debug.Log("user was added successfully");
                }
                else
                {
                    Debug.Log("Unable to add friend");
                }
            });
           
        }

        public void LoadPrivateChatRoom(string playfabId)
        {
            privateChat.SetActive(true);
            publicChat.SetActive(false);

            privateChatInput.SetActive(true);
            publicChatInput.SetActive(false);

            ChatRoomMessage message;
            //Debug.Log(GameManager.Instance.playerManager.playfabId + " CREATOR " + GameManager.Instance.playerManager.currentChatroomid

            message = new ChatRoomMessage
            {
                Id = GameManager.Instance.playerManager.currentChatroomid,
                SenderPlayfabId = GameManager.Instance.playerManager.playfabId,
                ReceiverPlayfabId = playfabId,

                Name = "Aisha"
            };

            Debug.Log("Load Private called! ");

            PushMessageToServer(MessageEvents.FETCH_CHATS_MESSAGES, message, HandleLoadPrivateChatRoomMessages);

        }

        public void HandleLoadPrivateChatRoomMessages(MessageProxy result)
        {

            Debug.Log("RESULT FROM MSG CALL " + result + JsonHelper.Serialize(result));
            ChatRoomMessage data = SerializationHelper.Deserialize<ChatRoomMessage>(result.messageBody.ToString());
            Debug.Log("RESULT FROM MSG CALL " + data.Name + " " + data.Id + " whola "+ data.Chats.Count);
            allChats.ClearPreviousChats();

            if (data.Chats.Count > 0)
            {
                for (int i = 0; i < data.Chats.Count; i++)
                {
                    allChats.chats.Add(data.Chats[i]);

                    Debug.Log(JsonHelper.Serialize(data.Chats[i]) + "data.chats");
                }
                allChats.SpawnChats();
            }
        }

        public async void LoadPublicChatRoom()
        {
            privateChat.SetActive(false);
            publicChat.SetActive(true);

            privateChatInput.SetActive(false);
            publicChatInput.SetActive(true);

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

        //Get friendRequest List

        public void GetFriendRequestList()
        {
            PlayfabApiHander.GetFriendRequestList(GameManager.Instance.playerManager.playfabId, GetFriendRequestListSuccess, new string[] { "Friends" });
        }

       private void GetFriendRequestListSuccess(bool success, Dictionary<string, string> resultData)
        {

            
            if (success)
            {
                try
                {
                    Dictionary<string, FriendRequestMessage> friendRequestObject = SerializationHelper.Deserialize<Dictionary<string, FriendRequestMessage>>(resultData["Friends"]);


                    foreach (KeyValuePair<string, FriendRequestMessage> kvp in friendRequestObject)
                    {
                        Debug.Log(kvp.Key + ": " + kvp.Value);
                    }
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message + " YOU Do not have any friends yet");
                }
                
            }
        }

        // Handlers
        public void HandleAcceptPlayfabFriend(MessageProxy message)
        {

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
