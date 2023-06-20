using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Network.Singleton<GameManager> , INetworkEventListener
{
    public PlayerManager playerManager;

    public void Start()
    {
        GameNetworkManager.Instance.networkEventlistener = this;
        playerManager = new PlayerManager();
    }




    public void EnterGameServer()
    {
        GameNetworkManager.Instance.networkEventlistener = this;
        GameNetworkManager.Instance.ConnectToServer(playerManager.serverInfo.ip, playerManager.serverInfo.port);
    }

    public void LogPlayerIn()
    {
        AuthenticationHandler.Instance.LoginToPlayfab();
    }

    public void OnConnected()
    {
        Debug.Log("Connected!");


        LoginMessage loginMessage = new LoginMessage();
        loginMessage.playfabId = playerManager.playfabId;
        loginMessage.userId = playerManager.userId;

        GameNetworkManager.Instance.PushMessageToServer(MessageEvents.LOGIN_MESSAGE, loginMessage, (data) =>
        {
            Debug.Log("Message Response; " + data.ToString());
        });
    }

    public void OnDisconnected()
    {
        Debug.Log("Disconnected!");
    }

    public void OnNetworkMessage(short messageId, object message)
    {
        Debug.Log(messageId + " " + message);

    }
}
