using Common.Api.ExigoWebService;
using Common.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExigoService
{
    public class Address : IAddress
    {
        public Address()
        {
            this.AddressType = AddressType.New;
        }

        [Required]
        public AddressType AddressType { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        [Required]
        public string Zip { get; set; }
        [Required]
        public string Country { get; set; }

        public string AddressDisplay
        {
            get { return this.Address1 + ((!this.Address2.IsEmpty()) ? " {0}".FormatWith(this.Address2) : ""); }
        }
        public bool IsComplete
        {
            get
            {
                return
                    !string.IsNullOrEmpty(Address1) &&
                    !string.IsNullOrEmpty(City) &&
                    !string.IsNullOrEmpty(State) &&
                    !string.IsNullOrEmpty(Zip) &&
                    !string.IsNullOrEmpty(Country);
            }
        }

        public string GetHash()
        {
            return Security.GetHashString(string.Format("{0}|{1}|{2}|{3}|{4}",
                this.AddressDisplay.Trim(),
                this.City.Trim(),
                this.State.Trim(),
                this.Zip.Trim(),
                this.Country.Trim()));
        }
        public override bool Equals(object obj)
        {
            try
            {
                var hasha = this.GetHash();
                var hashb = ((Address)obj).GetHash();
                return hasha.Equals(hashb);
            }
            catch
            {
                return false;
            }
        }
    }
}