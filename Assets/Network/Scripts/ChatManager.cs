using IO.Ably;
using IO.Ably.Realtime;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Network.Chat
{
    public struct ChatMessageResponse
    {
        public string message;
        public string clientId;
    }

    public struct ChatMessage
    {
        public string channelName;
        public string eventName;
        public string message;
    }

    public static class ChatManager
    {
        public delegate void OnChatMesasage(ChatMessageResponse chatMessage);

        private static AblyRealtime _ably;
        private static ClientOptions _clientOptions;
        private static bool _connected;
        public static ChatConnectionState connectionState;
        public static string connectionStateText = "Disconnected";
        public static Color connectionColor = Color.red;


        public static event OnChatMesasage OnMesasage;

        //_vRLkA.dtMWdw:vxlwHwwbRD6t_uP8Qu0b5ouI8xd63937moEWiuQhxSo
        public static void Init(string apiKey)
        {
            _clientOptions = new ClientOptions
            {
                Key = apiKey,
                AutomaticNetworkStateMonitoring = false,
                AutoConnect = false,
                CustomContext = SynchronizationContext.Current
            };

            _ably = new AblyRealtime(_clientOptions);
            _ably.Connection.On(args => {

                switch (args.Current)
                {
                    case ConnectionState.Initialized:
                        break;
                    case ConnectionState.Connecting:
                        connectionState = ChatConnectionState.Connecting;
                        connectionStateText = "Connecting";
                        connectionColor = Color.yellow;
                        Debug.Log(connectionStateText);
                        break;
                    case ConnectionState.Connected:
                        connectionState = ChatConnectionState.Connected;
                        connectionStateText = "Connected";
                        Debug.Log(connectionStateText);

                        connectionColor = Color.green;

                        break;
                    case ConnectionState.Disconnected:
                    case ConnectionState.Suspended:
                    case ConnectionState.Closing:
                    case ConnectionState.Closed:
                    case ConnectionState.Failed:
                        connectionState = ChatConnectionState.Disconnected;
                        connectionStateText = "Disconnected";
                        Debug.Log(connectionStateText);

                        connectionColor = Color.red;
                        break;
                }

                _connected = args.Current == ConnectionState.Connected;
            });
        }

        public static void Connect(string clientId)
        {
            _clientOptions.ClientId= clientId;
            if(_connected)
            {
                _ably.Close();
            }
            else
            {
                _ably.Connect();
            }
        }


        public static void SubscribeToChannel(string channelId, string _event)
        {
            if(_connected)
            {
                _ably.Channels.Get(channelId).Subscribe(_event, message =>
                {
                    OnMesasage?.Invoke(new ChatMessageResponse
                    {
                        clientId = message.ClientId,
                        message = message.Data.ToString(),
                    });
                });
            }
        }

        public static void UnsubscribeToChannel(string channelId, string _event)
        {
            if (_connected)
            {
                _ably.Channels.Get(channelId).Unsubscribe(_event, message =>
                {
                    OnMesasage?.Invoke(new ChatMessageResponse
                    {
                        clientId = message.ClientId,
                        message = message.Data.ToString(),
                    });
                });
            }
        }

        public static string GetChannels()
        {
            var channelNames = string.Join(", ", _ably.Channels.Select(channel => channel.Name));
            return channelNames;
        }

        public static async Task<List<ChatMessageResponse>> LoadChannelMessageHistory(string channelId, string _event)
        {
            
            var historyPage = await _ably.Channels.Get(channelId).HistoryAsync();
            List<ChatMessageResponse> _messages = new List<ChatMessageResponse>();   
            while (true)
            {
                foreach (var message in historyPage.Items)
                {

                    _messages.Add(new ChatMessageResponse
                    {
                        clientId = message.ClientId,
                        message = message.Data.ToString(),
                    });
                }
                if (historyPage.IsLast)
                {
                    break;
                }
                historyPage = await historyPage.NextAsync();
            };
            return _messages;
            
        }

        public static async void PublishMessage(ChatMessage message)
        {
            // async-await makes sure call is executed in the background and then result is posted on UnitySynchronizationContext/Main thread
           Result res = await _ably.Channels.Get(message.channelName).PublishAsync(message.eventName, message.message);
            Debug.Log(res + "RESULT FROM MESG SEND");
        }
    }

}

