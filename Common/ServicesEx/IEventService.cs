using Common.ModelsEx.Event;
using System;
using System.Collections.Generic;

namespace Common.ServicesEx
{
    public interface IEventService
    {
        Event[] GetEvents(int customerId);

        Event GetEventDetails(int eventId);
        Event GetEventDetailsService(int eventId, bool ApiCall = false);

        EventConfirmation CreateEvent(EventBooking eventBooking);

        EventConfirmation EditEvent(EventBooking eventBooking);

        decimal GetPointAccountBalance(int customerId, int pointAccountId);

        RewardsAccount[] GetHostRewardPointAccounts(int customerId);
        List<Common.CRMContext.HostReminder> GetHostReminders();
        bool InsertCustomerHostReminders(int CustomerID, int GetTogetherID, DateTime GTStartDate);
        bool UpdateCustomerHostReminders(int GetTogetherID);
    }
}
