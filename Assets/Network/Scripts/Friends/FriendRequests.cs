using Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendRequests : MonoBehaviour
{
    // Start is called before the first frame update
    public GameNetworkManager networkManager;
    public GameManager gameManager;
    public FriendsReqs friendReqs;
    private bool friendsListed = false;
    void Start()
    {
        
    }

    private void Update()
    {
        if (!string.IsNullOrEmpty(gameManager.playerManager.playfabId) && !friendsListed)
        {
            networkManager.GetFriendRequestList();
            friendsListed = true;
        }
    }

    public void ClearFriends()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }


}
