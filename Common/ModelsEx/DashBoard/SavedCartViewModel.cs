using ExigoService;

namespace Common.ModelsEx.DashBoard
{
    public class SavedCartViewModel
    {
        public int Id { get; set; }
        public string CartName { get; set; }
        public string UrlLink { get; set; }
        public BasePropertyBag PropertyBag { get; set; }
        public string ImageLink { get; set; }
        public string DisplayText { get; set; }
    }
}