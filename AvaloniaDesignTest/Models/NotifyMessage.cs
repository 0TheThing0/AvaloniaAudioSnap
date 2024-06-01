namespace AvaloniaDesignTest.ViewModels;

public enum MessageType  {
    Info,
    Error,
    Success
}

public class NotifyMessage
{
    
    
    public MessageType MessageType { get; set; }
    public string Message { get; set; }

    public NotifyMessage(MessageType type, string message)
    {
        MessageType = type;
        Message = message;
    }
}