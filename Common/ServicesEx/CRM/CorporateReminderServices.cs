using Common.Api.ExigoWebService;
using Common.CRMContext;
using Common.ModelsEx.CRM;
using Common.ModelsEx.Shopping;
using ExigoService;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ServicesEx
{
    public class CorporateReminderServices
    {
        [Inject]
        public static ExigoApi Api { get; set; }
        public static List<CorporateRemindersViewModel> Get()
        {
            List<CorporateRemindersViewModel> lstCorporateReminders = new List<CorporateRemindersViewModel>();
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"GetCorporateReminders");
                lstCorporateReminders = context.Query<CorporateRemindersViewModel>(sql).ToList();
            }
            return lstCorporateReminders;
        }


        public static List<CorporateRemindersViewModel> GetCustomersCorporateReminders(int customerid)
        {
            List<CorporateRemindersViewModel> lstCorporateReminders = new List<CorporateRemindersViewModel>();
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"GetCorporateRemindersCustomer {0},{1}",(int)CustomerExtendedGroup.CorporateReminder, customerid);
                lstCorporateReminders = context.Query<CorporateRemindersViewModel>(sql).ToList();
            }
            return lstCorporateReminders;
        }

        public static CorporateRemindersViewModel Get(int CorporateReminderID)
        {
            List<CorporateRemindersViewModel> lstCorporateReminders = new List<CorporateRemindersViewModel>();
            using (var context = Exigo.Sql())
            {
                //var sql = string.Format(@"SELECT [CorporateReminderID],[Name],[Description],[StartDate],[EndDate],[RawHtml],[Recipiants],[Status],[Type] FROM [CRMContext].[CorporateReminders] where CorporateReminderID = {0};", CorporateReminderID);
                var sql = string.Format(@"GetCorporateRemindersByCorporateReminderID {0}", CorporateReminderID);
                lstCorporateReminders = context.Query<CorporateRemindersViewModel>(sql).ToList();
            }
            return lstCorporateReminders.FirstOrDefault();
        }


        public static CorporateReminder Insert(CorporateReminder CorporateReminder)
        {
            List<CorporateReminder> lstCorporateReminders = new List<CorporateReminder>();
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"InsertCorporateReminders '{0}','{1}','{2}','{3}','{4}',{5},{6},{7}", 
                    CorporateReminder.Name.Replace("'", "''"), CorporateReminder.Description.Replace("'", "''"), CorporateReminder.StartDate,CorporateReminder.EndDate,CorporateReminder.RawHtml.Replace("'", "''"), CorporateReminder.Recipiants,CorporateReminder.Status,CorporateReminder.Type);
                lstCorporateReminders = context.Query<CorporateReminder>(sql).ToList();
            }
            return lstCorporateReminders.FirstOrDefault();
        }

        public static CorporateReminder Update(CorporateReminder CorporateReminder)
        {
            List<CorporateReminder> lstCorporateReminders = new List<CorporateReminder>();
            using (var context = Exigo.Sql())
            {
                var sql = string.Format(@"UpdateCorporateReminder '{0}','{1}','{2}','{3}','{4}',{5},{6},{7},{8}", 
                    CorporateReminder.Name.Replace("'", "''"), CorporateReminder.Description.Replace("'", "''"), CorporateReminder.StartDate,CorporateReminder.EndDate,CorporateReminder.RawHtml.Replace("'", "''"), CorporateReminder.Recipiants,CorporateReminder.Status,CorporateReminder.Type,CorporateReminder.CorporateReminderID);
                lstCorporateReminders = context.Query<CorporateReminder>(sql).ToList();
            }
            return lstCorporateReminders.FirstOrDefault();
        }

        public static CorporateReminder Archive(int CorporateReminderId, bool status)
        {
            try
            {
                List<CorporateReminder> lstCorporateReminders = new List<CorporateReminder>();
                using (var context = Exigo.Sql())
                {
                    var sql = string.Format(@"UpdateCorporateReminderStatus {0},{1}", status, CorporateReminderId);
                    lstCorporateReminders = context.Query<CorporateReminder>(sql).ToList();
                }
                return lstCorporateReminders.FirstOrDefault();
            }
            catch (Exception)
            {
                return new CorporateReminder();
            }
            
        }


        public static bool ArchiveCustomer(int CorporateReminderId, int customerId)
        {
            try
            {
                var api = Exigo.WebService();
                var response = api.CreateCustomerExtended(new CreateCustomerExtendedRequest
                {
                    CustomerID = customerId,
                    ExtendedGroupID = (int)CustomerExtendedGroup.CorporateReminder,
                    Field1 = CorporateReminderId.ToString()
                });
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }

    }
}