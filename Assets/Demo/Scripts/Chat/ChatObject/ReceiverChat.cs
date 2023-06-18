using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReceiverChat : MonoBehaviour
{
    public Text senderText;
    private Guid senderId;
    private Guid receiverId;
    private Guid chatRoomId;

    public void UpdateChat(string text, Guid sender, Guid receiver, Guid chatRoom)
    {
        senderText.text = text;
        senderId = sender;
        receiverId = receiver;
        chatRoomId = chatRoom;
    }
}
