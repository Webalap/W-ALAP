using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExigoService
{
    public class CountryRegionCollection
    {
        public IEnumerable<Country> Countries { get; set; }
        public IEnumerable<Region> Regions { get; set; }
    }
}