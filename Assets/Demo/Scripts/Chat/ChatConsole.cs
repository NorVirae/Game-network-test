using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using IO.Ably;
using IO.Ably.Realtime;
using System.Threading;
using System;
using UnityEngine.UI;
using TMPro;
using Network;

public class ChatConsole: Singleton<ChatConsole>
{
    // Variables
    private static string _apiKey = "_vRLkA.dtMWdw:vxlwHwwbRD6t_uP8Qu0b5ouI8xd63937moEWiuQhxSo";
    private AblyRealtime _ably;
    private ClientOptions _clientOptions;
    private Image _connectionStatus;
    public Button _connectButton;
    private bool _isConnected;
    private InputField _username;
    public ChatChannel _chatchannel;
    void Start()
    {
        InnitializeChatConsole();
        InnitializeUiComponents();

        _chatchannel = new ChatChannel(_ably);
        _chatchannel.InnitializeChannel();
    }

    private void InnitializeChatConsole()
    {
        _clientOptions = new ClientOptions
        {
            Key = _apiKey,
            // this will disable the library trying to subscribe to network state notifications
            AutomaticNetworkStateMonitoring = false,
            AutoConnect = false,
            // this will make sure to post callbacks on UnitySynchronization Context Main Thread
            CustomContext = SynchronizationContext.Current
        };

        _ably = new AblyRealtime(_clientOptions);

        _ably.Connection.On(args =>
        {
            Debug.Log($"Connection State is <b>{args.Current}</b>");
            //_connectionStatus.GetComponentInChildren<Text>().text = args.Current.ToString();
            var connectionStatusBtnImage = _connectionStatus.GetComponent<Image>();
            var connectionTextStatus = _connectionStatus.GetComponentInChildren<Text>();
            switch (args.Current)
            {
                case ConnectionState.Initialized:
                    connectionStatusBtnImage.color = Color.white;
                    connectionTextStatus.text = "Innitialised";
                    break;

                case ConnectionState.Connecting:
                    connectionStatusBtnImage.color = Color.gray;
                    connectionTextStatus.text = "Connecting";
                    break;

                case ConnectionState.Connected:
                    connectionStatusBtnImage.color = Color.green;
                    connectionTextStatus.text = "Connected";
                    break;

                case ConnectionState.Disconnected:
                    connectionStatusBtnImage.color = Color.yellow;
                    connectionTextStatus.text = "Disconnected";
                    break;

                case ConnectionState.Closing:
                    connectionStatusBtnImage.color = Color.yellow;
                    connectionTextStatus.text = "Closing";
                    break;

                case ConnectionState.Closed:
                case ConnectionState.Failed:
                case ConnectionState.Suspended:
                    connectionStatusBtnImage.color = Color.red;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();


            }
            _isConnected = args.Current == ConnectionState.Connected;

        });
    }

    private void InnitializeUiComponents()
    {
        _connectButton = GameObject.Find("ConnectButton").GetComponent<Button>();
        _connectionStatus = GameObject.Find("ConnectStatus").GetComponent<Image>();

        _username = GameObject.Find("UserNameField").GetComponent<InputField>();
        //_connectButton.onClick.AddListener(ConnectChat);
        Debug.Log(_connectionStatus);
    }

    public void ConnectChat()
    {
        Debug.Log(_username);
        _clientOptions.ClientId = _username.text;
        if (_isConnected)
        {
            _ably.Close();
        }
        else
        {
            _ably.Connect();
        }
    }
}
