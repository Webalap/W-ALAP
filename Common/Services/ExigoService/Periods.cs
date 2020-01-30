using System.Collections.Generic;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static IEnumerable<Period> GetPeriods(GetPeriodsRequest request)
        {
            #region ODATA Query
            //var context = Exigo.OData();           
            //// Setup the query
            //var query = context.Periods
            //    .Where(c => c.PeriodTypeID == request.PeriodTypeID);

            //if (request.PeriodIDs.Length > 0)
            //{
            //    query = query.Where(request.PeriodIDs.ToList().ToOrExpression<Common.Api.ExigoOData.Period, int>("PeriodID"));
            //    PeriodIDs = string.Join(",", request.PeriodIDs);
            //}

            //// Optionally filter by the customer.
            //// If the customer is provided, only periods the customer was a part of will be returned.
            //if (request.CustomerID != null)
            //{
            //    CustomerID = (int)request.CustomerID;
            //    var customer = context.Customers
            //        .Where(c => c.CustomerID == (int)request.CustomerID)
            //        .Select(c => new { c.CreatedDate })
            //        .FirstOrDefault();
            //    if (customer != null)
            //    {
            //        query = query.Where(c => c.EndDate >= customer.CreatedDate);
            //    }
            //}


            //// Get the data
            //var periods = new List<Common.Api.ExigoOData.Period>();

            //int lastResultCount = 50;
            //int callsMade = 0;

            //while (lastResultCount == 50)
            //{
            //    var results = query.Select(c => c)
            //        .Skip(callsMade * 50)
            //        .Take(50)
            //        .Select(c => c)
            //        .ToList();

            //    results.ForEach(c => periods.Add(c));

            //    callsMade++;
            //    lastResultCount = results.Count;
            //}
            //foreach (var period in periods)
            //{
            //    yield return (Period)period;
            //}
            #endregion
            #region SQL Procedure
            string PeriodIDs = string.Empty;
            int CustomerID = 0;
            if (request.PeriodIDs.Length > 0)
            {
                PeriodIDs = string.Join(",", request.PeriodIDs);
            }
            if (request.CustomerID != null)
            {
                CustomerID = (int)request.CustomerID;
            }
            // Get the data
            List<Period> periods = new List<Period>();
            using (var context2 = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetPeriodsByPeriodTypeID {0},'{1}',{2}", request.PeriodTypeID, PeriodIDs, CustomerID);
                periods = context2.Query<Period>(sqlProcedure).ToList();
            }
            return periods;
            #endregion
        }
        public static Period GetCurrentPeriod(int periodTypeID)
        {
           // var cachekey = GlobalSettings.Exigo.Api.CompanyKey + "CurrentPeriod_" + periodTypeID.ToString();
            //if (HttpContext.Current.Cache[cachekey] == null)
            //{
                //var period2 = Exigo.OData().Periods
                //    .Where(c => c.PeriodTypeID == periodTypeID)
                //    .Where(c => c.IsCurrentPeriod)
                //    .FirstOrDefault();

            //    HttpContext.Current.Cache[cachekey] = (Period)period;
            //}
                Common.Api.ExigoOData.Period period = new Common.Api.ExigoOData.Period();
                using (var context = Exigo.Sql())
                {
                    string sqlProcedure = string.Format("Exec GetCurrentPeriod {0}",periodTypeID);
                    period = context.Query<Common.Api.ExigoOData.Period>(sqlProcedure).FirstOrDefault();
                    period.IsCurrentPeriod = true;
                }
                return (Period)period;
        }
    }
}