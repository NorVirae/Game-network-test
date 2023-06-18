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
            actionQueueUpdateFunc = new Dictionary<int, Action<Datagram>>();
            RegisterHandlers();
        }


        private Dictionary<int, System.Action<Datagram>> actionQueueUpdateFunc = new Dictionary<int, System.Action<Datagram>>();
        private Dictionary<short, System.Action<Datagram, Action<Datagram>>> _handlers = new Dictionary<short, System.Action<Datagram, Action<Datagram>>>();

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

        internal void SendMessageToServer(short messageId, object message, Action<Datagram> callabck)
        {
            MessageProxy messageProxy = new()
            {
                messageID = messageId,
                messageBody = message,
            };
            int id = actionCount++;
            Debug.Log("MAIN ID ADDED " + id);
            actionQueueUpdateFunc.Add(id, callabck);

            SendDataToServerInternal(EventType.Message, messageProxy, id);
        }

        internal void RegisterHandlers()
        {
            _handlers.Add(MessageEvents.FETCH_CHATS_MESSAGES, handlePrivateMessage);
            _handlers.Add(MessageEvents.LOGIN_MESSAGE, handlePrivateMessage);
            _handlers.Add(MessageEvents.CHAT_MESSAGE, handlePrivateMessage);
            _handlers.Add(MessageEvents.FETCH_CHAT_ROOMS, handlePrivateMessage);



        }

        internal void handlePrivateMessage(Datagram datagram, Action<Datagram> callback)
        {
            ChatRoomMessage proxy = SerializationHelper.Deserialize<ChatRoomMessage>(datagram.body.ToString());
            Console.WriteLine(proxy.channelID + " CHANNEL  ID");
            callback.Invoke(datagram);
        }
        internal void SendDataToServerInternal(EventType type, object message, int id = -1)
        {
            Datagram datagram = new Datagram(type, message, id);
            GameNetworkManager.Instance.client?.SendData(datagram);
        }
    }
}
