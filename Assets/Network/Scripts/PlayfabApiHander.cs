using PlayFab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using PlayFab.ClientModels;
using System.Linq;
using ENet;

namespace Network
{
    public class PlayfabApiHander 
    {
       public enum FriendIdType { PlayFabId, Username, Email, DisplayName };
        private static void UpdateTitleID(string titleId)
        {
            if(string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                PlayFabSettings.staticSettings.TitleId = titleId;
            }
        }

        private static void UpdateDisplayName(string displayName)
        {
            PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
            {

                DisplayName = displayName
            }, result => {
                Debug.Log("The player's display name is now: " + result.DisplayName);
            }, error => Debug.LogError(error.GenerateErrorReport()));
        }
        public static void LoginWithCustomId(string Id, string titleId, string displayName, Action<bool, string> callabck)
        {
            Debug.Log(titleId + " TITLE ID " + PlayFabSettings.staticSettings.TitleId);

            UpdateTitleID(titleId);
            var request = new LoginWithCustomIDRequest { CustomId = Id, CreateAccount = true };
            PlayFabClientAPI.LoginWithCustomID(request,
                (LoginResult result) =>
                {
                    Debug.Log(result.ToString() + " ID");
                    UpdateDisplayName(displayName);
                    callabck?.Invoke(true, result.PlayFabId);
                },
                (PlayFabError error) =>
                {
                    callabck?.Invoke(false, error.ErrorMessage);
                }
            );

            Debug.Log(titleId + " TITLE ID " + PlayFabSettings.staticSettings.TitleId);

        }

        public static void GetFriendRequestList(string playfabId, Action<bool, Dictionary<string, string>> callback, params string[] key)
        {
            GetPlayerPublicData(playfabId, callback, key);
        }

        public static void GetPlayerPublicData(string playfabId, Action<bool, Dictionary<string, string>> callback, params string[] key)
        {
            var request = new GetUserDataRequest { PlayFabId = playfabId, Keys = new List<string>(key) };
            PlayFabClientAPI.GetUserData(request, (GetUserDataResult result) => 
            {
                if(result.Data!= null)
                {
                    Dictionary<string, string> r = result.Data.ToDictionary(x => x.Key, x => x.Value.Value);
                    callback?.Invoke(true, r);
                    return;
                }
                callback?.Invoke(false, null);
            }, (PlayFabError error) =>
            {
                callback?.Invoke(false, null);
            });
        }

        public static void SetPlayerPublicData(string playFabid, Dictionary<string, string> data, Action<bool> callback)
        {
            var request = new UpdateUserDataRequest { Data= data };
            PlayFabClientAPI.UpdateUserData(request, result =>
            {
                Debug.Log("Data set " + SerializationHelper.Serialize(result));
                callback(true);
            }, error =>
            {
                callback(false);
            });
        }
    }

}

