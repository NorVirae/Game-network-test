using System;

public class ChatRoomModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Topic { get; set; }
    public string Description { get; set; }
    public Guid UserId { get; set; }
}
