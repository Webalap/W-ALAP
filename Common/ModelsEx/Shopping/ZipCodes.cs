using System.Collections.Generic;

namespace Common.ModelsEx.Shopping
{
    public class ZipCodes //zip_codes
    {
        public string ZipCode { get; set; } //zip_code
        public decimal Distance { get; set; }  // distance
        public string City { get; set; }  // city
        public string State { get; set; } // state
        public string ZipCodeDistance { 
            get
            {
                return string.Format(@"'{0}',{1}",this.ZipCode,this.Distance);
            }
        }
    }
    public class ZipCodesList
    {
        public List<ZipCodes> ZipCodes { get; set; }

        public ZipCodesList()
        {
            ZipCodes = new List<ZipCodes>();
        }
    }
}