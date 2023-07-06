using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public class MessageHandler
    {

        public MessageHandler() {
            actionQueueUpdateFunc = new Dictionary<int, Action<MessageProxy>>();
            RegisterHandlers();
        }


        private Dictionary<int, Action<MessageProxy>> actionQueueUpdateFunc = new Dictionary<int, Action<MessageProxy>>();
        private Dictionary<short, Action<Datagram, Action<MessageProxy>>> _handlers = new Dictionary<short, Action<Datagram, Action<MessageProxy>>>();

        private int actionCount = 0;

        internal void HandleNetworkMessage(Datagram datagram)
        {
            Debug.Log(datagram.clientCallabckId + " CLIENT CALLBACK ");
            int index = Convert.ToInt32(datagram.clientCallabckId);
            if (actionQueueUpdateFunc.ContainsKey(index))
            {
                MessageProxy proxy = SerializationHelper.Deserialize<MessageProxy>(datagram.body.ToString());
                Debug.Log(proxy.messageID + " WHATEVER");
                Debug.Log(JsonUtility.ToJson(datagram) + "DATAGRAM");

                //actionQueueUpdateFunc[index]?.Invoke(proxy);
                _handlers[proxy.messageID]?.Invoke(datagram, actionQueueUpdateFunc[index]);
            }
            else
            {
                MessageProxy proxy = SerializationHelper.Deserialize<MessageProxy>(datagram.body.ToString());
                GameNetworkManager.Instance.OnNetworkMessage(proxy);
            }
        }

        internal void SendMessageToServer(short messageId, object messageBody, Action<MessageProxy> callabck)
        {
            MessageProxy messageProxy = new()
            {
                messageID = messageId,
                messageBody = messageBody,
            };
            int id = actionCount++;
            actionQueueUpdateFunc.Add(id, callabck);

            SendDataToServerInternal(EventType.Message, messageProxy, id);
        }

        internal void RegisterHandlers()
        {
            _handlers.Add(MessageEvents.SYSTEM_MESSAGE, handleChatMessage);
            _handlers.Add(MessageEvents.FETCH_CHATS_MESSAGES, handleChatMessage);
            _handlers.Add(MessageEvents.LOGIN_MESSAGE, handleLoginMessage);
            _handlers.Add(MessageEvents.CHAT_MESSAGE, handleChatMessage);
            _handlers.Add(MessageEvents.FETCH_CHAT_ROOMS, handleFetchChatRooms);
            _handlers.Add(MessageEvents.PLAYFAB_ADD_FRIEND, handleAddFriendMessage);
            _handlers.Add(MessageEvents.JOIN_PUBLIC_CHAT, handleAddFriendMessage);
        }

        internal void handleLoginMessage(Datagram datagram, Action<MessageProxy> callback)
        {
            MessageProxy proxy = SerializationHelper.Deserialize<MessageProxy>(datagram.body.ToString());
            callback.Invoke(proxy);
        }

        internal void handleChatMessage(Datagram datagram, Action<MessageProxy> callback)
        {
            MessageProxy proxy = SerializationHelper.Deserialize<MessageProxy>(datagram.body.ToString());
            callback.Invoke(proxy);
        }

        internal void handleFetchChatMessage(Datagram datagram, Action<MessageProxy> callback)
        {
            MessageProxy proxy = SerializationHelper.Deserialize<MessageProxy>(datagram.body.ToString());
            callback.Invoke(proxy);
        }

        internal void handleFetchChatRooms(Datagram datagram, Action<MessageProxy> callback)
        {
            MessageProxy proxy = SerializationHelper.Deserialize<MessageProxy>(datagram.body.ToString());
            callback.Invoke(proxy);
        }

        internal void handleAddFriendMessage(Datagram datagram, Action<MessageProxy> callback)
        {
            MessageProxy proxy = SerializationHelper.Deserialize<MessageProxy>(datagram.body.ToString());
            callback.Invoke(proxy);
        }

        internal void SendDataToServerInternal(EventType type, object message, int id = -1)
        {
            Datagram datagram = new Datagram(type, message, id);
            GameNetworkManager.Instance.client?.SendData(datagram);
        }
    }
}
