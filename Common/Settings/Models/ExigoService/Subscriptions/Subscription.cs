using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public class Subscription : ISubscription
    {
        public int SubscriptionID { get; set; }
        public string SubscriptionDescription { get; set; }
    }
}
