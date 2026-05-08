namespace HandwritenRecognition.Data.Entities;

public class BaseEntity
{
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}