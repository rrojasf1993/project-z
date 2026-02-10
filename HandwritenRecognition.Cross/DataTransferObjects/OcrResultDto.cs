namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrResultDto
{
    public List<OcrLineDto> Lines { get; set; } = new();
    public Guid DocumentId { get; set; }
    public ImageInfoDto ImageInfo { get; set; }
    public QualityInfoDto Quality { get; set; }
    public ProcessDto ProcessInfo { get; set; }
}

