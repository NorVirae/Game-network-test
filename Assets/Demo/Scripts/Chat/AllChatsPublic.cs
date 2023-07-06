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
    public List<GameObject> chatsPrefabObjects = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        InnitializeList();
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

    public void SpawnSingleChat(ChatMessageResponse chat)
    {
        chats.Add(chat);

        if (chat.clientId == GameManager.Instance.playerManager.playfabId)
        {
            PublicSenderChat item = Instantiate(publicSenderChat, this.transform).GetComponent<PublicSenderChat>();
            item.UpdateChat("You", chat.message);
        }
        else
        {
            PublicReceiverChat item = Instantiate(publicReceiverChat, this.transform).GetComponent<PublicReceiverChat>();
            item.UpdateChat(chat.clientId, chat.message);
        }

    }

    public void SpawnChats()
    {

        for (int i = 0; i < chats.Count; i++)
        {
            if (chats[i].clientId == GameManager.Instance.playerManager.playfabId)
            {
                PublicSenderChat item = Instantiate(publicSenderChat, this.transform);
                item.UpdateChat("You", chats[i].message);
            }
            else
            {
                PublicReceiverChat item = Instantiate(publicReceiverChat, this.transform);
                item.UpdateChat(chats[i].clientId, chats[i].message);
            }

        }
        isLoaded = true;
    }
}
