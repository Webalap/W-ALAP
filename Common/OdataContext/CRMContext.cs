using System;

namespace Common.CRMContext
{

    /// <summary>
    /// There are no comments for CRMContext in the schema.
    /// </summary>

    public partial class Action
    {
        public Action()
        {

        }

        public int ActionID { get; set; }

        public Nullable<int> ContactID { get; set; }

        public Nullable<int> AsigneeID { get; set; }

        public string Name { get; set; }

        public string Status { get; set; }


        public Nullable<DateTime> CreatedDate { get; set; }

        public Nullable<DateTime> ModifiedDate { get; set; }

        public Nullable<DateTime> CLosedDate { get; set; }

        public Nullable<bool> IsVisible { get; set; }

        public Nullable<int> NextID { get; set; }
    }
    /// <summary>
    /// There are no comments for Call in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CallID
    /// </KeyProperties>
    public partial class Call
    {

        public int CallID { get; set; }

        public Nullable<int> ContactID { get; set; }

        public string CallResult { get; set; }

        public Nullable<System.DateTime> CallTime { get; set; }

        public string NoteText { get; set; }

        public string Via { get; set; }

        public string RecordingLink { get; set; }

        public Nullable<int> AuthorID { get; set; }

    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.CorporateReminder in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CorporateReminderID
    /// </KeyProperties>
    public partial class CorporateReminder
    {
        public int CorporateReminderID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Nullable<System.DateTime> StartDate { get; set; }


        public Nullable<System.DateTime> EndDate { get; set; }

        public string RawHtml { get; set; }

        public Nullable<int> Recipiants { get; set; }

        public bool Status { get; set; }

        public Nullable<int> Type { get; set; }

    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.CustomerBirthdayCalendarEvent in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerBirthdayCalendarEventID
    /// </KeyProperties>
    public partial class CustomerBirthdayCalendarEvent
    {

        public int CustomerBirthdayCalendarEventID { get; set; }

        public int CustomerID { get; set; }

        public Nullable<int> EnrollerID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }


    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.CustomerHostReminder in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerHostReminderID
    /// </KeyProperties>
    public partial class CustomerHostReminder
    {

        public int CustomerHostReminderID { get; set; }

        public Nullable<int> CustomerID { get; set; }

        public Nullable<int> GetTogetherID { get; set; }

        public Nullable<int> HostReminderID { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.Deal in the schema.
    /// </summary>
    /// <KeyProperties>
    /// DealID
    /// </KeyProperties>
    public partial class Deal
    {

        public int DealID { get; set; }

        public Nullable<int> ContactID { get; set; }

        public string Name { get; set; }

        public string Body { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }

        public Nullable<decimal> Amount { get; set; }

        public string Status { get; set; }

        public Nullable<bool> IsClosed { get; set; }

        public Nullable<int> Stage { get; set; }

        public Nullable<DateTime> ExpectedClosed { get; set; }

        public Nullable<DateTime> ModifiedDate { get; set; }

        public Nullable<int> AuthorID { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.Event in the schema.
    /// </summary>
    /// <KeyProperties>
    /// EventID
    /// </KeyProperties>
    public partial class Event
    {

        public int EventID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Nullable<DateTime> StartDate { get; set; }
        public Nullable<DateTime> EndDate { get; set; }

        public Nullable<bool> Repeat { get; set; }
        public Nullable<bool> Reminder { get; set; }
        public Nullable<int> ContactID { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public Nullable<int> AuthorID { get; set; }
        public bool IsAllDay { get; set; }
        public Nullable<bool> IsVisible { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.GetTogetherCalendarEvent in the schema.
    /// </summary>
    /// <KeyProperties>
    /// GetTogetherCalendarEventID
    /// </KeyProperties>
    public partial class GetTogetherCalendarEvent
    {

        public int GetTogetherCalendarEventID { get; set; }
        public Nullable<int> GTID { get; set; }

        public Nullable<int> HostID { get; set; }

        public string HostFirstName { get; set; }

        public string HostLastName { get; set; }

        public string GtName { get; set; }

        public string Link { get; set; }

        public string Address { get; set; }

        public Nullable<DateTime> CreatedDate { get; set; }

        public Nullable<DateTime> GtDate { get; set; }

        public Nullable<int> EnrollerID { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.HostReminder in the schema.
    /// </summary>
    /// <KeyProperties>
    /// HostReminderID
    /// </KeyProperties>
    public partial class HostReminder
    {
        public int HostReminderID { get; set; }

        public string Title { get; set; }

        public Nullable<int> Duration { get; set; }

        public string Description { get; set; }

        public bool IsAfterBooking { get; set; }

        public bool IsBeforeGT { get; set; }

        public bool IsAfterGT { get; set; }
    }
    /// <summary>
    /// There are no comments for CodeFirstNamespace.Note in the schema.
    /// </summary>
    /// <KeyProperties>
    /// NoteID
    /// </KeyProperties>
    public partial class Note
    {
        public int NoteID { get; set; }

        public Nullable<int> ContactID { get; set; }
        public string Body { get; set; }
        public Nullable<DateTime> CreatedDate { get; set; }
        public Nullable<DateTime> ModifiedDate { get; set; }
        public string DealID { get; set; }
        public Nullable<int> AuthorID { get; set; }

    }
}