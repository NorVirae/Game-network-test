using Network.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class AllChatsPublic : MonoBehaviour
{
    public PublicSenderChat publicSenderChat;
    public PublicReceiverChat publicReceiverChat;
    private bool isLoaded = false;

    public List<ChatMessageResponse> chats = new List<ChatMessageResponse>();
    // Start is called before the first frame update
    void Start()
    {
        InnitializeList();

        Debug.Log(chats.Count + " COUNTS");


    }

    private void Update()
    {
        if (!isLoaded)
        {
            SpawnChats();

        }
    }

    private void InnitializeList()
    {
        // Collect chat into a list and list them out.
        chats.Add(new ChatMessageResponse
        {
            message = "Hello",
            clientId = "#Ompo"
        });
    }

    public void ClearPreviousChats()
    {
        chats.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SpawnChats()
    {

        for (int i = 0; i < chats.Count; i++)
        {
            if (chats[i].clientId == GameManager.Instance.playerManager.playfabId)
            {
                publicSenderChat.UpdateChat("You",chats[i].message);
                Instantiate(publicSenderChat, this.transform, false);
            }
            else
            {
                publicReceiverChat.UpdateChat(chats[i].clientId,chats[i].message);
                Instantiate(publicReceiverChat, this.transform, false);
            }

        }
        isLoaded = true;
    }
}
