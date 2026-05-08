using HandwritenRecognition.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HandwritenRecognition.Data;

public class OcrDbContext : DbContext
{
    public DbSet<OcrDocument> Documents => Set<OcrDocument>();
    public DbSet<OcrLine> Lines => Set<OcrLine>();
    public DbSet<FieldRule> FieldRules => Set<FieldRule>();
    public DbSet<OcrResult> Results => Set<OcrResult>();
    public DbSet<OcrJob> Jobs => Set<OcrJob>();
    public DbSet<ImageInfo> ImageInfos => Set<ImageInfo>();
    public DbSet<QualityData> QualityDatas => Set<QualityData>();
    protected DbSet<ProcessData> ProcessDatas => Set<ProcessData>();
    protected DbSet<OcrBoundingBox> OcrBoundingBoxes => Set<OcrBoundingBox>();
    protected DbSet<OcrLine> OcrLines => Set<OcrLine>();
    protected DbSet<ExtractedFields> ExtractedFields => Set<ExtractedFields>();

    public OcrDbContext()
    {
    }

    public OcrDbContext(DbContextOptions<OcrDbContext> options) : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<OcrDocument>()
            .Property(ocd => ocd.Id)
            .IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<OcrDocument>().HasKey(ocd => ocd.Id);
        modelBuilder.Entity<OcrDocument>().Property(ocd => ocd.CreatedAt).IsRequired();
        modelBuilder.Entity<OcrDocument>().Property(ocd => ocd.ConfidenceAvg).IsRequired();
        modelBuilder.Entity<OcrDocument>().Property(ocd => ocd.Status).IsRequired();
        modelBuilder.Entity<OcrDocument>().Property(ocd => ocd.UpdatedAt).IsRequired();


        modelBuilder.Entity<OcrLine>().HasKey(ol => ol.Id);
        modelBuilder.Entity<OcrLine>().Property(ol => ol.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<OcrLine>().Property(ol => ol.LineIndex).IsRequired();
        modelBuilder.Entity<OcrLine>().Property(ol => ol.OriginalText).IsRequired();
        modelBuilder.Entity<OcrLine>().Property(ol => ol.Status).IsRequired().HasMaxLength(60);
        modelBuilder.Entity<OcrLine>().Property(ol => ol.Confidence).IsRequired();
        modelBuilder.Entity<OcrLine>().Property(ol => ol.BoundingBox).IsRequired();

        modelBuilder.Entity<OcrJob>().HasKey(ocr => ocr.JobId);
        modelBuilder.Entity<OcrJob>().Property(ocr => ocr.JobId).IsRequired();
        modelBuilder.Entity<OcrJob>().Property(ocr => ocr.JobId).ValueGeneratedOnAdd();
        modelBuilder.Entity<OcrJob>().Property(ocr => ocr.Status).IsRequired().HasMaxLength(60);
        modelBuilder.Entity<OcrJob>().Property(ocr => ocr.FileName).IsRequired().HasMaxLength(255);
        modelBuilder.Entity<OcrJob>().Property(ocr => ocr.PreprocessedFileName).HasMaxLength(255);
        modelBuilder.Entity<OcrJob>().Property(ocr => ocr.Error).HasMaxLength(10000);
        

        modelBuilder.Entity<OcrResult>().HasKey(r => r.ResultId);
        modelBuilder.Entity<OcrResult>().Property(r => r.ResultId).IsRequired();
        modelBuilder.Entity<OcrResult>().Property(r => r.ResultId).ValueGeneratedOnAdd();


        modelBuilder.Entity<ImageInfo>().HasKey(i => i.Id);
        modelBuilder.Entity<ImageInfo>().Property(i => i.Id).IsRequired();
        modelBuilder.Entity<ImageInfo>().Property(i => i.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<ImageInfo>().Property(i => i.ActualHeight).IsRequired();
        modelBuilder.Entity<ImageInfo>().Property(i => i.ActualWidth).IsRequired();
        modelBuilder.Entity<ImageInfo>().Property(i => i.OriginalHeight).IsRequired();
        modelBuilder.Entity<ImageInfo>().Property(i => i.OriginalWidth).IsRequired();

        modelBuilder.Entity<ImageInfo>().HasKey(q => q.Id);
        modelBuilder.Entity<QualityData>().Property(q => q.Id).IsRequired();
        modelBuilder.Entity<QualityData>().Property(q => q.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<QualityData>().Property(q => q.Level).IsRequired();
        modelBuilder.Entity<QualityData>().Property(q => q.Notes).HasMaxLength(3000);


        modelBuilder.Entity<FieldRule>().Property(fr => fr.IsActive).IsRequired();
        modelBuilder.Entity<FieldRule>().Property(fr => fr.ValidationPattern).HasMaxLength(1500);
        modelBuilder.Entity<FieldRule>().Property(fr => fr.DetectionPattern).HasMaxLength(1500);
        modelBuilder.Entity<FieldRule>().Property(fr=>fr.UseNextLine).IsRequired();
        modelBuilder.Entity<FieldRule>().Property(fr => fr.Priority).IsRequired();
        modelBuilder.Entity<FieldRule>().HasKey(fr => fr.Id);
        modelBuilder.Entity<FieldRule>().Property(fr => fr.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<FieldRule>().Property(fr => fr.FieldName).IsRequired();
        

        modelBuilder.Entity<ProcessData>().Property(pd => pd.Id).IsRequired();
        modelBuilder.Entity<ProcessData>().HasKey(pd => pd.Id);
        modelBuilder.Entity<ProcessData>().Property(pd => pd.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<ProcessData>().Property(pd => pd.ProcessingTime).IsRequired();
        modelBuilder.Entity<ProcessData>().Property(pd => pd.Profile).IsRequired().HasMaxLength(300);
        
        
        modelBuilder.Entity<ExtractedFields>().Property(ef => ef.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<ExtractedFields>().HasKey(ef => ef.Id);
        modelBuilder.Entity<ExtractedFields>().Property(ef => ef.FieldName).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef=>ef.Value).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef=>ef.WasHumanCorrected).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef=>ef.Value).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef => ef.CreatedAt).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef=>ef.UpdatedAt).IsRequired();
        
        modelBuilder.Entity<OcrBoundingBox>().HasKey(obb => obb.Id);
        modelBuilder.Entity<OcrBoundingBox>().Property(obb => obb.Id).IsRequired();
        modelBuilder.Entity<OcrBoundingBox>().Property(obb=>obb.H).IsRequired().HasMaxLength(10);
        modelBuilder.Entity<OcrBoundingBox>().Property(obb => obb.W).IsRequired().HasMaxLength(10);
        modelBuilder.Entity<OcrBoundingBox>().Property(obb => obb.X).IsRequired().HasMaxLength(10);
        modelBuilder.Entity<OcrBoundingBox>().Property(obb => obb.Y).IsRequired().HasMaxLength(10);
        modelBuilder.Entity<OcrBoundingBox>().Property(obb => obb.W).IsRequired().HasMaxLength(10);
        
        modelBuilder.Entity<FieldType>().Property(ft => ft.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<FieldType>().Property(ft => ft.Description).IsRequired().HasMaxLength(500);
        modelBuilder.Entity<FieldType>().HasKey(ft => ft.Id);
        modelBuilder.Entity<FieldType>().Property(ft => ft.CreatedAt).IsRequired();
        modelBuilder.Entity<FieldType>().Property(ft => ft.UpdatedAt).IsRequired();

        modelBuilder.Entity<RuleScope>().Property(rs => rs.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<RuleScope>().HasKey(rs => rs.Id);
        modelBuilder.Entity<RuleScope>().Property(rs=>rs.Name).IsRequired().HasMaxLength(300);
        modelBuilder.Entity<RuleScope>().Property(rs=>rs.Notes).HasMaxLength(500);
        
        modelBuilder.Entity<RuleType>().Property(rt => rt.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<RuleType>().HasKey(rt => rt.Id);
        modelBuilder.Entity<RuleType>().Property(rt => rt.Kind).IsRequired().HasMaxLength(500);
        modelBuilder.Entity<RuleType>().Property(rt=>rt.Notes).HasMaxLength(500);
        
        modelBuilder.Entity<DocumentTypes>().HasKey(rt => rt.Id);
        modelBuilder.Entity<DocumentTypes>().Property(rt => rt.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<DocumentTypes>().Property(rt => rt.Name).IsRequired().HasMaxLength(300);
        modelBuilder.Entity<DocumentTypes>().Property(rt=>rt.Notes).HasMaxLength(500);
        
        modelBuilder.Entity<DocumentTypeKeywordRules>().HasKey(kwr => kwr.Id);
        modelBuilder.Entity<DocumentTypeKeywordRules>().Property(kwr => kwr.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<DocumentTypeKeywordRules>().Property(kwr => kwr.Keyword).IsRequired().HasMaxLength(600);
        modelBuilder.Entity<DocumentTypeKeywordRules>().Property(kwr => kwr.Weight).IsRequired();
        
        modelBuilder.Entity<DocumentTypeRegexPatternRules>().HasKey(trpr => trpr.Id);
        modelBuilder.Entity<DocumentTypeRegexPatternRules>().Property(trpr => trpr.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<DocumentTypeRegexPatternRules>().Property(trpr => trpr.RegexPattern).IsRequired().HasMaxLength(1500);
        modelBuilder.Entity<DocumentTypeRegexPatternRules>().Property(trpr => trpr.Weight).IsRequired();
        
        modelBuilder.Entity<DocumentTypeExample>().HasKey(dte => dte.Id);
        modelBuilder.Entity<DocumentTypeExample>().Property(dte => dte.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<DocumentTypeExample>().Property(dte=>dte.SampleText).IsRequired().HasMaxLength(-1);
        modelBuilder.Entity<DocumentTypeExample>().Property(dte => dte.EmbeddingVector).HasMaxLength(-1);

    }
}