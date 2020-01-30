using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static GetCustomerRankQualificationsResponse GetCustomerRankQualifications(int customerID, int periodTypeID)
        {
            return GetCustomerRankQualifications(new GetCustomerRankQualificationsRequest
            {
                CustomerID = customerID,
                PeriodTypeID = periodTypeID
            });
        }
        public static GetCustomerRankQualificationsResponse GetCustomerRankQualifications(GetCustomerRankQualificationsRequest request)
        
        {
            // Get the rank qualifications from the API
            var apiResponse = new GetRankQualificationsResponse();
            try
            {
                var apiRequest = new GetRankQualificationsRequest
                {
                    CustomerID = request.CustomerID,
                    PeriodType = request.PeriodTypeID
                };
                apiRequest.RankID = request.RankID != null ? (int)request.RankID : 0;

                apiResponse = Exigo.WebService().GetRankQualifications(apiRequest);
            }
            catch(Exception exception)
            {
                return new GetCustomerRankQualificationsResponse()
                {
                    IsUnavailable = exception.Message.ToUpper().Contains("UNAVAILABLE"),
                    ErrorMessage = exception.Message
                };
            }


            // Create the response
            var adjustedTotalPercentComplete = (apiResponse.Score > 99.1M && apiResponse.Score < 100 ? 99 : apiResponse.Score);
            var response = new GetCustomerRankQualificationsResponse()
            {
                TotalPercentComplete = adjustedTotalPercentComplete,
                IsQualified          = (apiResponse.Qualifies || (apiResponse.QualifiesOverride != null && ((bool)apiResponse.QualifiesOverride) == true)),
                Rank                 = new Rank()
                {
                    RankID           = apiResponse.RankID,
                    RankDescription  = apiResponse.RankDescription
                }
            };
            if (apiResponse.BackRankID != null)
            {
                response.PreviousRank = new Rank()
                {
                    RankID          = (int)apiResponse.BackRankID,
                    RankDescription = apiResponse.BackRankDescription
                };
            }
            if (apiResponse.NextRankID != null)
            {
                response.NextRank = new Rank()
                {
                    RankID          = (int)apiResponse.NextRankID,
                    RankDescription = apiResponse.NextRankDescription
                };
            }


            // Loop through each leg and create our responses
            var legs = new List<RankQualificationLeg>();
            foreach (var qualificationLeg in apiResponse.PayeeQualificationLegs)
            {
                var leg = new RankQualificationLeg();

                // Assemble the requirements
                var results = new List<RankRequirement>();
              foreach (var qualification in qualificationLeg)
               {
                   var requirement = new RankRequirement(qualification);
                    results.Add(requirement);
                }

                //// Exigo: Depricated - Assemble the requirements
                //var results = new List<RankRequirement>();
                //foreach (var definition in RankQualificationDefinitions)
                //{
                //    var requirement = GetRequirement(qualificationLeg, definition);
                //    if (requirement != null)
                //    {
                //        requirement.RequirementDescription  = GlobalUtilities.MergeFields(requirement.RequirementDescription, requirement, "UNABLE TO MERGE FIELDS");
                //        requirement.QualifiedDescription    = GlobalUtilities.MergeFields(requirement.QualifiedDescription, requirement, "UNABLE TO MERGE FIELDS");
                //        requirement.NotQualifiedDescription = GlobalUtilities.MergeFields(requirement.NotQualifiedDescription, requirement, "UNABLE TO MERGE FIELDS");

                //        results.Add(requirement);
                //    }
                //}

                // Clean up nulls
                results.RemoveAll(c => string.IsNullOrEmpty(c.RequiredValue));
                leg.Requirements = results;


                legs.Add(leg);
            }
            response.QualificationLegs = legs;


            return response;
        }

        #region Helper Methods
        private static BooleanRankRequirementDefinition Boolean(string label, string Description = "", string Expression = "", string Qualified = "", string NotQualified = "")
        {
            return new BooleanRankRequirementDefinition
            {
                Label                   = label,
                Expression              = Expression,
                RequirementDescription  = Description,
                QualifiedDescription    = Qualified,
                NotQualifiedDescription = NotQualified
            };
        }
        private static DecimalRankRequirementDefinition Decimal(string label, string Description = "", string Expression = "", string Qualified = "", string NotQualified = "")
        {
            return new DecimalRankRequirementDefinition
            {
                Label                   = label,
                Expression              = Expression,
                RequirementDescription  = Description,
                QualifiedDescription    = Qualified,
                NotQualifiedDescription = NotQualified
            };
        }
        private static RankRequirement GetRequirement(QualificationResponse[] qualifications, IRankRequirementDefinition definition)
        {
            Regex regex = new Regex(definition.Expression);
            QualificationResponse qualification = null;

            qualification = (from q in qualifications
                             let description = q.QualificationDescription.ToUpper()
                             let matches = regex.Matches(description)
                             where matches.Count > 0
                             select q).FirstOrDefault();

            if (qualification != null)
            {
                return new RankRequirement(qualification, definition);
            }
            else
            {
                return null;
            }
        }
        #endregion
    }
}