using AutoMapper;
using HandwritenRecognition.Cross;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Cross.Infrastructure;
using HandwritenRecognition.Data.Entities;

namespace HandwritenRecognition.Domain.MappingConfig;

public class OcrDocumentProfile : Profile
{
    public OcrDocumentProfile()
    {
        CreateMap<OcrBoundingBoxDto, OcrBoundingBox>().ReverseMap();
        CreateMap<ImageInfoDto, ImageInfo>().ReverseMap();
        CreateMap<ProcessDto, ProcessData>()
            .ForMember(d=>d.Profile,src=>src.MapFrom(s=>s.ImageProfile)).ReverseMap();
        CreateMap<QualityInfoDto, QualityData>().ReverseMap();

        CreateMap<OcrLineDto, OcrLine>()
            .ForMember(d => d.BoundingBox,
                src => src.MapFrom<string>(s => SerializationHelper.Serialize(s.BoundingBox)))
            .ForMember(d => d.Confidence, src => src.MapFrom(s => s.ConfidenceScore))
            .ForMember(d => d.LineIndex, src => src.MapFrom(s => s.LineIndex))
            .ForMember(d => d.Status, src => src.MapFrom(s => s.ConfidenceStatus))
            .ForMember(d => d.OriginalText, src => src.MapFrom(s => s.OriginalText))
            .ForMember(d => d.CorrectedText, src => src.MapFrom(s => s.CorrectedText))
            .ForMember(d => d.BoundingBoxes, src => src.MapFrom(s => s.FrontendFriendlyBoxes));

        CreateMap<OcrLine, OcrLineDto>()
            .ForMember(d => d.BoundingBox,
                src => src.MapFrom<List<List<int>>>(s =>
                    SerializationHelper.Deserialize<List<List<int>>>((s.BoundingBox))))
            .ForMember(d => d.ConfidenceScore, src => src.MapFrom(s => s.Confidence))
            .ForMember(d => d.ConfidenceStatus, src => src.MapFrom(s => s.Status))
            .ForMember(d => d.LineId, src => src.MapFrom(s => s.Id))
            .ForMember(d => d.LineIndex, src => src.MapFrom(s => s.LineIndex))
            .ForMember(d => d.OriginalText, src => src.MapFrom(s => s.OriginalText))
            .ForMember(d => d.CorrectedText, src => src.MapFrom(s => s.CorrectedText))
            .ForMember(d => d.FrontendFriendlyBoxes, src => src.MapFrom(s => s.BoundingBoxes));


        CreateMap<OcrJobDto, OcrJob>();
        CreateMap<OcrJob, OcrJobDto>()
            .ForMember(s => s.Status, dst => dst.MapFrom(s => s.Status))
            .ForMember(s => s.Error, dst => dst.MapFrom(s => s.Error))
            .ForMember(s => s.JobId, dst => dst.MapFrom(s => s.JobId))
            .ForMember(s => s.Result, dst => dst.MapFrom(s => s.Result));
        
        CreateMap<OcrResultDto, OcrDocumentDto>()
            .ForMember(d => d.Id, src => src.MapFrom(s => s.DocumentId))
            .ForMember(d=>d.CreatedAt, src=>src.MapFrom(s=>DateTime.Now))
            .ForMember(d=>d.UpdatedAt, src=>src.MapFrom(s=>DateTime.Now))
            .ForMember(d => d.Lines, src => src.MapFrom(s => s.Lines))
            .ForMember(d => d.ConfidenceAvg, src => src.MapFrom(s => s.QualityInfo.FirstOrDefault().AvgConfidence)).ReverseMap();
        CreateMap<OcrDocumentDto, OcrDocument>()
            .ForMember(d => d.Lines, src => src.MapFrom(s => s.Lines.AsEnumerable()));
        
        CreateMap<OcrDocument, OcrDocumentDto>()
            .ForMember(d => d.Lines, src => src.MapFrom(s=>s.Lines.ToList()));
        
        CreateMap<OcrResultDto, OcrResult>()
            .ForMember(d => d.ImageInfo, src => src.MapFrom(s => s.ImageInfo))
            .ForMember(d => d.Quality, src => src.MapFrom(s=>s.QualityInfo))
            .ForMember(d => d.ProcessInfo, src => src.MapFrom(s=>s.ProcessData)).ReverseMap();
        
        CreateMap<RuleTypeDto,RuleType>().ReverseMap();
        CreateMap<RuleScopeDto,RuleScope>().ReverseMap();
        CreateMap<FieldTypeDto, FieldType>().ReverseMap();
        CreateMap<DocumentTypeDto, DocumentTypes>().ReverseMap();
        CreateMap<FieldRuleDto, FieldRule>().ReverseMap();
    }
 }