using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiverChat : MonoBehaviour
{
    public Text senderText;
    public Text senderId;
    private string receiverId;
    private Guid chatRoomId;

    public void UpdateChat(string text, string sender, string receiver, Guid chatRoom)
    {
        if(String.IsNullOrEmpty(text) && String.IsNullOrEmpty(sender) && String.IsNullOrEmpty(receiver) && chatRoom != null)
        {
            senderText.text = text;
            senderId.text = sender;
            receiverId = receiver;
            chatRoomId = chatRoom;
        }
        
    }
}
