using System.Diagnostics;
using System.Threading.Tasks;
using HandwritenRecognition.Data.Entities;
using HandwritenRecognition.Data.Repository;

namespace HandwritenRecognition.Data.UnitOfWork;

public interface IUnitOfWork
{
    IGenericRepository<OcrDocument> OcrDocuments { get; }
    IGenericRepository<OcrLine> Lines { get; }
    IGenericRepository<FieldRule> FieldRules { get; }
    IGenericRepository<OcrJob> OcrJobs { get; }
    IGenericRepository<OcrResult> OcrResults { get; }
    IGenericRepository<ProcessData> ProcessData { get; }
    IGenericRepository<QualityData> QualityInfo { get; }
    IGenericRepository<ImageInfo> ImageInfo { get; }
    IGenericRepository<OcrBoundingBox> BoundingBoxes { get; }
    Task<int> SaveChangesAsync();
}