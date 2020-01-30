namespace Common.ModelsEx.Shopping
{
    public class Shop : ShoppingExperience
    {
        public string SiteType { get; set; }
        public string AmbassadorEmail { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public string CorporateSiteOwner { get; set; }
        public bool IsShippingPaidByAmbassador { get; set; }
        public string AmbassadorName { get; set; }
        public string PrimaryCardNumber { get; set; }
        public int CardOwnerId { get; set; }
        public decimal ShippingAmount { get; set; }
        public string  SiteUrl { get; set; }
    }
}