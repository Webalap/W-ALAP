using ExigoService;
using System.Collections.Generic;

namespace Common.ModelsEx.Shopping
{
    public class OrderConfirmation : Base.ServiceResponse
    {
        public OrderConfirmation()
            :base()
        {
            Order = new Order();
            lstPayment = new List<Payment>();
            Payment = new Payment();
        }

        public int CustomerID { get; set; }
        public Order Order { get; set; }
        public Payment Payment { get; set; }
        public List<Payment> lstPayment { get; set; }
        public int OwnerID { get; set; } // Added by Usman Akram to push affiliation to DataLayer on ThankYou page.
        public List<Product> Products { get; set; } // Added by Usman Akram to push products to DataLayer on ThankYou page.
    }
}