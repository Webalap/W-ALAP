using ExigoService;
using System;

namespace Common.ModelsEx.Event
{
    public class EventBooking
    {
        public Customer Customer { get; set; }
        
        public CustomerSite CustomerSite { get; set; }
        
        public int CreatorCustomerID { get; set; }
        
        public int HostCustomerID { get; set; }

        public DateTime StartDate { get; set; }
        
        public DateTime ActualStartDate { get; set; }
        
        public DateTime CloseDate
        {
            get
            {
                return ActualStartDate.AddDays(7);
            }
        }
        
        public DateTime LockoutDate 
        { 
            get
            {
                return CloseDate.AddDays(7);
            }
        }
        public DateTime PartyDate { get; set; }
        public string PartyStartTime { get; set; }
        public string PartyEndTime { get; set; }
        public string TimeZone { get; set; }
    }
}