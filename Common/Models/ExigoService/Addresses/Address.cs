using Common.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;


namespace ExigoService
{

    public class Address : IAddress {

        public Address() {

            AddressType = AddressType.New;

        }

        public Address( 
                    AddressType type, 

                    String address1, 
                    String address2, 

                    String city, 
                    String state, 
                    String zip, 

                    String country 
        ) {

            AddressType = type;

            Address1 = address1;
            Address2 = address2;

            City = city;
            State = state;
            Zip = zip;

            Country = country;

        }

        private Address( Address other ) {

            Contract.Requires( other != null );

            AddressType = other.AddressType;

            Address1 = other.Address1;
            Address2 = other.Address2;

            City = other.City;
            State = other.State;
            Zip = other.Zip;

            Country = other.Country;

        }


        public Address DeepClone() {

            return new Address( this );

        }



        [Required]
        public AddressType AddressType { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public String Address1 { get; set; }

        public String Address2 { get; set; }


        [Required]
        public String City { get; set; }

        [Required(ErrorMessage = "Please select state or province")]
        public String State { get; set; }

        [Required]
        public String Zip { get; set; }


        [Required]
        public String Country { get; set; }           


        public String AddressDisplay {
            get { return Address1 + ( !Address2.IsEmpty() ? " {0}".FormatWith( Address2 ) : String.Empty ); }
        }


        public bool IsComplete {

            get {

                return !String.IsNullOrEmpty( Address1 ) &&
                       !String.IsNullOrEmpty( City ) &&
                       !String.IsNullOrEmpty( State ) &&
                       !String.IsNullOrEmpty( Zip ) &&
                       !String.IsNullOrEmpty( Country );

            }

        }


        public String GetHash() {

            return Security.GetHashString( 
                String.Format( 
                        "{0}|{1}|{2}|{3}|{4}",
                        ( AddressDisplay ?? String.Empty ).Trim(),
                        ( City ?? String.Empty ).Trim(),
                        ( State ?? String.Empty ).Trim(),
                        ( Zip ?? String.Empty ).Trim(),
                        ( Country ?? String.Empty ).Trim() 
                ) 

            );

        }

        public override bool Equals( Object obj ) {

            try {
                //Check for null or type is not Address
                if ((obj == null) || (this.GetType() != obj.GetType()))
                {
                    return false;
                }

                String hasha = GetHash();

                String hashb = ( (Address)obj ).GetHash();

                return hasha.Equals( hashb );

            } catch {
                return false;
            }

        }


        //REVIEW: What is the point of this GetHashCode method if all it does is call the base class method?
        public override int GetHashCode() {

            return base.GetHashCode();

        }

    }
  

}
