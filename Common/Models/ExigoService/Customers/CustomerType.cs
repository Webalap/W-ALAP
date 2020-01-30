using System;
using System.Diagnostics.Contracts;


namespace ExigoService
{

    public sealed class CustomerType : ICustomerType {

        public CustomerType() {

            CustomerTypeID = 0;
            CustomerTypeDescription = null;

        }

        public CustomerType( int id, String description ) {

            CustomerTypeID = id;
            CustomerTypeDescription = description;

        }

        private CustomerType( CustomerType other ) {

            Contract.Requires( other != null );

            CustomerTypeID = other.CustomerTypeID;
            CustomerTypeDescription = other.CustomerTypeDescription;

        }


        public CustomerType DeepClone() {

            return new CustomerType( this );

        }



        public int CustomerTypeID { get; set; }
        public String CustomerTypeDescription { get; set; }

    }

}
