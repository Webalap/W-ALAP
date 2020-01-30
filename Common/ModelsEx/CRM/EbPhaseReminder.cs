using System;

namespace Common.ModelsEx.CRM
{
    public class EbPhaseReminder
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public int Duration { get; set; }
        public int ThresholdDays { get; set; }
        public DateTime EbPhaseDateTime { get; set; }
    }
    public class EbPhaseReminderCustomer : EbPhaseReminder
    {
        public int CustomerID { get; set; }
        public string CustomersTitle { get; set; }
        public string CustomersDescription { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime AmbassadorDate { get; set; }
    }
    public class EbPhaseCalendarEntity : EbPhaseReminder
    {
        public EbPhaseReminderCustomer Customers { get; set; }
        public string CalendarDescription
        {
            get
            {
                return Customers.CustomersDescription;
            }
        }
        public string CalendarTitle
        {
            get
            {
                return Customers.Title;
            }
        }
        public EbPhaseCalendarEntity()
        {
            Customers = new EbPhaseReminderCustomer();
        }
    }
}