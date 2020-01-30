using Common.ModelsEx.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Common.ServicesEx
{
    public class EbPhaseReminderServices
    {


        private static List<EbPhaseReminder> GetTemplateEbPhase()
        {
            List<EbPhaseReminder> lstEbPhase = new List<EbPhaseReminder>();
            lstEbPhase.Add(new EbPhaseReminder
            {
                Duration = 1,
                ThresholdDays = 15,
                Title = @"Halfway in EB Phase 1",
            });
            lstEbPhase.Add(new EbPhaseReminder
            {
                Duration = 1,
                ThresholdDays = 55,
                Title = @"Halfway in EB Phase 2",
            });
            lstEbPhase.Add(new EbPhaseReminder
            {
                Duration = 1,
                ThresholdDays = 85,
                Title = @"Halfway in EB Phase 3",
            });
            lstEbPhase.Add(new EbPhaseReminder
            {
                Duration = 1,
                ThresholdDays = 35,
                Title = @"5 days left in EB Phase 1",
            });
            lstEbPhase.Add(new EbPhaseReminder
            {
                Duration = 1,
                ThresholdDays = 65,
                Title = @"5 days left in EB Phase 2",
            });
            lstEbPhase.Add(new EbPhaseReminder
            {
                Duration = 1,
                ThresholdDays = 95,
                Title = @"5 days left in EB Phase 3",
            });
            return lstEbPhase;
        }
        public static List<EbPhaseCalendarEntity> GetCustomerEbPhase(List<CustomerEbPhase> lstCustomer, int CustomerID)
        {
            List<EbPhaseReminderCustomer> lstEbPhases = new List<EbPhaseReminderCustomer>();
            List<EbPhaseCalendarEntity> lstEbPhasesCalendar = new List<EbPhaseCalendarEntity>();
            var ebPhases = GetTemplateEbPhase();
            int ID = 1;
            foreach (var customer in lstCustomer)
            {
                foreach (var ebPhase in ebPhases)
                {
                    DateTime ebphaseTime = customer.Date1.Value;
                    ebphaseTime = ebphaseTime.AddDays(ebPhase.ThresholdDays);

                    lstEbPhases.Add(new EbPhaseReminderCustomer
                    {
                        AmbassadorDate = customer.Date1 ?? customer.Date1.Value,
                        Title = ebPhase.Title,
                        Duration = ebPhase.ThresholdDays,
                        CustomersDescription = string.Format("{0} of Ambassador {1} {2} CustomerID {3} Volume {4}", ebPhase.Title, customer.FirstName, customer.LastName, customer.CustomerID, customer.Volume22),
                        FirstName = customer.FirstName,
                        Lastname = customer.LastName,
                        ThresholdDays = ebPhase.ThresholdDays,
                        EbPhaseDateTime = ebphaseTime,
                        CustomerID = customer.CustomerID
                    });
                }
            }
            //return NormalizeEbPhases(lstEbPhases);
            foreach (var item in lstEbPhases)
            {
                var customerEbhase = new List<EbPhaseReminderCustomer>();
                lstEbPhasesCalendar.Add(new EbPhaseCalendarEntity
                {
                    ID = ID++,
                    Duration = item.Duration,
                    ThresholdDays = item.Duration,
                    EbPhaseDateTime = item.EbPhaseDateTime,
                    Title = item.Title,
                    Customers = new EbPhaseReminderCustomer
                    {
                        CustomerID = item.CustomerID,
                        CustomersTitle = item.Title,
                        CustomersDescription = item.CustomersDescription,
                        FirstName = item.FirstName,
                        Lastname = item.Lastname,
                        AmbassadorDate = item.AmbassadorDate,
                        Duration = item.Duration,
                        ThresholdDays = item.ThresholdDays
                    }
                });
            }
            // remove the entrieson the bases of customer extended detail group, customerid, threshold days
            var customerExtendedDetails = ExigoService.Exigo.GetCustomerArchiveEb(CustomerID); // Current Customer ID
            // customerExtended Detail.field2 == i.customer.cutoemrid
            // customerExtended Detail.field3 == i.customer.Duration
            var result = (from x in customerExtendedDetails
                          from y in lstEbPhasesCalendar
                              .Where(y => y.Customers.CustomerID.ToString() == x.Field2 && y.Duration.ToString() == x.Field3)
                          select y).ToList();
            foreach (var item in result)
            {
                lstEbPhasesCalendar.Remove(item);
            }
            return lstEbPhasesCalendar;
        }


        private static List<EbPhaseCalendarEntity> NormalizeEbPhases(List<EbPhaseReminderCustomer> lstEbPhases)
        {
            List<EbPhaseCalendarEntity> lstEbPhasesNormalize = new List<EbPhaseCalendarEntity>();
            List<EbPhaseReminderCustomer> lst = lstEbPhases;
            EbPhaseCalendarEntity ebPhase;
            int ID = 1;
            foreach (var item in lstEbPhases)
            {
                var PhasesOnSameDay = lst.Where(i => i.EbPhaseDateTime.Equals(item.EbPhaseDateTime)).ToList();
                ebPhase = new EbPhaseCalendarEntity
                {
                    ID = ID++,
                    Duration = item.Duration,
                    ThresholdDays = item.Duration,
                    EbPhaseDateTime = item.EbPhaseDateTime,
                    Title = item.Title
                };
            }
            return lstEbPhasesNormalize;
        }
    }
}