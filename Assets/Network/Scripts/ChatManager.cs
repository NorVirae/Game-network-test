using IO.Ably;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        private static ChatConnectionState connectionState;

        public static event OnChatMesasage OnMesasage;

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
                    case IO.Ably.Realtime.ConnectionState.Initialized:
                        break;
                    case IO.Ably.Realtime.ConnectionState.Connecting:
                        connectionState = ChatConnectionState.Connecting;
                        break;
                    case IO.Ably.Realtime.ConnectionState.Connected:
                        connectionState = ChatConnectionState.Connected;
                        break;
                    case IO.Ably.Realtime.ConnectionState.Disconnected:
                    case IO.Ably.Realtime.ConnectionState.Suspended:
                    case IO.Ably.Realtime.ConnectionState.Closing:
                    case IO.Ably.Realtime.ConnectionState.Closed:
                    case IO.Ably.Realtime.ConnectionState.Failed:
                        connectionState = ChatConnectionState.Disconnected;
                        break;
                }

                _connected = args.Current == IO.Ably.Realtime.ConnectionState.Connected;
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

        private static async Task<List<ChatMessageResponse>> LoadChannelMessageHistory(string channelId, string _event)
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

        private static async void PublishMessage(ChatMessage message)
        {
            // async-await makes sure call is executed in the background and then result is posted on UnitySynchronizationContext/Main thread
            await _ably.Channels.Get(message.channelName).PublishAsync(message.eventName, message.message);
            
        }
    }

}

