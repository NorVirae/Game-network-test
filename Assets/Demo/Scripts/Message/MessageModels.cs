
using Network;
using IO.Ably.Realtime;
using System.Collections.Generic;
using System;

public class LoginMessage : Message
{
    public string UserId;
    public string PlayfabId;
}

public class SystemMessage : Message
{
    public SystemMessageType messageType;
    public string message;

}

public class ChatMessage : Message
{
    public Guid Id { get; set; }
    public string SenderPlayfabId { get; set; }
    public string ReceiverPlayfabId { get; set; }
    public string Content { get; set; }
    public Guid ChatRoomId { get; set; }
    public string MediaUrl { get; set; }
}

public class ChatRoomMessage : Message
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string SenderPlayfabId { get; set; }
    public string ReceiverPlayfabId { get; set; }
    public List<ChatModel> Chats { get; set;}

}

public class FriendRequestMessage: Message
{
    public string PlayfabId;
}

