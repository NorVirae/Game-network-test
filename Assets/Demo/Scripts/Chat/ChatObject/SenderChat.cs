using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SenderChat : MonoBehaviour
{
    public Text senderText;
    public Text senderId;
    private string receiverId;
    private Guid chatRoomId;

    public void UpdateChat(string text, string sender, string receiver, Guid chatRoom)
    {
        senderText.text = text;
        senderId.text = sender;
        receiverId = receiver;
        chatRoomId = chatRoom;
    }
}
