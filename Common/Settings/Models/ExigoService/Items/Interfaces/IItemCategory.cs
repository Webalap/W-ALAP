using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public interface IItemCategory
    {
        int ItemCategoryID { get; set; }
        string ItemCategoryDescription { get; set; }
        int SortOrder { get; set; }        
        
        int? ParentItemCategoryID { get; set; }
        IEnumerable<ItemCategory> Subcategories { get; set; }
    }
}
