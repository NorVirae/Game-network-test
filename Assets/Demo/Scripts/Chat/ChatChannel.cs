using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO.Ably;
using Assets.Ably.Examples.Chat;
using UnityEngine.UI;


public class ChatChannel
{
    // Start is called before the first frame update
    private readonly AblyRealtime _ably;
    private string _channelName = "quivChat";
    private string _eventName = "gamegang";
    private InputField _messagePayload;
    private Button _sendButton;
    private Button _loadMessagesBtn;

    public ChatChannel(AblyRealtime ably)
    {
        _ably = ably;
    }


    public void InnitializeChannel()
    {
        _sendButton = GameObject.Find("SendButton").GetComponent<Button>();
        _messagePayload = GameObject.Find("Payload").GetComponent<InputField>();
        _loadMessagesBtn = GameObject.Find("LoadMessages").GetComponent<Button>();
        _sendButton.onClick.AddListener(SendMessage);
        _loadMessagesBtn.onClick.AddListener(fetchAllMessages);
        SubscribeToChannel();
    }
    public async void  SendMessage()
    {
        var channelName = _channelName;
        var eventName = _eventName;
        var payload = _messagePayload.text;
        // async-await makes sure call is executed in the background and then result is posted on UnitySynchronizationContext/Main thread
        var result = await _ably.Channels.Get(channelName).PublishAsync(eventName, payload);
        Debug.Log("Send Message");
    }

    public void SubscribeToChannel()
    {
        var channelName = _channelName;
        var eventName = _eventName;
        _ably.Channels.Get(channelName).Subscribe(eventName, message =>
        {
            Debug.Log($"Received message <b>{message.Data}</b> from channel <b>{channelName}</b>");
        });
        Debug.Log($"Successfully subscribed to channel <b>{channelName}</b>");
    }

    public async void fetchAllMessages() // Remember to modify function to fetch message history from db
    {
        var channelName = _channelName;
        Debug.Log($"#### <b>{channelName}</b> ####");
        var historyPage = await _ably.Channels.Get(channelName).HistoryAsync();
        while (true)
        {
            foreach (var message in historyPage.Items)
            {
                Debug.Log(message.Data.ToString());
            }
            if (historyPage.IsLast)
            {
                break;
            }
            historyPage = await historyPage.NextAsync();
        };
        Debug.Log($"#### <b>{channelName}</b> ####");
    }


}
