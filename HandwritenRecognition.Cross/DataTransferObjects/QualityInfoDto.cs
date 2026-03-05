namespace HandwritenRecognition.Cross.DataTransferObjects;

public class QualityInfoDto
{
    public string Level { get; set; }
    public float BlurScore { get; set; }
    public List<string> Notes { get; set; }
    public float AvgConfidence { get; set; }
}