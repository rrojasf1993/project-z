using System.Reflection.Metadata;
using HandwritenRecognition.Cross.DataTransferObjects;
using HandwritenRecognition.Data.Entities;

namespace HandwritenRecognition.Domain.Rules.DocumentClassifier;

public interface IDocumentClassifier
{
    Task<string> ClassifyDocumentAsync(OcrDocumentDto document);
}