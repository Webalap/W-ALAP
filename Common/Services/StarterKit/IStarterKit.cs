using System.Collections.Generic;

namespace Common.Services.StarterKit
{
    public interface IStarterKit
    {
       IEnumerable<StarterKitItems> GetStarterKits();
       IEnumerable<StarterKitItems> GetStarterKitItems();
       bool SaveStarterKitItems(string StarterKitCategoryID, string strItemCode);
       StarterKitItems GetSavedStarterKits(string StarterKitCategoryID);
    }
}
