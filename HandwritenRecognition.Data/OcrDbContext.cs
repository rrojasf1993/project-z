using HandwritenRecognition.Data.Entities;

namespace HandwritenRecognition.Data;

public class OcrDbContext //:DbContext
{
    public DbSet<OcrDocument> Documents => Set<OcrDocument>();
    public DbSet<OcrLine> Lines => Set<OcrLine>();
    public DbSet<FieldRule> FieldRules=>Set<FieldRule>();
}