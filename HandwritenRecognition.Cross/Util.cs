using HandwritenRecognition.Cross.DataTransferObjects;

namespace HandwritenRecognition.Cross;

public static class Util
{
    public static OcrBoundingBoxDto ConvertFromPaddle(List<List<int>> points)
    {
        if(points is null || !points.Any())
            return new OcrBoundingBoxDto();
        var xs = points.Select(p => p[0]);
        var ys = points.Select(p => p[1]);
        var minX = xs.Min();
        var minY = ys.Min();
        var maxX = xs.Max();
        var maxY = ys.Max();
        return new OcrBoundingBoxDto
        {
            X = minX,
            Y = minY,
            W = maxX - minX,
            H = maxY - minY,
        };
    }
}