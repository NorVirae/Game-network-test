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
        
        private static void UpdateTitleID()
        {
            if(string.IsNullOrEmpty(PlayFabSettings.staticSettings.TitleId))
            {
                throw new Exception("Playfab title settings cannot be null! Please update the title Id in settings");
            }
        }

        public static void LoginWithCustomId(string Id, Action<bool, string> callabck)
        {
            var request = new LoginWithCustomIDRequest { CustomId = Id, CreateAccount = true};
            PlayFabClientAPI.LoginWithCustomID(request, 
                (LoginResult result) =>
                {
                    callabck?.Invoke(true,result.PlayFabId);
                },
                (PlayFabError error) =>
                {
                    callabck?.Invoke(false, error.ErrorMessage);
                }
            );
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

