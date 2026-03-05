using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using HandwritenRecognition.Data.Entities;
using HandwritenRecognition.Data.Repository;

namespace HandwritenRecognition.Data.UnitOfWork;

public class UnitOfWork(OcrDbContext context) : IUnitOfWork, IDisposable
{
    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<OcrDocument> OcrDocuments =>
        field ??= new HandwritenRecognitionRepository<OcrDocument>(context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<OcrLine> Lines =>
        field ??= new HandwritenRecognitionRepository<OcrLine>(context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<FieldRule> FieldRules
    {
        get { return field ??= new HandwritenRecognitionRepository<FieldRule>(context); }
    }

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<OcrJob> OcrJobs
    {
        get { return field ??= new HandwritenRecognitionRepository<OcrJob>(context); }
    }

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<OcrResult> OcrResults =>
        field ??= new HandwritenRecognitionRepository<OcrResult>(context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<ProcessData> ProcessData =>
        field ??= new HandwritenRecognitionRepository<ProcessData>(context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<QualityData> QualityInfo =>
        field ??= new HandwritenRecognitionRepository<QualityData>(context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<ImageInfo> ImageInfo => field ??= new HandwritenRecognitionRepository<ImageInfo>(context);


    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }


    private bool _disposed = false;


    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                context.Dispose();

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}