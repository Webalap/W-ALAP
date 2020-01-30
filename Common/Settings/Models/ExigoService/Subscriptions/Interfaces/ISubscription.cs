using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public interface ISubscription
    {
        int SubscriptionID { get; set; }
        string SubscriptionDescription { get; set; }
    }
}
