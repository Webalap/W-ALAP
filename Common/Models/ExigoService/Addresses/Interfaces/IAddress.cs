using System;


namespace ExigoService
{

    public interface IAddress {

        AddressType     AddressType         { get; set; }


        String          Address1            { get; set; }
        String          Address2            { get; set; }

        String          City                { get; set; }
        String          State               { get; set; }
        String          Zip                 { get; set; }

        String          Country             { get; set; }


        String          AddressDisplay      { get; }

        bool            IsComplete          { get; }

    }

}
