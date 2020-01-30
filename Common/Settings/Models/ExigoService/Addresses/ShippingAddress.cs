using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class ShippingAddress : Address
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string Company { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Email { get; set; }

        public string FullName
        {
            get { return string.Join(" ", this.FirstName, this.LastName); }
        }
    }
}