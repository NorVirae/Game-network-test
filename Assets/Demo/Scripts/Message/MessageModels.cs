
using Network;
using IO.Ably.Realtime;
using System.Collections.Generic;

public class LoginMessage : MessageProxy
{
    public string userId;
    public string playfabId;
}

public class SystemMessage : MessageProxy
{
    public SystemMessageType messageType;
    public string message;

}

public class ChatMessage : MessageProxy
{
    public string channelID;
    public string clientID;
    public string eventName;
    public ChatModel messageBody;
}

public class ChatRoomMessage : MessageProxy
{
    public string channelID;
    public string clientID;
    public string eventName;
    public ChatRoomModel messageBody;
    public List<ChatModel> chats;
}