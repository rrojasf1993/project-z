namespace HandwritenRecognition.Services;

public interface IFileService
{
    public string GetFileContentAsBase64Str(string path);
}