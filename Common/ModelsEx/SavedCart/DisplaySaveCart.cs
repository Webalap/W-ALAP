namespace Common.ModelsEx.SavedCart
{
    public class DisplaySaveCart
    {
        public int ID { get; set; }
        public int CustomerID { get; set; }
        public string PropertyBag { get; set; }
        public int ItemCount { get; set; }
        public decimal Total  { get; set; }
        public string CartType { get; set; }
        public string CustomerName { get; set; }
    }
}