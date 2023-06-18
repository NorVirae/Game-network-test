using System;
using System.Collections.Generic;

public class ChatRoomModel
{
    public Guid id { get; set; }
    public string title { get; set; }
    public string topic { get; set; }
    public string description { get; set; }
    public string creatorid { get; set; }
}


public class ChatRoomModelMessages: ChatRoomModel
{
    
   public List<ChatModel> chats { get; set; }
}

public class ChatRoomBaseModelMessages
{
    public string messageID;
    public ChatRoomModelMessages messageBody;
}