namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrFeatureSet
{
    public List<string> TopLines { get; set; }
    public List<string> BottomLines { get; set; }
    public List<string> FullText { get; set; }
}