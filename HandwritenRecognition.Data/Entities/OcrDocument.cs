using System;
using System.Collections.Generic;
using HandwritenRecognition.Cross;

namespace HandwritenRecognition.Data.Entities;

public class OcrDocument
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public float ConfidenceAvg { get; set; }
    public ICollection<OcrLine> Lines { get; set; }
    public OcrDocumentStatus_ Status { get; set; } = OcrDocumentStatus_.PendingReview;
}