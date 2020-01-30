using Common.Attributes;
using Common.CRMContext;
using System;

namespace Common.ModelsEx.CRM
{
    public class CorporateRemindersViewModel : CorporateReminder
    {
        public string StatusValue
        {
            get
            {
                return Status ? CorporateReminderStatus.Active.ToString() : CorporateReminderStatus.InActive.ToString();
            }
        }
        public string ReminderType
        {
            get
            {
                return ((CorporateReminderType)Type).GetDescription().ToString();
            }
        }
        public string StartDateDisplay
        {
            get
            {
                return StartDate.HasValue ?  StartDate.Value.ToShortDateString() :String.Empty;
            }
        }
        public string EndDateDisplay
        {
            get
            {
                return EndDate.HasValue ?  EndDate.Value.ToShortDateString() : string.Empty;
            }
        }
    }

    public enum CorporateReminderStatus
    {
        Active ,
        InActive
    }

    public enum CorporateReminderType
    {
        [Description("Product Launch")]
        ProductLaunch = 1,

        [Description("Trainings")]
        Trainings = 2,

        [Description("GTGT Week")]
        GTGTWeek = 3,

        [Description("Flash Sales")]
        FlashSales = 4,

        [Description("Talks")]
        Talks = 5
    }
}