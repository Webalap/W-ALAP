using System;

namespace ExigoService
{
    public interface IImageFile
    {
        string Path { get; set; }
        string FileName { get; set; }
        DateTime ModifiedDate { get; set; }
        int Size { get; set; }
        string Url { get; set; }
    }
}
