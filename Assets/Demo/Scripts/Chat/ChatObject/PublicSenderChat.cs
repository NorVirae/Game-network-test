using Network.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicSenderChat : MonoBehaviour
{
    public ChatMessageResponse response;
    public Text senderText;

    public void UpdateChat(string clientId, string message)
    {
        response.clientId = clientId;
        response.message = message;
        senderText.text = message;
    }
}
