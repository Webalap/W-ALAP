using System;

namespace Common.Api.ExigoWebService
{
    public partial class CustomerExtendedResponse
    {
        public static explicit operator ExigoService.CustomerExtendedDetails(CustomerExtendedResponse customerExtended)
        {
            var model = new ExigoService.CustomerExtendedDetails();
            if (customerExtended == null) return model;

            model.CustomerID = customerExtended.CustomerID;
            model.CustomerExtendedDetailID = customerExtended.CustomerExtendedID;
            model.CustomerExtendedGroupID = customerExtended.ExtendedGroupID;
            model.Field1 = customerExtended.Field1;
            model.Field2 = customerExtended.Field2;
            model.Field3 = customerExtended.Field3;
            model.Field4 = customerExtended.Field4;
            model.Field5 = customerExtended.Field5;
            model.Field6 = customerExtended.Field6;
            model.Field7 = customerExtended.Field7;
            model.Field8 = customerExtended.Field8;
            model.Field9 = customerExtended.Field9;
            model.Field10 = customerExtended.Field10;
            model.Field11 = customerExtended.Field11;
            model.Field12 = customerExtended.Field12;
            model.Field13 = customerExtended.Field13;
            model.Field14 = customerExtended.Field14;
            model.Field15 = customerExtended.Field15;
            model.Field16 = customerExtended.Field16;
            model.Field17 = customerExtended.Field17;
            model.Field18 = customerExtended.Field18;
            model.Field19 = customerExtended.Field19;
            model.Field20 = customerExtended.Field20;
            model.Field21 = customerExtended.Field21;
            model.Field22 = customerExtended.Field22;
            model.Field23 = customerExtended.Field23;
            model.Field24 = customerExtended.Field24;
            model.Field25 = customerExtended.Field25;
            model.Field26 = customerExtended.Field26;
            model.Field27 = customerExtended.Field27;
            model.Field28 = customerExtended.Field28;
            model.Field29 = customerExtended.Field29;
            model.Field30 = customerExtended.Field30;
            model.ModifiedDate = DateTime.Now;

            return model;
        }
    }
}