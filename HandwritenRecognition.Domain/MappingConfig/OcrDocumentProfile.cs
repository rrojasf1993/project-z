using AutoMapper;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Infrastructure;
using HandwritenRecognition.Data.Entities;

namespace HandwritenRecognition.Domain.MappingConfig;

public class OcrDocumentProfile : Profile
{
    public OcrDocumentProfile()
    {
        CreateMap<ImageInfoDto, ImageInfo>().ReverseMap();
        CreateMap<ProcessDto, ProcessData>()
            .ForMember(d=>d.Profile,src=>src.MapFrom(s=>s.ImageProfile)).ReverseMap();
        CreateMap<QualityInfoDto, QualityData>().ReverseMap();
        
        CreateMap<OcrLineDto, OcrLine>()
            .ForMember(d => d.BoundingBox,
                src => src.MapFrom<string>(s => SerializationHelper.Serialize(s.BoundingBox)))
            .ForMember(d => d.Confidence, src => src.MapFrom(s => s.ConfidenceScore))
            .ForMember(d => d.LineIndex, src => src.MapFrom(s => s.LineId))
            .ForMember(d => d.Status, src => src.MapFrom(s => s.ConfidenceStatus))
            .ForMember(d => d.OriginalText, src => src.MapFrom(s => s.Text))
            .ReverseMap();
        CreateMap<OcrJobDto, OcrJob>().ReverseMap();
        CreateMap<OcrResultDto, OcrDocumentDto>()
            .ForMember(d => d.Id, src => src.MapFrom(s => s.DocumentId))
            .ForMember(d => d.Lines, src => src.MapFrom(s => s.Lines))
            .ForMember(d => d.ConfidenceAvg, src => src.MapFrom(s => s.QualityInfo.FirstOrDefault().AvgConfidence)).ReverseMap();
        CreateMap<OcrDocumentDto, OcrDocument>()
            .ForMember(d => d.Lines, src => src.MapFrom(s => s.Lines))
            .ReverseMap();

        CreateMap<OcrResultDto, OcrResult>()
            .ForMember(d => d.ImageInfo, src => src.MapFrom(s => s.ImageInfo))
            .ForMember(d => d.Quality, src => src.MapFrom(s=>s.QualityInfo))
            .ForMember(d => d.ProcessInfo, src => src.MapFrom(s=>s.ProcessData));
    }
 }