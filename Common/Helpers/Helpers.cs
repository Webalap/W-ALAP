using Common.Attributes;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using CorporateReminderType = Common.ModelsEx.CRM.CorporateReminderType;
namespace Common.Helpers
{
    public static partial class CRMCommon
    {

        public enum CRMEntity
        {
            [Description("Action")]
            Action = 1,
            [Description("Contact")]
            Contact = 2,
            [Description("Meeting")]
            Event = 3,
            [Description("Call")]
            Call = 4,
            [Description("Note")]
            Note = 5,
            [Description("Order")]
            Order = 6,
            [Description("Customer")]
            Customer = 7,
            [Description("Corporate Reminder")]
            CorporateReminder = 8,
        }
        /// <summary>
        /// will return the success message after sccessfully saved. 
        /// </summary>
        /// <param name="crmAction"></param>
        /// <returns></returns>
        public static string GetSuccessMessage(int crmAction, bool IsUpdate, bool IsDelete)
        {
            string actionName = string.Empty;
            switch (crmAction)
            {
                case 1:
                    actionName = GetDescription(CRMEntity.Action);
                    break;
                case 2:
                    actionName = GetDescription(CRMEntity.Contact);
                    break;
                case 3:
                    actionName = GetDescription(CRMEntity.Event);
                    break;
                case 4:
                    actionName = GetDescription(CRMEntity.Call);
                    break;
                case 5:
                    actionName = GetDescription(CRMEntity.Note);
                    break;
                case 7:
                    actionName = GetDescription(CRMEntity.Customer);
                    break;
                case 8:
                    actionName = GetDescription(CRMEntity.CorporateReminder);
                    break;
                default:
                    break;
            }
            if (IsUpdate)
            {
                return string.Format("{0} has been updated successfully.", actionName);
            }
            else if (IsDelete)
            {
                return string.Format("{0} has been deleted successfully.", actionName);
            }
            else
            {
                return string.Format("{0} has been saved successfully.", actionName);
            }

        }

        /// <summary>
        /// getting the description of enum
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetDescription(this Enum value)
        {
            Type type = value.GetType();
            string name = Enum.GetName(type, value);
            if (name != null)
            {
                System.Reflection.FieldInfo field = type.GetField(name);
                if (field != null)
                {
                    DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                    if (attr != null)
                    {
                        return attr.Description;
                    }
                }
            }
            return null;
        }


        public static SelectList GetCorporateReminderTypes()
        {
            var listItems = new List<SelectListItem>();
            
            foreach (var item in Enum.GetNames(typeof(CorporateReminderType)))
            {
                CorporateReminderType CorporateReminder = (CorporateReminderType)Enum.Parse(typeof(CorporateReminderType), item, true);
                listItems.Add(new SelectListItem { Text = CorporateReminder.GetDescription(), Value = ((int)(CorporateReminder)).ToString() });
            }
            var dataToReturn =  new SelectList(listItems, "Value", "Text");
            return dataToReturn;
        }
        
    }
}