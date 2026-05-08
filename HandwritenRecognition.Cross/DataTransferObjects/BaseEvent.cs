namespace HandwritenRecognition.Cross.DataTransferObjects;

public class BaseEvent<T> where T : class
{
    public string Type { get; set; }
    public DateTime TimeStamp { get; set; }
    public T? Data { get; set; }
}