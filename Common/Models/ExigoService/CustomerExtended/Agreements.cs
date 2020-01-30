using Common;
using System;

namespace ExigoService
{
    public class Agreements
    {
        public Agreements() { }

        public Agreements(CustomerExtendedDetails detail,
            DateTime agreementsEffectiveDate,
            int gracePeriodDays)
        {
            AgreementsEffectiveDate = agreementsEffectiveDate;
            GracePeriodDays = gracePeriodDays;

            populateAgreements(detail);
        }

        public int? CustomerExtendedDetailID { get; set; }
        public bool AgreeToIhPolicyAndProcedures { get; set; }
        public bool AgreeToIhSaEnrollmentPolicy { get; set; }
        public bool AgreeToIhESignNotice { get; set; }

        public bool IsCompliant 
        { 
            get
            {
                return (AgreeToIhSaEnrollmentPolicy &&
                    AgreeToIhPolicyAndProcedures &&
                    AgreeToIhESignNotice);
            }
        }

        public bool? GracePeriodExpired { get; set; }
        public bool BypassAgreements { get; set; }
        public DateTime? AgreementsEffectiveDate { get; set; }
        public int? GracePeriodDays { get; set; }

        private void populateAgreements(CustomerExtendedDetails detail)
        {
            CustomerExtendedDetailID = detail.CustomerExtendedDetailID;
            AgreeToIhPolicyAndProcedures = false;
            AgreeToIhSaEnrollmentPolicy = false;
            AgreeToIhESignNotice = false;
            GracePeriodExpired = false;

            DateTime date1 = DateTime.MinValue,
                date4 = DateTime.MinValue,
                date5 = DateTime.MinValue,
                date6 = DateTime.MinValue;

            DateTime.TryParse(detail.Field1, out date1);
            DateTime.TryParse(detail.Field4, out date4);
            DateTime.TryParse(detail.Field5, out date5);
            DateTime.TryParse(detail.Field6, out date6);

            if (date1 >= AgreementsEffectiveDate)
            {
                AgreeToIhSaEnrollmentPolicy = true;
            }

            if (date4 >= AgreementsEffectiveDate)
            {
                AgreeToIhPolicyAndProcedures = true;
            }

            if (date5 >= AgreementsEffectiveDate)
            {
                AgreeToIhESignNotice = true;
            }

            if (!DateTime.MinValue.Equals(date6) &&
                (DateTimeSpan.CompareDates(DateTime.Now, date6).Days > GracePeriodDays ||
                !GracePeriodDays.HasValue))
            {
                GracePeriodExpired = true;
            }
            else if (DateTime.MinValue.Equals(date6))
            {
                GracePeriodExpired = null;
            }
        }
    }
}