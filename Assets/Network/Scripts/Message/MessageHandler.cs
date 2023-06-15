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
            actionQueueUpdateFunc = new Dictionary<int, Action<object>>();
        }


        private Dictionary<int, System.Action<object>> actionQueueUpdateFunc = new Dictionary<int, System.Action<object>>();
        private int actionCount = 0;

        internal void HandleNetworkMessage(Datagram datagram)
        {
            Debug.Log($"Network message: {datagram}");
            int index = Convert.ToInt32(datagram.clientCallabckId);
            if (actionQueueUpdateFunc.ContainsKey(index))
            {
                MessageProxy proxy = SerializationHelper.Deserialize<MessageProxy>(datagram.body.ToString());
                actionQueueUpdateFunc[(int)datagram.clientCallabckId]?.Invoke(proxy);
            }
            else
            {
                MessageProxy proxy = SerializationHelper.Deserialize<MessageProxy>(datagram.body.ToString());
                GameNetworkManager.Instance.OnNetworkMessage(proxy);
            }
        }

        internal void SendMessageToServer(short messageId, object message, Action<object> callabck)
        {
            Debug.Log("message ID "+ messageId);
            MessageProxy messageProxy = new()
            {
                messageID = messageId,
                messageBody = message,
            };
            int id = actionCount++;
            actionQueueUpdateFunc.Add(id, callabck);
            Debug.Log("Message Send ooo! " + SerializationHelper.Serialize(messageProxy));

            SendDataToServerInternal(EventType.Message, messageProxy, actionCount++);
        }

        internal void SendDataToServerInternal(EventType type, object message, int id = -1)
        {
            Datagram datagram = new Datagram(type, message, id);
            GameNetworkManager.Instance.client?.SendData(datagram);
        }
    }
}
