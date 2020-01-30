using System.Collections.Generic;

namespace Common.Services.StarterKit
{
    public class StarterKitViewModel
    {
        public IEnumerable<StarterKitItems> Items { get; set; }
        public StarterKitItems SavedItems { get; set; }
        public IEnumerable<StarterKitItems> StarterKitCategories{ get; set; }
    }
    public class StarterKitItems
    {
        public int ID { get; set; }
        public string ItemCode { get; set; }
        public string KitItemCode { get; set; }
        public string ShortDetail { get; set; }
        public string SmallPicture { get; set; }
        public decimal Price { get; set; }
    }
}