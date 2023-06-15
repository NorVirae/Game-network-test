
using Network;
using IO.Ably.Realtime;


public class LoginMessage : Message
{
    public string userId;
    public string playfabId;
}

public class SystemMessage : Message
{
    public SystemMessageType messageType;
    public string message;

}

public class ChatMessage : Message
{
    public string channelID;
    public string clientID;
    public string eventName;
    public object messageBody;
}