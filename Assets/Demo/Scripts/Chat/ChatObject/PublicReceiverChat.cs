using Network.Chat;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PublicReceiverChat : MonoBehaviour
{
    public ChatMessageResponse response;
    public Text receiverText;
    public void UpdateChat(string clientId, string message)
    {
        response.clientId = clientId;
        response.message = message;
        receiverText.text = message;
    }
}
