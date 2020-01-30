using System;

namespace ExigoService
{
    public class ImageFile : IImageFile
    {
        public string Path { get; set; }
        public string FileName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public int Size { get; set; }

        public string Url { get; set; }
 
        public ImageFile() { }
    }
}
