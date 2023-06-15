
using Network;



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
    public string messageID;
    public object messageBody;
}