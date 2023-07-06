using Network;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class AllChats : MonoBehaviour
{
    public SenderChat senderChat;
    public ReceiverChat receiverChat;
    public List<ChatModel> chats = new List<ChatModel>();
    private bool isLoaded = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (!isLoaded)
        {
            Debug.Log("FRIM UPDATE");
            InnitializeList();
            isLoaded = true;
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
        SpawnChats();

    }

    public void ClearPreviousChats()
    {
        chats.Clear();
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void SpawnSingleChat(ChatModel chat)
    {
        chats.Add(chat);

        if (chat.SenderPlayfabId == GameManager.Instance.playerManager.playfabId)
        {
            Debug.Log(" YOU");
            SenderChat item = Instantiate(senderChat, this.transform).GetComponent<SenderChat>();
            item.UpdateChat(chat.Content, chat.SenderPlayfabId, chat.ReceiverPlayfabId, chat.ChatRoomId);

        }
        else
        {
            Debug.Log("THE OTHER");
            ReceiverChat item = Instantiate(receiverChat, this.transform).GetComponent<ReceiverChat>();
            item.UpdateChat(chat.Content, chat.SenderPlayfabId, chat.ReceiverPlayfabId, chat.ChatRoomId);

        }

    }

    public void SpawnChats()
    {
        
        Debug.Log("SPAWN CHATS CALLED");
        for (int i = 0; i < chats.Count; i++)
        {
            if (chats[i].SenderPlayfabId == GameManager.Instance.playerManager.playfabId)
            {
                Debug.Log(" YOU");
                SenderChat item = Instantiate(senderChat, this.transform).GetComponent<SenderChat>();
                item.UpdateChat(chats[i].Content, chats[i].SenderPlayfabId, chats[i].ReceiverPlayfabId, chats[i].ChatRoomId);

            }
            else
            {
                Debug.Log("THE OTHER");
                ReceiverChat item = Instantiate(receiverChat, this.transform).GetComponent<ReceiverChat>();
                item.UpdateChat(chats[i].Content, chats[i].SenderPlayfabId, chats[i].ReceiverPlayfabId, chats[i].ChatRoomId);

            }

        }

    }

}
