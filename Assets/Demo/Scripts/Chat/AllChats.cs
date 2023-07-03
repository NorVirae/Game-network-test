using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AllChats : MonoBehaviour
{
    public SenderChat senderChat;
    public ReceiverChat receiverChat;
    public List<ChatModel> chats = new List<ChatModel>();
    private bool isLoaded = false;
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
        chats.Add(new ChatModel
        {
            Id = Guid.NewGuid(),
            SenderPlayfabId = Guid.NewGuid().ToString(),
            ReceiverPlayfabId = Guid.NewGuid().ToString(),
            Content = "hello dear!",
            ChatRoomId = Guid.NewGuid()
        });

        chats.Add(new ChatModel
        {
            Id = Guid.NewGuid(),
            SenderPlayfabId = Guid.NewGuid().ToString(),
            ReceiverPlayfabId = Guid.NewGuid().ToString(),
            Content = "hello dear!",
            ChatRoomId = Guid.NewGuid()
        });


        chats.Add(new ChatModel
        {
            Id = Guid.NewGuid(),
            SenderPlayfabId = Guid.NewGuid().ToString(),
            ReceiverPlayfabId = Guid.NewGuid().ToString(),
            Content = "hello dear!",
            ChatRoomId = Guid.NewGuid()
        });


        chats.Add(new ChatModel
        {
            Id = Guid.NewGuid(),
            SenderPlayfabId = Guid.NewGuid().ToString(),
            ReceiverPlayfabId = Guid.NewGuid().ToString(),
            Content = "hello dear!",
            ChatRoomId = Guid.NewGuid()
        });


        chats.Add(new ChatModel
        {
            Id = Guid.NewGuid(),
            SenderPlayfabId = Guid.NewGuid().ToString(),
            ReceiverPlayfabId = Guid.NewGuid().ToString(),
            Content = "hello dear!",
            ChatRoomId = Guid.NewGuid()
        });


        chats.Add(new ChatModel
        {
            Id = Guid.NewGuid(),
            SenderPlayfabId = Guid.NewGuid().ToString(),
            ReceiverPlayfabId = Guid.NewGuid().ToString(),
            Content = "hello dear!",
            ChatRoomId = Guid.NewGuid()
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
            if (chats[i].SenderPlayfabId == GameManager.Instance.playerManager.playfabId)
            {
                senderChat.UpdateChat(chats[i].Content, chats[i].SenderPlayfabId, chats[i].ReceiverPlayfabId, chats[i].ChatRoomId);
                Instantiate(senderChat, this.transform, false);
            }
            else
            {
                receiverChat.UpdateChat(chats[i].Content, chats[i].SenderPlayfabId, chats[i].ReceiverPlayfabId, chats[i].ChatRoomId);
                Instantiate(receiverChat, this.transform, false);
            }

        }

        isLoaded = true;
    }

}
