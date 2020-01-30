
using System;


using Common;


namespace ExigoService {

    public static partial class Exigo {

        public static bool IsAmbassadorActive(int customerId)
        {

            const int VOLUME_ID__IS_ACTIVE = 14;   // Month-to-date PRV greater than $500


            VolumeCollection volumes = GetCustomerVolumes3(

                new GetCustomerVolumesRequest
                {
                    CustomerID = customerId,
                    PeriodTypeID = PeriodTypes.Monthly,
                    VolumeIDs = new[] { VOLUME_ID__IS_ACTIVE }
                }

            );


            return (volumes.Volume14 > 0.0M);
        }
        public static bool AmbassadorHas1500PRV(int customerId)
        {

            const int VOLUME_ID = 22;   // PRV greater than $1500


            VolumeCollection volumes = GetCustomerVolumes3(

                new GetCustomerVolumesRequest
                {
                    CustomerID = customerId,
                    PeriodTypeID = PeriodTypes.Monthly,
                    VolumeIDs = new[] { VOLUME_ID }
                }

            );


            return (volumes.Volume22 >= 1500.0M);

        }

        public static bool InEBPeriod(int customerId)
        {
            //ExigoCustomer exigoCustomer = Exigo.OData().Customers.Where(c => (c.CustomerID == ambassadorId)).FirstOrDefault();
            Customer exigoCustomer = Exigo.GetCustomer(customerId);

            if (exigoCustomer == null)
            {
                return false;
            }

            if (!exigoCustomer.Date1.HasValue)
            {
                return false;
            }

            DateTime ambassadorStartDate = exigoCustomer.Date1.Value;

            bool isFirst100Days = IsFirst100DaysOfBecomingSa(DateTime.Now, ambassadorStartDate);

            return isFirst100Days;
        }

        private static bool IsFirst100DaysOfBecomingSa(DateTime asOf, DateTime dateBecameSa)
        {

            TimeSpan timeSinceBecameSa = asOf - dateBecameSa;

            return (timeSinceBecameSa.TotalDays <= 100);

        }


    }

}
