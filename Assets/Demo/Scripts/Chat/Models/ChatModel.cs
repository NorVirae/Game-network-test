using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatModel
{
    public Guid Id { get; set; }
    public string SenderPlayfabId { get; set; }
    public string ReceiverPlayfabId { get; set; }
    public string Content { get; set; }
    public Guid ChatRoomId { get; set; }
    public string MediaUrl { get; set; }

}
