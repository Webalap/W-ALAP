using System.ComponentModel.DataAnnotations;

namespace ExigoService
{
    public class ShippingAddress : Address
    {
        public ShippingAddress() { }

        public ShippingAddress(Address address)
        {
          
            this.Address1 = address.Address1;
            this.Address2 = address.Address2;
            this.City = address.City;
            this.State = address.State;
            this.Zip = address.Zip;
            this.Country = address.Country;
        }
        public int CustomerID { get; set; }
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string Company { get; set; }

        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

       [RegularExpression(@"^[-a-z-A-Z0-9_}{?]+(\.[-a-z-A-Z0-9}{\'?]+)*@([a-z-A-Z0-9][-a-z0-9]*)+(\.[-a-z-A-Z0-9}{\'?]+)+$", ErrorMessage = @"Please Enter a valid email")]

        public string Email { get; set; }

        public string FullName
        {
            get { return string.Join(" ", FirstName, LastName); }
        }
    }
}