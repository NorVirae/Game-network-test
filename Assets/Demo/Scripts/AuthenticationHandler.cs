using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthenticationHandler : Singleton<AuthenticationHandler>
{
    public UserProfileCatch userProfile;
    public PlayerManager playerManager;
    //public GameObject playfabUILogin;
    //public GameObject gameServerLogin;


    private string serverKey = "ServerData";

    public void LoginToPlayfab()
    {
        try
        {
            if(string.IsNullOrEmpty(userProfile.userId))
            {
                userProfile.userId = HashtagFromId.Generate(Guid.NewGuid().ToString());
            }

            PlayfabApiHander.LoginWithCustomId(userProfile.userId, (succes, data) =>
            {
                if(succes)
                {
                    GameManager.Instance.playerManager.userId = userProfile.userId;
                    GameManager.Instance.playerManager.playfabId = data;
                    Debug.Log($"IP:{GameNetworkManager.Instance.IP}, PORT:{GameNetworkManager.Instance.port}");

                    GameNetworkManager.Instance.ConnectToServer(GameNetworkManager.Instance.IP, GameNetworkManager.Instance.port);
                    ChatConsole.Instance.ConnectChat();
                    Debug.Log($"Login Succes!; userId:{userProfile.userId}, playfabId:{data}");

                    GetPlayerServer();
                }
                else
                {
                    Debug.LogError($"Login Failed {data}");

                }
            });
        }catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void LoginWithGoogle()
    {

    }

    public void GetPlayerServer()
    {
        PlayfabApiHander.GetPlayerPublicData(GameManager.Instance.playerManager.playfabId, (success, data) =>
        {
            if(success)
            {
                if (data.ContainsKey(serverKey))
                {
                    ServerInfo info = SerializationHelper.Deserialize<ServerInfo>(data[serverKey]);
                    GameManager.Instance.playerManager.serverInfo = info;
                    //ProceedToJoinGameServer();
                }
                else
                {
                    
                    ServerInfo info = new ServerInfo
                    {
                        name = "Server 1",
                        id = Guid.NewGuid().ToString(),
                        ip = "127.0.0.1",
                        port = 1137
                    };

                    PlayfabApiHander.SetPlayerPublicData(GameManager.Instance.playerManager.playfabId, new Dictionary<string, string>
                    {
                        { serverKey , SerializationHelper.Serialize(info) }
                    }, _success =>
                    {
                        if(_success)
                        {
                            GameManager.Instance.playerManager.serverInfo = info;
                            //ProceedToJoinGameServer();
                        }
                    });
                }
            }
            else
            {
                Debug.Log("Failed to get the server data");
            }
        }, serverKey);
    }

   // private void ProceedToJoinGameServer()
    //{
      //  playfabUILogin.SetActive(false);
      //  gameServerLogin.SetActive(true);
    //}

    public void JoinGameServer()
    {
        GameManager.Instance.EnterGameServer();
    }
}
