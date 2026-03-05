namespace HandwritenRecognition.Data.Entities;

public class QualityData
{
    public Guid Id { get; set; }
    public string Level { get; set; }
    public float BlurScore { get; set; }
    public List<string> Notes { get; set; }

    public int OcrJobResultId { get; set; }
    public OcrResult Result { get; set; }
}