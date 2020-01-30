using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface ICart
    {
        ShoppingCartItemCollection Items { get; set; }
    }
}