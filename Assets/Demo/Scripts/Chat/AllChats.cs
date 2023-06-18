using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllChats : MonoBehaviour
{
    public SenderChat senderChat;
    public ReceiverChat receiverChat;
    public List<ChatModel> chats = new List<ChatModel>();
    // Start is called before the first frame update
    void Start()
    {
        InnitializeList();

        Debug.Log(chats.Count + " COUNTS");

        for (int i = 0; i < chats.Count; i++)
        {
            if(i / 2 == 0)
            {
                senderChat.UpdateChat(chats[i].msg, chats[i].senderid, chats[i].receiverid, chats[i].chatroomid);
                Instantiate(senderChat, this.transform, false);
            }
            else
            {
                receiverChat.UpdateChat(chats[i].msg, chats[i].senderid, chats[i].receiverid, chats[i].chatroomid);
                Instantiate(receiverChat, this.transform, false);
            }
            
        }

    }

    private void InnitializeList()
    {
        // Collect chat into a list and list them out.
        chats.Add(new ChatModel
        {
            id = Guid.NewGuid(),
            senderid = Guid.NewGuid(),
            receiverid = Guid.NewGuid(),
            msg = "hello dear!",
            chatroomid = Guid.NewGuid()
        });

        chats.Add(new ChatModel
        {
            id = Guid.NewGuid(),
            senderid = Guid.NewGuid(),
            receiverid = Guid.NewGuid(),
            msg = "hello dear!",
            chatroomid = Guid.NewGuid()
        });


        chats.Add(new ChatModel
        {
            id = Guid.NewGuid(),
            senderid = Guid.NewGuid(),
            receiverid = Guid.NewGuid(),
            msg = "hello dear!",
            chatroomid = Guid.NewGuid()
        });


        chats.Add(new ChatModel
        {
            id = Guid.NewGuid(),
            senderid = Guid.NewGuid(),
            receiverid = Guid.NewGuid(),
            msg = "hello dear!",
            chatroomid = Guid.NewGuid()
        });


        chats.Add(new ChatModel
        {
            id = Guid.NewGuid(),
            senderid = Guid.NewGuid(),
            receiverid = Guid.NewGuid(),
            msg = "hello dear!",
            chatroomid = Guid.NewGuid()
        });


        chats.Add(new ChatModel
        {
            id = Guid.NewGuid(),
            senderid = Guid.NewGuid(),
            receiverid = Guid.NewGuid(),
            msg = "hello dear!",
            chatroomid = Guid.NewGuid()
        });
    }


    public void SpawnChats()
    {
        for (int i = 0; i < chats.Count; i++)
        {
            if (i / 2 == 0)
            {
                senderChat.UpdateChat(chats[i].msg, chats[i].senderid, chats[i].receiverid, chats[i].chatroomid);
                Instantiate(senderChat, this.transform, false);
            }
            else
            {
                receiverChat.UpdateChat(chats[i].msg, chats[i].senderid, chats[i].receiverid, chats[i].chatroomid);
                Instantiate(receiverChat, this.transform, false);
            }

        }
    }

}
