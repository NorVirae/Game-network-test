using Network;
using Network.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AuthenticationHandler : Singleton<AuthenticationHandler>
{
    public UserProfileCatch userProfile;
    public NetworkSettings networkSettings;

    public PlayerManager playerManager;

    public InputField userName;

    //public GameObject playfabUILogin;
    //public GameObject gameServerLogin;


    private string serverKey = "ServerData";

    public void LoginToPlayfab()
    {
        try
        {

            if (string.IsNullOrEmpty(userProfile.userId))
            {
                userProfile.userId = HashtagFromId.Generate(Guid.NewGuid().ToString());
            }

            PlayfabApiHander.LoginWithCustomId(userProfile.userId, networkSettings.titleID, userName.text, (succes, data) =>
            {
                if(succes)
                {
                    GameManager.Instance.playerManager.userId = userProfile.userId;
                    GameManager.Instance.playerManager.playfabId = data;
                    Debug.Log($"IP:{GameNetworkManager.Instance.IP}, PORT:{GameNetworkManager.Instance.port}");
                    ChatManager.Connect(data);

                    GameNetworkManager.Instance.ConnectToServer(GameNetworkManager.Instance.IP, GameNetworkManager.Instance.port);

                    GameNetworkManager.Instance.connectionStatus.color = ChatManager.connectionColor;
                    GameNetworkManager.Instance.connectionText.text = ChatManager.connectionStateText;
                   
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

   
    //this function fetches pending friendrequest
    public void FetchPendingFriendRequestsList(string playfabId)
    {
        PlayfabApiHander.GetFriendRequestList(playfabId, FetchPendingFriendsRequestListSuccess, "FRIEND_REQUEST");
    }

    //handle actions requiring FriendsRequestList
    public static void FetchPendingFriendsRequestListSuccess(bool success, Dictionary<string, string> friendRequestList)
    {

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
