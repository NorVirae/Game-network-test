using Newtonsoft.Json;
using QNetLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Network
{
    public class Client
    {
        public SimpleWebClient webSocket;
        private ClientState state;

        public void Connect(string ip, int port)
        {
            webSocket = new SimpleWebClient();
            webSocket.onClientStateChange += State;
            webSocket.OnConnectionMessage += OnConnectionMessage;
            webSocket.Connect(ip, port);
        }

        public void Disconnect()
        {
            if (webSocket != null)
            {
                webSocket.Stop();
            }
        }

        private void OnConnectionMessage(byte[] payload, int lenght, EndPoint sender, DeliveryMethod deliveryMethod, Channel channel)
        {
            GameThreadHandler.ExecuteInLateUpdate(() =>
            {
                string msg = Encoding.ASCII.GetString(payload);
                Datagram datagram = JsonConvert.DeserializeObject<Datagram>(msg);
                NetworkDataHandler.HandleNetworkEvent(datagram);
            });
        }

        public void SendData(Datagram datagram)
        {
            datagram.key = "";
            string data = datagram.ToString();
            byte[] payload = Encoding.ASCII.GetBytes(data);
            webSocket.SendConnectionPayload(payload);
        }


        void State(ClientState clientState)
        {
            state = clientState;
            switch (clientState)
            {
                case ClientState.Disconnected:
                    GameNetworkManager.Instance.OnDisconnted();
                    break;
                case ClientState.Connected:
                    Debug.Log("Connected!; waiting for connection Approval");
                    break;
                case ClientState.ConnectionTimeout:
                    GameNetworkManager.Instance.OnDisconnted();
                    break;

            }
        }


    }
}


