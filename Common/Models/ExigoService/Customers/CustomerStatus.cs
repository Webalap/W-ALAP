using System;
using System.Diagnostics.Contracts;


namespace ExigoService
{

    public sealed class CustomerStatus : ICustomerStatus {

        public CustomerStatus() {

            CustomerStatusID = 0;
            CustomerStatusDescription = null;

        }

        public CustomerStatus( int id, String description ) {

            CustomerStatusID = id;
            CustomerStatusDescription = description;

        }

        private CustomerStatus( CustomerStatus other ) {

            Contract.Requires( other != null );

            CustomerStatusID = other.CustomerStatusID;
            CustomerStatusDescription = other.CustomerStatusDescription;

        }


        public CustomerStatus DeepClone() {

            return new CustomerStatus( this );

        }



        public int CustomerStatusID { get; set; }
        public String CustomerStatusDescription { get; set; }

    }

}
