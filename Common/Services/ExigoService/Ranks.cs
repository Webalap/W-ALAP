using Common;
using Common.Kendo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static IEnumerable<Rank> GetRanks()
        {
            //var context = Exigo.OData();
            //var apiRanks = context.Ranks.Where(s => s.RankID > 0).OrderBy(c => c.RankID);

            //foreach (var apiRank in apiRanks)
            //{
            //    yield return (Rank)apiRank;
            //}
            using (var context = Exigo.Sql())
            {
                string sqlProcedure = @"GetAllRanks";
                return context.Query<Rank>(sqlProcedure).ToList();
            }
        }
        public static Rank GetRank(int rankID)
        {
            return GetRanks()
                .Where(c => c.RankID == rankID)
                .FirstOrDefault();
        }

        public static IEnumerable<Rank> GetNextRanks(int rankID)
        {
            return GetRanks()
                .Where(c => c.RankID > rankID)
                .OrderBy(c => c.RankID)
                .ToList();
        }
        public static Rank GetNextRank(int rankID)
        {
            return GetNextRanks(rankID).FirstOrDefault();
        }

        public static IEnumerable<Rank> GetPreviousRanks(int rankID)
        {
            return GetRanks()
                .Where(c => c.RankID < rankID)
                .OrderByDescending(c => c.RankID)
                .ToList();
        }
        public static Rank GetPreviousRank(int rankID)
        {
            return GetPreviousRanks(rankID).FirstOrDefault();
        }

        public static CustomerRankCollection GetCustomerRanks(GetCustomerRanksRequest request)
        {
            var result = new CustomerRankCollection();

            int? periodId;
            int periodtypeId;
            int customerId;

            if (request.PeriodID != null)
            {
                periodId = request.PeriodID;
            }
            else
            {
                periodId = Exigo.GetCurrentPeriod(PeriodTypes.Monthly).PeriodID;
            }

            
            periodtypeId = request.PeriodTypeID;
            

            
            customerId = request.CustomerID;

   

            using (SqlConnection cnn = Exigo.Sql())
            {
                cnn.Open();
                SqlCommand cmd = new SqlCommand("dbo.GetCustomerRankCollection", cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(new SqlParameter("@customerid", request.CustomerID));
                cmd.Parameters.Add(new SqlParameter("@periodtypeid", periodtypeId));
                cmd.Parameters.Add(new SqlParameter("@periodid", periodId));
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        result.HighestPaidRankInAnyPeriod.RankID = (int)rdr["RankID"];
                        result.HighestPaidRankInAnyPeriod.RankDescription = (string)rdr["RankDescription"];

                        result.CurrentPeriodRank.RankID = (int)rdr["PeriodPaidRankId"];
                        result.CurrentPeriodRank.RankDescription = (string)rdr["PeriodPaidRankDescription"];

                        result.HighestPaidRankUpToPeriod.RankID = (int)rdr["PeriodRankId"];
                        result.HighestPaidRankUpToPeriod.RankDescription = (string)rdr["PeriodRankDescription"];
                    }
                }
            }        
         
            Rank highestRankAchieved = null;
            using (var context2 = Exigo.Sql())
            {
                string sqlProcedure = string.Format("GetCustomerRank {0}", request.CustomerID);
                highestRankAchieved = context2.Query<Rank>(sqlProcedure).FirstOrDefault();
                context2.Close();
            }
            if (highestRankAchieved != null)
            {
               result.HighestPaidRankInAnyPeriod = (Rank)highestRankAchieved;
                    
            }

            List<VolumeCollection> query = null;
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("GetPeriodVolumes {0},{1},'{2}'", request.CustomerID, request.PeriodTypeID, DateTime.Now);
                query = context.Query<VolumeCollection>(SqlProcedure).ToList();

            }
            if (request.PeriodID != null)
            {
                query = query.Where(c => c.PeriodID == request.PeriodID).ToList();
            }
            else
            {
                query = query.Where(c => c.Period.PeriodID == GetCurrentPeriod(request.PeriodTypeID).PeriodID).ToList();
            }

            var periodRanks = query.Select(c => new
            {
                c.Rank,
                c.PaidAsRank
            }).FirstOrDefault();

            if (periodRanks != null)
            {
                if (periodRanks.PaidAsRank != null)
                {
                    result.CurrentPeriodRank = (Rank)periodRanks.PaidAsRank;
                }
                if (periodRanks.Rank != null)
                {
                    result.HighestPaidRankUpToPeriod = (Rank)periodRanks.Rank;
                }
            }

            return result;
        }

        public static GetDownlineUpcomingPromotionsResponse GetDownlineUpcomingPromotions(GetDownlineUpcomingPromotionsRequest request)
        {
            var response = new GetDownlineUpcomingPromotionsResponse();


            // Determine if we passed a KendoGridRequest. 
            // If we did, use the page and pagesize from the request instead, as it takes priority due to it's unique implications.
            var whereClause = string.Empty;
            if (request.KendoGridRequest != null)
            {
                request.Page = request.KendoGridRequest.Page;
                request.RowCount = request.KendoGridRequest.PageSize;
                request.TotalRowCount = request.KendoGridRequest.Total;
                whereClause = request.KendoGridRequest.SqlWhereClause;
                whereClause = whereClause.Replace("RankScore", "Score");
                whereClause = whereClause.Replace("CustomerID", "c.CustomerID");
            }

            var results = new List<CustomerRankScore>();

            //calling store Procedure "GetDownlineUpcomingPromotions"
            int periodTypeID = request.PeriodTypeID;
            int downlineCustomerID = request.DownlineCustomerID;
            string strRankID = (request.RankID != null) ? "AND PaidRankID = " + request.RankID + @"" : "0";
            int skip = request.Skip;
            int take = request.Take;
            string sortingOrder = " ORDER BY ";
            //need to append defualt sorting in query
            sortingOrder += KendoUtilities.GetSqlOrderByClause((request.KendoGridRequest != null) ? request.KendoGridRequest.SortObjects :
                                new List<SortObject>(),
                                new SortObject("TotalScore", "DESC"),
                                new SortObject("c.CreatedDate", "ASC"));
                using (var sqlcontext = Exigo.Sql())
                {
                    sqlcontext.Open();
                    string sqlProcedure = string.Format(@"GetDownlineUpcomingPromotions {0},{1},'{2}','{3}','{4}',{5},{6}", periodTypeID, downlineCustomerID, strRankID, whereClause.Replace("'", "''"), sortingOrder, skip, take);
                    results = sqlcontext.Query<CustomerRankScore, Rank, CustomerRankScore>(sqlProcedure, (customer, rank) =>
                    {
                        customer.Rank = rank;
                        return customer;
                    }, splitOn: "RankID").ToList();

                    sqlcontext.Close();
                    response.CustomerRankScores = results;
                }

            if (request.KendoGridRequest != null && response.CustomerRankScores != null && response.CustomerRankScores.Count()>0)
            {
                response.TotalCount = response.CustomerRankScores.FirstOrDefault().TotalRows;
            }
            else
            {
                response.TotalCount = response.CustomerRankScores.Count();
            }


            return response;
        }
    }
}