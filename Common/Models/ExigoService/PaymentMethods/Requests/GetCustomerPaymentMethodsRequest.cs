namespace ExigoService
{
    public class GetCustomerPaymentMethodsRequest
    {
        public int CustomerID { get; set; }
        public bool ExcludeInvalidMethods { get; set; }
        public bool ExcludeIncompleteMethods { get; set; }
        public bool ExcludeNonAutoshipPaymentMethods { get; set; }
    }
}