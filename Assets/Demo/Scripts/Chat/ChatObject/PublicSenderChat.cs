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
    public Text SenderId;
    public void UpdateChat(string clientId, string message)
    {
        if (string.IsNullOrEmpty(clientId) && string.IsNullOrEmpty(message))
        {
            response.clientId = clientId;
            response.message = message;
            senderText.text = message;
            SenderId.text = clientId;
        }
        
    }
}
