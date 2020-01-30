using Common;
using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using CustomerExtendedGroup = Common.ModelsEx.Shopping.CustomerExtendedGroup;
namespace ExigoService
{
    public static partial class Exigo
    {
        public static VolumeCollection GetCustomerVolumes(GetCustomerVolumesRequest request)
        {
            #region OData
            //var requestedSpecificVolumes = (request.VolumeIDs != null && request.VolumeIDs.Length > 0);

            //    var baseQuery = Exigo.OData().PeriodVolumes;
            //    var query = baseQuery
            //        .Where(c => c.CustomerID == request.CustomerID)
            //        .Where(c => c.PeriodTypeID == request.PeriodTypeID);

            //// Allows for a request for monthly volumes without knowing the exact period id for the month
            //if (request.ReportForMonth.HasValue)
            //{
            //    DateTime reportStart = new DateTime(request.ReportForMonth.Value.Year, request.ReportForMonth.Value.Month, 1);

            //    query = query.Where(c => c.Period.StartDate >= reportStart && c.Period.StartDate <= reportStart.AddDays(1));

            //    // Validation 
            //    if (request.PeriodID != null)
            //    {
            //        throw new Exception("PeriodID and ReportForMonth parameters cannot be specified at the same time.");
            //    }
            //    if (request.PeriodTypeID != PeriodTypes.Monthly)
            //    {
            //        throw new Exception("PeriodType must be Monthly when ReportForMonth parameter is specified.");
            //    }
            //}
            //// Determine which period ID to use
            //else if (request.PeriodID != null)
            //{
            //    query = query.Where(c => c.PeriodID == (int)request.PeriodID);
            //}
            //else
            //{
            //    query = query.Where(c => c.Period.IsCurrentPeriod);
            //}

            //PeriodVolume data;

            //if (!requestedSpecificVolumes)
            //{
            //    data = query.Select(c => new Common.Api.ExigoOData.PeriodVolume()
            //    {
            //        CustomerID   = c.CustomerID,
            //        ModifiedDate = c.ModifiedDate,
            //        PaidRankID   = c.PaidRankID,
            //        PaidRank     = c.PaidRank,
            //        RankID       = c.RankID,
            //        Rank         = c.Rank,
            //        PeriodID     = c.PeriodID,
            //        Period       = c.Period,
            //        PeriodTypeID = c.PeriodTypeID                    
            //    }).FirstOrDefault();
            //}
            //else
            //{
            //    var volumes = new List<string>();
            //    foreach (var id in request.VolumeIDs)
            //    {
            //        volumes.Add("Volume" + id);
            //    }
            //    var select = string.Format("new({0},Period,Rank,PaidRank)", string.Join(",", volumes));
            //    var finalQuery = query;

            //    var url = finalQuery.ToString();
            //    data = Exigo.OData().Execute<Common.Api.ExigoOData.PeriodVolume>(new Uri(url)).FirstOrDefault();
            //}

            //return (VolumeCollection)data;
            #endregion

            #region SQL Procedure Call
            bool IsCurrentPeriod = false;
            if (request.ReportForMonth.HasValue)
            {
                // Validation 
                if (request.PeriodID != null)
                {
                    throw new Exception("PeriodID and ReportForMonth parameters cannot be specified at the same time.");
                }
                if (request.PeriodTypeID != PeriodTypes.Monthly)
                {
                    throw new Exception("PeriodType must be Monthly when ReportForMonth parameter is specified.");
                }
            }

            if (!request.ReportForMonth.HasValue && request.PeriodID == null)
            {
                IsCurrentPeriod = true;
            }

            var finalResult = new VolumeCollection();
            using (var Context = Exigo.Sql())
            {
                string sqlProcedure = string.Format(@"GetCustomerPeriodVolumes {0},{1},{2},'{3}',{4}", request.CustomerID, request.PeriodTypeID, request.PeriodID != null ? request.PeriodID : 0, request.ReportForMonth.HasValue ? new DateTime(request.ReportForMonth.Value.Year, request.ReportForMonth.Value.Month, 1).ToString() : new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).ToString(), IsCurrentPeriod == false ? 0 : 1);
                var result = Context.Query<VolumeCollection>(sqlProcedure).ToList();
                //Selecting Final result
                if (result == null)
                {
                    finalResult.HighestAchievedRankThisPeriod.RankDescription = string.Empty;
                    return finalResult;
                }

                finalResult = result.Select(s => new VolumeCollection()
                {
                    Rank = s.Rank,
                    HighestAchievedRankThisPeriod = s.Rank,
                    PaidAsRank = s.PaidAsRank1,
                    PayableAsRank = s.PaidAsRank1,
                    Period = s.Period1,
                    CustomerID = s.CustomerID,
                    RankID = s.RankID,
                    RankDescription = s.RankDescription,
                    PaidRankID = s.PaidRankID,
                    PaidRankDescription = s.PaidRankDescription,
                    ModifiedDate = s.ModifiedDate,
                    PeriodID = s.PeriodID,
                    Volume1 = s.Volume1,
                    Volume2 = s.Volume2,
                    Volume3 = s.Volume3,
                    Volume9 = s.Volume9,
                    Volume10 = s.Volume10,
                    Volume12 = s.Volume12,
                    Volume13 = s.Volume13,
                    Volume14 = s.Volume14,
                    Volume16 = s.Volume16,
                    Volume17 = s.Volume17,
                    Volume18 = s.Volume18,
                    Volume22 = s.Volume22,
                    PeriodTypeID = s.PeriodTypeID,
                    PeriodDescription = s.PeriodDescription,
                    StartDate = s.StartDate,
                    EndDate = s.EndDate,
                }).FirstOrDefault();
            }
            if (finalResult == null)
            {
                finalResult = new VolumeCollection();
                finalResult.HighestAchievedRankThisPeriod.RankID = 0;
                finalResult.HighestAchievedRankThisPeriod.RankDescription = string.Empty;
                return finalResult;
            }
            return finalResult;
            #endregion

        }
        public static CustomerExtendedDetails GetCustomerClienteleFields(int customerID)
        {
            return Exigo.GetCustomerExtendedDetails(customerID, (int)CustomerExtendedGroup.ClienteleFields).FirstOrDefault();
        }
        public static List<CustomerExtendedDetails> GetCustomerArchiveEb(int customerID)
        {
            return Exigo.GetCustomerExtendedDetails(customerID, (int)CustomerExtendedGroup.EBPhaseReminderArchive).Where(i=>i.CustomerID == customerID).ToList();
        }
        public static void ArchiveEb(int customerID,int duration,int ownerID)
        {
            var createCustomerExRequest = new CreateCustomerExtendedRequest
            {
                CustomerID = ownerID,
                ExtendedGroupID = (int)CustomerExtendedGroup.EBPhaseReminderArchive,
                Field1 = DateTime.Now.ToString(),
                Field2 = customerID.ToString(), //customerID
                Field3 =duration.ToString(), //duration
            };
            var api = Exigo.WebService();
            api.CreateCustomerExtended(createCustomerExRequest);
        }
        //This is currently being used for the Dashboard Widget
        //public static VolumeCollection GetCustomerVolumes2(GetCustomerVolumesRequest request)
        //{
        //    // Establish the query
        //    var query = Exigo.OData().PeriodVolumes
        //        .Where(c => c.CustomerID == request.CustomerID)
        //        .Where(c => c.PeriodTypeID == PeriodTypes.Monthly)
        //        .Where(c => c.Period.IsCurrentPeriod);
        //        //.Where(c => c.Period.StartDate <= DateTime.Now);

        //    // Fetch the data
        //    var data = query.Select(c => new PeriodVolume()
        //    {
        //        CustomerID      = c.CustomerID,               
        //        PeriodID        = c.PeriodID,
        //        Period          = c.Period,
        //        PeriodTypeID    = c.PeriodTypeID,
        //        Volume1         = c.Volume1,
        //        Volume2         = c.Volume2,
        //        Volume3         = c.Volume3,
        //        Volume9         = c.Volume9,
        //        Volume10        = c.Volume10,
        //        Volume12        = c.Volume12,
        //        Volume13        = c.Volume13,
        //        Volume14        = c.Volume14
        //    }).FirstOrDefault();

        //    return (VolumeCollection)data;
        //}

        //This uses an api call rather then OData
        public static VolumeCollection GetCustomerVolumes3(GetCustomerVolumesRequest request)
        {
            var vc = new VolumeCollection();
            //var api = Exigo.WebService();
            ////Create Request
            //var volumeRequest = new GetVolumesRequest
            //{
            //     CustomerID = request.CustomerID,
            //     PeriodType = request.PeriodTypeID
            //};
            ////Get Response
            //var response = api.GetVolumes(volumeRequest).Volumes.FirstOrDefault();

            //// here api call to SQl Server call

            VolumeResponse response = new VolumeResponse();
            using (var context = Exigo.Sql())
            {
                string sqlQuery = string.Format(@"GetCustomerVolumes3 {0},{1}", request.CustomerID, request.PeriodTypeID);
                response = context.Query<VolumeResponse>(sqlQuery).FirstOrDefault();
            }

            if (response != null)
            {
                vc.CustomerID = response.CustomerID;
                vc.PayableAsRank.RankID = response.RankID;
                vc.Period.PeriodDescription = response.PeriodDescription;
                vc.Volume1 = response.Volume1;
                vc.Volume2 = response.Volume2;
                vc.Volume3 = response.Volume3;
                vc.Volume9 = response.Volume9;
                vc.Volume10 = response.Volume10;
                vc.Volume12 = response.Volume12;
                vc.Volume13 = response.Volume13;
                vc.Volume14 = response.Volume14;
                vc.PaidAsRank.RankID = response.PaidRankID;
            };
            return vc;
        }

        #region Recruiting-Reward

        public static decimal GetRecruitingReward(GetPointAccountRequest request)
        {
            var recruitingReward = GetCustomerPointAccount(request.CustomerID, request.PointAccountID);

            return (null != recruitingReward ? recruitingReward.Balance : 0M);
        }

        #endregion

        #region Enrollee-Reward

        public static decimal GetEnrolleeReward(GetPointAccountRequest request)
        {
           var enrolleeReward = GetCustomerPointAccount(request.CustomerID, request.PointAccountID);

            return (null != enrolleeReward ? enrolleeReward.Balance : 0M);
        }

        #endregion
    }
}