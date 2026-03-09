namespace HandwritenRecognition.Cross.DataTransferObjects;

public class OcrResultDto
{
    public List<OcrLineDto> Lines { get; set; } = new();
    public Guid DocumentId { get; set; }
    public List<ImageInfoDto> ImageInfo { get; set; } = [];
    public List<QualityInfoDto> QualityInfo { get; set; } = [];
    public List<ProcessDto> ProcessData { get; set; } = [];
}