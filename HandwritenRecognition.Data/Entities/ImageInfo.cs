namespace HandwritenRecognition.Data.Entities;

public class ImageInfo
{
    public Guid Id { get; set; }
    public int OriginalWidth { get; set; }
    public int OriginalHeight { get; set; }
    public int ActualWidth { get; set; }
    public int ActualHeight { get; set; }

    public int OcrJobResultId { get; set; }
    public OcrResult Result { get; set; }
}