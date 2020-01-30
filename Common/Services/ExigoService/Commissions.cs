using Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static IEnumerable<ICommission> GetCommissionList(int customerID)
        {
            // Historical Commissions
            var historicalCommissions = GetHistoricalCommissionList(customerID);
            foreach (var commission in historicalCommissions)
            {
                yield return commission;
            }
        }

        public static IEnumerable<ICommission> GetHistoricalCommissionList(int customerID)
        {
            List<HistoricalCommission> result = new List<HistoricalCommission>();
            

            using (SqlConnection cnn = Exigo.Sql())
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("dbo.HistoricalCommission", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@customerid", customerID));
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        HistoricalCommission data = new HistoricalCommission();

                        data.CustomerID = (int)rdr["CustomerID"];
                        data.CurrencyCode = (string)rdr["CurrencyCode"];
                        data.Total = (decimal)rdr["Total"];

                        data.Period.PeriodID = (int)rdr["PeriodID"];
                        data.Period.PeriodTypeID = (int)rdr["PeriodTypeID"];
                        data.Period.PeriodDescription = (string)rdr["PeriodDescription"];
                        data.Period.StartDate = (DateTime)rdr["StartDate"];
                        data.Period.EndDate = (DateTime)rdr["EndDate"];

                        data.CommissionRunID = (int)rdr["CommissionRunID"];
                        data.Earnings = (decimal)rdr["Earnings"];
                        data.PreviousBalance = (decimal)rdr["PreviousBalance"];
                        data.BalanceForward = (decimal)rdr["BalanceForward"];
                        data.Fee = (decimal)rdr["Fee"];

                        data.PaidRank.RankID = (int)rdr["PaidRankID"];
                        data.PaidRank.RankDescription = (string)rdr["PaidRankDescription"];

                        for (int i = 1; i <= 200; i++)
                        {
                            PropertyInfo Volume = data.Volumes.GetType().GetProperty("Volume" + i);
                            object p = (object)data.Volumes;
                            Volume.SetValue(p, rdr["Volume"+ i]);
                        }
                        data.Volumes.CustomerID = (int)rdr["CustomerID"];
                        data.Volumes.EndDate = (DateTime)rdr["EndDate"];
                        data.Volumes.StartDate = (DateTime)rdr["StartDate"];
                        data.Volumes.PeriodTypeID = (int)rdr["PeriodTypeID"];
                        data.Volumes.PeriodID = (int)rdr["PeriodID"];
                        data.Volumes.PeriodDescription = (string)rdr["PeriodDescription"];
                        data.Volumes.PaidRankID = (int)rdr["PaidRankID"];
                        data.Volumes.PaidRankDescription = (string)rdr["PaidRankDescription"];

                        result.Add( data );                       
                    }
                }
                return result;
            }
        
            //// Historical Commissions
            //var commissions = Exigo.OData().Commissions.Expand("CommissionRun/Period").Expand("PaidRank")
            //    .Where(c => c.CustomerID == customerID)
            //    .OrderByDescending(c => c.CommissionRunID);
            //if (commissions != null)
            //{
            //    foreach (var commission in commissions)
            //    {
            //        yield return (HistoricalCommission)commission;
            //    }
            //}
        }
        public static List<CommissionBonusDetail> GetCustomerHistoricalCommissionList(int customerID,int RunId,string WhereClause,string OrderbyClause,int Skip) {

            List<CommissionBonusDetail> data = new List<CommissionBonusDetail>();
            using (var context = Exigo.Sql())
            {
                if (WhereClause.Contains("FromCustomerName"))
                {
                    WhereClause = WhereClause.Replace("FromCustomerName", " Concat(c.FirstName,' ',c.LastName)");
                }
                string sqlProcedure2 = string.Format("exec GetCommissionBonusDetail {0},{1},'{2}','{3}'", customerID, RunId, WhereClause.Replace("'", "''"), OrderbyClause);
                data = context.Query<CommissionBonusDetail>(sqlProcedure2).ToList();
                
            }
            return data; 
        }

        public static IEnumerable<ICommission> GetCommissionPeriodList(int customerID)
        {
            List<HistoricalCommission> result = new List<HistoricalCommission>();


            using (SqlConnection cnn = Exigo.Sql())
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("dbo.HistoricalCommission", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@customerid", customerID));
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        HistoricalCommission data = new HistoricalCommission();

                        data.CustomerID = (int)rdr["CustomerID"];
                        data.CurrencyCode = (string)rdr["CurrencyCode"];
                        data.Total = (decimal)rdr["Total"];

                        data.Period.PeriodID = (int)rdr["PeriodID"];
                        data.Period.PeriodTypeID = (int)rdr["PeriodTypeID"];
                        data.Period.PeriodDescription = (string)rdr["PeriodDescription"];
                        data.Period.StartDate = (DateTime)rdr["StartDate"];
                        data.Period.EndDate = (DateTime)rdr["EndDate"];

                        data.CommissionRunID = (int)rdr["CommissionRunID"];
                        data.Earnings = (decimal)rdr["Earnings"];
                        data.PreviousBalance = (decimal)rdr["PreviousBalance"];
                        data.BalanceForward = (decimal)rdr["BalanceForward"];
                        data.Fee = (decimal)rdr["Fee"];

                        data.PaidRank.RankID = (int)rdr["PaidRankID"];
                        data.PaidRank.RankDescription = (string)rdr["PaidRankDescription"];

                        for (int i = 1; i <= 200; i++)
                        {
                            PropertyInfo Volume = data.Volumes.GetType().GetProperty("Volume" + i);
                            object p = (object)data.Volumes;
                            Volume.SetValue(p, rdr["Volume" + i]);
                        }
                        data.Volumes.CustomerID = (int)rdr["CustomerID"];
                        data.Volumes.EndDate = (DateTime)rdr["EndDate"];
                        data.Volumes.StartDate = (DateTime)rdr["StartDate"];
                        data.Volumes.PeriodTypeID = (int)rdr["PeriodTypeID"];
                        data.Volumes.PeriodID = (int)rdr["PeriodID"];
                        data.Volumes.PeriodDescription = (string)rdr["PeriodDescription"];
                        data.Volumes.PaidRankID = (int)rdr["PaidRankID"];
                        data.Volumes.PaidRankDescription = (string)rdr["PaidRankDescription"];

                        result.Add(data);
                    }
                }
                return result;
            }
            //// Historical Commissions
            //var commissions = Exigo.OData().Commissions.Expand("CommissionRun/Period")
            //    .Where(c => c.CustomerID == customerID)
            //    .OrderByDescending(c => c.CommissionRunID);
            //var maxRankID = commissions.ToList().Max(i => i.PaidRankID);
            //TODO: following procedure is working fine except PaidAsRankID need to get

            //List<Common.Api.ExigoOData.Commission> lstCommision;
            //using (var sqlcontext = Exigo.Sql())
            //{
            //    sqlcontext.Open();
            //    string sqlProcedure = string.Format(@"GetCommissionPeriodList {0}",customerID);
            //    lstCommision = sqlcontext.Query<Common.Api.ExigoOData.Commission, Common.Api.ExigoOData.CommissionRun, Common.Api.ExigoOData.Period, Common.Api.ExigoOData.Commission>(sqlProcedure, (commission, commissionRun,period) =>
            //    {
            //        commission.CommissionRun = commissionRun;
            //        commission.CommissionRun.Period = period;
            //        return commission;
            //    }, splitOn: "CommissionRunID,PeriodID").ToList();

            //    sqlcontext.Close();
            //}
            //if (commissions != null)
            //{
            //    foreach (var commission in commissions)
            //    {
            //        yield return (HistoricalCommission)commission;
            //    }
            //}
        }

        public static HistoricalCommission GetCustomerHistoricalCommission(int customerID, int commissionRunID)
        {
            List<HistoricalCommission> result = new List<HistoricalCommission>();


            using (SqlConnection cnn = Exigo.Sql())
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("dbo.HistoricalCommission", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@customerid", customerID));
                cmd.Parameters.Add(new SqlParameter("@commissionRunId", commissionRunID));

                HistoricalCommission data = new HistoricalCommission();

                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {


                        data.CustomerID = (int)rdr["CustomerID"];
                        data.CurrencyCode = (string)rdr["CurrencyCode"];
                        data.Total = (decimal)rdr["Total"];

                        data.Period.PeriodID = (int)rdr["PeriodID"];
                        data.Period.PeriodTypeID = (int)rdr["PeriodTypeID"];
                        data.Period.PeriodDescription = (string)rdr["PeriodDescription"];
                        data.Period.StartDate = (DateTime)rdr["StartDate"];
                        data.Period.EndDate = (DateTime)rdr["EndDate"];

                        data.CommissionRunID = (int)rdr["CommissionRunID"];
                        data.Earnings = (decimal)rdr["Earnings"];
                        data.PreviousBalance = (decimal)rdr["PreviousBalance"];
                        data.BalanceForward = (decimal)rdr["BalanceForward"];
                        data.Fee = (decimal)rdr["Fee"];

                        data.PaidRank.RankID = (int)rdr["PaidRankID"];
                        data.PaidRank.RankDescription = (string)rdr["PaidRankDescription"];

                        for (int i = 1; i <= 200; i++)
                        {
                            PropertyInfo Volume = data.Volumes.GetType().GetProperty("Volume" + i);
                            object p = (object)data.Volumes;
                            Volume.SetValue(p, rdr["Volume" + i]);
                        }
                        data.Volumes.CustomerID = (int)rdr["CustomerID"];
                        data.Volumes.EndDate = (DateTime)rdr["EndDate"];
                        data.Volumes.StartDate = (DateTime)rdr["StartDate"];
                        data.Volumes.PeriodTypeID = (int)rdr["PeriodTypeID"];
                        data.Volumes.PeriodID = (int)rdr["PeriodID"];
                        data.Volumes.PeriodDescription = (string)rdr["PeriodDescription"];
                        data.Volumes.PaidRankID = (int)rdr["PaidRankID"];
                        data.Volumes.PaidRankDescription = (string)rdr["PaidRankDescription"];

                    }
                }
                //return result;
                //// Get the commission record
                //var commission = Exigo.OData().Commissions.Expand("CommissionRun/Period").Expand("PaidRank")
                //    .Where(c => c.CustomerID == customerID)
                //    .Where(c => c.CommissionRunID == commissionRunID)
                //    .FirstOrDefault();
                //if (commission == null) return null;
                //var result = (HistoricalCommission)commission;

                bool isPeriodMonthly = data.Period.PeriodTypeID == PeriodTypes.Monthly;

                GetCustomerVolumesRequest getCustomerVolumesRequest = new GetCustomerVolumesRequest
                {
                    CustomerID = customerID,
                    PeriodID = data.Period.PeriodID,
                    PeriodTypeID = PeriodTypes.Monthly,
                    VolumeIDs = new int[] { 3, 9, 10, 12, 13, 14 }
                };

                if (!isPeriodMonthly)
                {
                    // Override the result volumes to ensure the numbers are always monthly
                    getCustomerVolumesRequest.PeriodID = null;
                    getCustomerVolumesRequest.ReportForMonth = data.Period.EndDate;
                }

                data.Volumes = GetCustomerVolumes(getCustomerVolumesRequest);

                return data;
            }
        }





        public static IEnumerable<RealTimeCommission> GetCustomerRealTimeCommissions(GetCustomerRealTimeCommissionsRequest request)
        {
            var results = new List<RealTimeCommission>();

            // Get the commission record
            var realtimeresponse = Exigo.WebService().GetRealTimeCommissions(new Common.Api.ExigoWebService.GetRealTimeCommissionsRequest
            {
                CustomerID = request.CustomerID
            });
            if (realtimeresponse.Commissions.Length == 0) return results;


            // Get the unique periods for each of the commission results
            var periods = new List<Period>();
            var periodRequests = new List<GetPeriodsRequest>();
            foreach (var commissionResponse in realtimeresponse.Commissions)
            {
                var periodID = commissionResponse.PeriodID;
                var periodTypeID = commissionResponse.PeriodType;

                var req = periodRequests.Where(c => c.PeriodTypeID == periodTypeID).FirstOrDefault();
                if (req == null)
                {
                    periodRequests.Add(new GetPeriodsRequest()
                    {
                        PeriodTypeID = periodTypeID,
                        PeriodIDs = new int[] { periodID }
                    });
                }
                else
                {
                    var ids = req.PeriodIDs.ToList();
                    ids.Add(periodID);
                    req.PeriodIDs = ids.Distinct().ToArray();
                }
            }

            periodRequests.ForEach(s => GetPeriods(s).ToList().ForEach(a => periods.Add(a)));
            //replaced the below loops with above line.
            //foreach (var req in periodRequests)
            //{
            //    var responses = GetPeriods(req);
            //    foreach (var response in responses)
            //    {
            //        periods.Add(response);
            //    }
            //}

            // Process each commission response 
            try
            {
                foreach (var commission in realtimeresponse.Commissions)
                {
                    var typedCommission = (RealTimeCommission)commission;

                    typedCommission.Period = periods
                        .Where(c => c.PeriodID == commission.PeriodID && c.PeriodTypeID == commission.PeriodType)
                        .FirstOrDefault();
                   
                    //typedCommission.Period = periods
                    //    .Where(c => c.PeriodTypeID == commission.PeriodType)
                    //    .Where(c => c.PeriodID == commission.PeriodID)
                    //    .FirstOrDefault();
                    

                    bool isPeriodMonthly = typedCommission.Period.PeriodTypeID == PeriodTypes.Monthly;

                    GetCustomerVolumesRequest getCustomerVolumesRequest = new GetCustomerVolumesRequest
                    {
                        CustomerID = request.CustomerID,
                        PeriodID = typedCommission.Period.PeriodID,
                        PeriodTypeID = PeriodTypes.Monthly,
                        VolumeIDs = request.VolumeIDs
                    };

                    if (!isPeriodMonthly)
                    {
                        getCustomerVolumesRequest.PeriodID = null;
                        getCustomerVolumesRequest.ReportForMonth = typedCommission.Period.EndDate;// period.EndDate;
                    }

                    typedCommission.Volumes = GetCustomerVolumes(getCustomerVolumesRequest);

                    typedCommission.PaidRank = typedCommission.Volumes.PayableAsRank;

                    results.Add(typedCommission);
                }

                return results.OrderByDescending(c => c.Period.StartDate);
            }
            catch { return results; }
        }
    }
}