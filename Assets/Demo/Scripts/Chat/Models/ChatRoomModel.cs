using System;
using System.Collections.Generic;


public class ChatRoomModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<string> UserIds { get; set; }

}

public class ChatRoomModelMessages: ChatRoomModel
{
    
   public List<ChatModel> chats { get; set; }
}
