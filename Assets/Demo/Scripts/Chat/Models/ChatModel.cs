using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatModel
{
    public Guid id { get; set; }
    public string senderid { get; set; }
    public string receiverid { get; set; }
    public string msg { get; set; }
    public Guid chatroomid { get; set; }

}
