using System.Diagnostics.CodeAnalysis;

namespace HandwritenRecognition.Data.Entities;

public class OcrResult
{
    private Guid _resultId;
    private List<OcrLine> _lines = [];
    private List<ImageInfo> _imageInfo= [];
    private List<QualityData> _quality=[];
    private List<ProcessData> _processInfo=[];

    public Guid ResultId
    {
        get => _resultId;
        set => _resultId = value;
    }

    public List<OcrLine> Lines
    {
        get => _lines;
        set => _lines = value;
    }
    public  List<ImageInfo> ImageInfo
    {
        get => _imageInfo;
        [MemberNotNull(nameof(_imageInfo))] set => _imageInfo = value;
    }

    public  List<QualityData> Quality
    {
        get => _quality;
        [MemberNotNull(nameof(_quality))] set => _quality = value;
    }

    public  List<ProcessData> ProcessInfo
    {
        get => _processInfo;
        [MemberNotNull(nameof(_processInfo))] set => _processInfo = value;
    }
}