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
    public DbSet<ProcessData> ProcessDatas => Set<ProcessData>();


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
        modelBuilder.Entity<OcrDocument>().Property(ocd => ocd.CreatedAt).HasDefaultValueSql("getdate()");
        modelBuilder.Entity<OcrDocument>().Property(ocd => ocd.ConfidenceAvg).IsRequired();
        modelBuilder.Entity<OcrDocument>().Property(ocd => ocd.Status).IsRequired();


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
        modelBuilder.Entity<FieldRule>().Property(fr => fr.ValidationPattern).IsRequired().HasMaxLength(1500);
        modelBuilder.Entity<FieldRule>().Property(fr => fr.DetectionPattern).IsRequired().HasMaxLength(1500);
        modelBuilder.Entity<FieldRule>().Property(fr=>fr.Scope).IsRequired();
        modelBuilder.Entity<FieldRule>().Property(fr=>fr.UseNextLine).IsRequired();
        modelBuilder.Entity<FieldRule>().Property(fr=>fr.MinConfidence).IsRequired();
        modelBuilder.Entity<FieldRule>().Property(fr => fr.ConfidenceWeight).IsRequired();
        modelBuilder.Entity<FieldRule>().Property(fr => fr.Priority).IsRequired();
        modelBuilder.Entity<FieldRule>().Property(fr => fr.DocumentType).IsRequired().HasMaxLength(300);
        modelBuilder.Entity<FieldRule>().HasKey(fr => fr.Id);
        modelBuilder.Entity<FieldRule>().Property(fr => fr.FieldType).HasMaxLength(300).IsRequired();
        

        modelBuilder.Entity<ProcessData>().Property(pd => pd.Id).IsRequired();
        modelBuilder.Entity<ProcessData>().Property(pd => pd.Id).ValueGeneratedOnAdd();
        modelBuilder.Entity<ProcessData>().Property(pd => pd.ProcessingTime).IsRequired();
        modelBuilder.Entity<ProcessData>().Property(pd => pd.Profile).IsRequired().HasMaxLength(300);
        
        modelBuilder.Entity<ExtractedFields>().Property(ef => ef.Id).IsRequired().ValueGeneratedOnAdd();
        modelBuilder.Entity<ExtractedFields>().HasKey(ef => ef.Id);
        modelBuilder.Entity<ExtractedFields>().Property(ef => ef.FieldName).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef=>ef.Value).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef=>ef.RuleId).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef=>ef.WasHumanCorrected).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef=>ef.Value).IsRequired();
        modelBuilder.Entity<ExtractedFields>().Property(ef=>ef.CreatedAt).IsRequired();






    }
}