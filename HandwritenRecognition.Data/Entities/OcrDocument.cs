using System;
using System.Collections.Generic;
using HandwritenRecognition.Cross;

namespace HandwritenRecognition.Data.Entities;

public class OcrDocument:BaseEntity
{
    public Guid Id { get; set; }

    public float ConfidenceAvg { get; set; }
    public ICollection<OcrLine> Lines { get; set; }
    public OcrDocumentStatus_ Status { get; set; } = OcrDocumentStatus_.PendingReview;

    public List<ExtractedFields>? ExtractedFieldsForDocument { get; set; }
    public DocumentTypes? AssociatedType { get; set; }
}