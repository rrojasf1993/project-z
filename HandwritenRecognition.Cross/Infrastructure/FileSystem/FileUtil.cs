namespace HandwritenRecognition.Cross.Infrastructure.FileSystem;

public static class FileUtil
{
    public static async Task<string> SaveUploadedFile(Stream inputStream,string fileName, string destinationFilePath)
    {
        MemoryStream tempStream = new MemoryStream();
        await inputStream.CopyToAsync(tempStream);
        tempStream.Seek(0, SeekOrigin.Begin);
        
        if(!Directory.Exists(destinationFilePath))
            Directory.CreateDirectory(destinationFilePath);
        string filePath = Path.Combine(destinationFilePath, fileName);
        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        {
            await fileStream.WriteAsync(tempStream.ToArray(), 0, (int)tempStream.Length);
        }
        return Path.GetFullPath(filePath);
    }

    public static async Task<MemoryStream> DownloadTempUploadedFile(string destinationFilePathWithName)
    {
        string filePath =destinationFilePathWithName;
        MemoryStream ms = new MemoryStream();
        using (FileStream fileStream = new FileStream(filePath, FileMode.Open))
        {
            await fileStream.CopyToAsync(ms);
            ms.Seek(0, SeekOrigin.Begin);
        }
        return ms;
    }
    
    
    
    
}