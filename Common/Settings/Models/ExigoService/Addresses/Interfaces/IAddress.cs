﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public interface IAddress
    {
        AddressType AddressType { get; set; }

        string Address1 { get; set; }
        string Address2 { get; set; }
        string City { get; set; }
        string State { get; set; }
        string Zip { get; set; }
        string Country { get; set; }

        string AddressDisplay { get; }
        bool IsComplete { get; }
    }
}