using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static IEnumerable<PointAccount> GetPointAccounts()
        {
            List<PointAccount> pointAccounts = null;
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetPointAccounts");
                pointAccounts = context.Query<PointAccount>(sqlProcedure).ToList();
                context.Close();
            }

            foreach (var pointAccount in pointAccounts)
            {
                yield return (ExigoService.PointAccount)pointAccount;
            }
        }
        public static PointAccount GetPointAccount(int pointAccountID)
        {
          PointAccount pointAccount =  null;
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetPointAccounts");
                pointAccount = context.Query<PointAccount>(sqlProcedure).ToList().Where(c => c.PointAccountID == pointAccountID).FirstOrDefault();
                context.Close();
            }

            if (pointAccount == null) return null;

            return (ExigoService.PointAccount)pointAccount;
        }

        public static IEnumerable<CustomerPointAccount>  GetCustomerPointAccounts(int customerID)
        {
            List<CustomerPointAccount> pointAccounts = null;
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetCustomersPointAccounts {0}", customerID);
                pointAccounts = context.Query<CustomerPointAccount>(sqlProcedure).ToList();
                context.Close();
            }

            foreach (var pointAccount in pointAccounts)
            {
                yield return (ExigoService.CustomerPointAccount)pointAccount;
            }
        }
        public static CustomerPointAccount GetCustomerPointAccount(int customerID, int pointAccountID)
        {
            CustomerPointAccount pointAccount = null;
            using (var context = Sql())
            {
                string sqlProcedure = string.Format("GetPointAccountsBalance {0}, {1}", customerID, pointAccountID);
                pointAccount = context.Query<CustomerPointAccount>(sqlProcedure).FirstOrDefault();
                context.Close();
            }
            if (pointAccount == null) return null;

            return pointAccount;
        }
        public static bool ValidateCustomerHasPointAmount(int customerID, int pointAccountID, decimal pointAmount)
        {
            var pointAccount = GetCustomerPointAccount(customerID, pointAccountID);
            if (pointAccount == null) return false;

            return pointAccount.Balance >= pointAmount;
        }
    }
}