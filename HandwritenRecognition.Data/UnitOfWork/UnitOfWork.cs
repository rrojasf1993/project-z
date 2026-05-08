using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using HandwritenRecognition.Data.Entities;
using HandwritenRecognition.Data.Repository;

namespace HandwritenRecognition.Data.UnitOfWork;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly  OcrDbContext _context;
    public UnitOfWork(OcrDbContext context)
    {
        _context = context;
    }

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<OcrDocument> OcrDocuments =>
        field ??= new HandwritenRecognitionRepository<OcrDocument>(_context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<OcrLine> Lines =>
        field ??= new HandwritenRecognitionRepository<OcrLine>(_context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<FieldRule> FieldRules
    {
        get { return field ??= new HandwritenRecognitionRepository<FieldRule>(_context); }
    }

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<OcrJob> OcrJobs
    {
        get { return field ??= new HandwritenRecognitionRepository<OcrJob>(_context); }
    }

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<OcrResult> OcrResults =>
        field ??= new HandwritenRecognitionRepository<OcrResult>(_context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<ProcessData> ProcessData =>
        field ??= new HandwritenRecognitionRepository<ProcessData>(_context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<QualityData> QualityInfo =>
        field ??= new HandwritenRecognitionRepository<QualityData>(_context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<ImageInfo> ImageInfo => field ??= new HandwritenRecognitionRepository<ImageInfo>(_context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<OcrBoundingBox> BoundingBoxes=> field ??= new HandwritenRecognitionRepository<OcrBoundingBox>(_context);
    
    
    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<FieldType> FieldTypes=> field ??= new HandwritenRecognitionRepository<FieldType>(_context);
    
    
    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<ExtractedFields> ExtractedFields=> field ??= new HandwritenRecognitionRepository<ExtractedFields>(_context);
    
    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<RuleScope> RuleScopes=> field ??= new HandwritenRecognitionRepository<RuleScope>(_context);

    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<RuleType> RuleTypes=> field ??= new HandwritenRecognitionRepository<RuleType>(_context);
    
    [field: AllowNull]
    [field: MaybeNull]
    public IGenericRepository<DocumentTypes> DocumentTypes=> field ??= new HandwritenRecognitionRepository<DocumentTypes>(_context);



    public Task<int> SaveChangesAsync()
    {
        return _context.SaveChangesAsync();
    }


    private bool _disposed = false;


    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
            if (disposing)
                _context.Dispose();

        _disposed = true;
    }

    public void Dispose()
    { Dispose(true);
        GC.SuppressFinalize(this);
    }
}