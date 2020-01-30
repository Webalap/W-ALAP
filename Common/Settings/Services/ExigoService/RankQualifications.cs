using Common;
using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace ExigoService
{
    public static partial class Exigo
    {
        private static IEnumerable<IRankRequirementDefinition> Definitions
        {
            get
            {
                if (_definitions == null)
                {
                    _definitions = new List<IRankRequirementDefinition>
                    {
                        Boolean("Customer Type", 
                            Expression: @"^MUST BE A VALID CUSTOMER TYPE",
                            Description: "You must be a Distributor.",
                            Qualified: "",
                            NotQualified: "You are not a Distributor."
                        ),

                        Boolean("Customer Status",  
                            Expression: @"^CUSTOMER STATUS IS ACTIVE",
                            Description: "Your account must be in good standing.",
                            Qualified: "",
                            NotQualified: "You are not an Active Distributor."
                        ),

                        Boolean("Active", 
                            Expression: @"^ACTIVE$", 
                            Description: "You must be considered Active.",
                            Qualified: "",
                            NotQualified: "You must be Active in order to qualify for the next rank."
                        ),

                        Boolean("Qualified", 
                            Expression: @"^MUST BE QUALIFIED$",
                            Description: "You must be qualified to receive commissions.",
                            Qualified: "",
                            NotQualified: "You must be qualified for commissions in order to advance to the next rank."
                        ),

                        Boolean("Enroller Tree", 
                            Expression: @"^DISTRIBUTOR MUST BE IN ENROLLER TREE$",
                            Description: "You must have a current position in the enroller tree.",
                            Qualified: "",
                            NotQualified: "You must have a current position in the enroller tree in order to advance to the next rank."
                        ),

                        Decimal("Lesser Leg Volume", 
                            Expression: @"^\d+ LESSER LEG VOLUME$",
                            Description: "You need at least {{RequiredValueAsDecimal:N0}} volume in your lesser leg.",
                            Qualified: "",
                            NotQualified: "You need at least <strong>{{FormattedAmountNeededToQualify:N0}}</strong> more volume in your lesser leg."
                        ),

                        Decimal("C500 Legs in Enroller Tree", 
                            Expression: @"^\d+ C500 LEGS ENROLLER TREE$",
                            Description: "You must personally enroll at least {{RequiredValueAsDecimal:N0}} C500 distributor(s).",
                            Qualified: "",
                            NotQualified: "You need <strong>{{FormattedAmountNeededToQualify:N0}} more C500 distributor(s) in your enroller tree</strong> to advance to the next rank."
                        ),

                        Decimal("PV", 
                            Expression: @"^\d+ PV$",
                            Description: "You need at least {{RequiredValueAsDecimal:N0}} PV.",
                            Qualified: "",
                            NotQualified: "You need <strong>{{FormattedAmountNeededToQualify:N0}} more PV</strong> to advance."
                        ),

                        Decimal("PV Last Period", 
                            Expression: @"^\d+ PV 1 PERIOD",
                            Description: "You needed at least {{RequiredValueAsDecimal:N0}} PV last period.",
                            Qualified: "",
                            NotQualified: "Your last period didn't have enough PV to advance you."
                        ),

                        Decimal("PV 2 Periods Ago", 
                            Expression: @"^\d+ PV 2 PERIOD",
                            Description: "You needed at least {{RequiredValueAsDecimal:N0}} PV two periods ago.",
                            Qualified: "",
                            NotQualified: "Your PV from two periods ago didn't have enough PV to advance you."
                        ),

                        Decimal("PV 3 Periods Ago", 
                            Expression: @"^\d+ PV 3 PERIOD",
                            Description: "You needed at least {{RequiredValueAsDecimal:N0}} PV three periods ago.",
                            Qualified: "",
                            NotQualified: "Your PV from three periods ago didn't have enough PV to advance you."
                        ),

                        Decimal("Capped Enrollment GPV at 50% per leg", 
                            Expression: @"^\d+ CAPPED ENROLLMENT GROUP PV AT 50% PER LEG$",
                            Description: "You need at least {{RequiredValueAsDecimal:N0}} capped enrollment GPV at 50% per leg this period.",
                            Qualified: "",
                            NotQualified: "Your current capped enrollment GPV at 50% per leg is insufficient. You need <strong>{{FormattedAmountNeededToQualify}} more</strong> to advance"
                        ),

                        Decimal("Capped Enrollment GPV at 50% per leg last period", 
                            Expression: @"^\d+ CAPPED ENROLLMENT GROUP PV AT 50% PER LEG 1 PERIOD",
                            Description: "You needed at least {{RequiredValueAsDecimal:N0}} capped enrollment GPV at 50% per leg last period.",
                            Qualified: "",
                            NotQualified: "Your capped enrollment GPV at 50% per leg last period was insufficient. You needed <strong>{{FormattedAmountNeededToQualify}} more</strong> to advance."
                        ),

                        Decimal("Capped Enrollment GPV at 50% per leg two periods ago", 
                            Expression: @"^\d+ CAPPED ENROLLMENT GROUP PV AT 50% PER LEG 2 PERIOD",
                            Description: "You needed at least {{RequiredValueAsDecimal:N0}} capped enrollment GPV at 50% per leg two periods ago.",
                            Qualified: "",
                            NotQualified: "Your capped enrollment GPV at 50% per leg two periods ago was insufficient. You needed <strong>{{FormattedAmountNeededToQualify}} more</strong> to advance."
                        ),

                        Decimal("Capped Enrollment GPV at 50% per leg three periods ago", 
                            Expression: @"^\d+ CAPPED ENROLLMENT GROUP PV AT 50% PER LEG 3 PERIOD",
                            Description: "You needed at least {{RequiredValueAsDecimal:N0}} capped enrollment GPV at 50% per leg three periods ago.",
                            Qualified: "",
                            NotQualified: "Your capped enrollment GPV at 50% per leg three periods ago was insufficient. You needed <strong>{{FormattedAmountNeededToQualify}} more</strong> to advance."
                        )
                    }.ToArray();
                }
                return _definitions;
            }
        }
        private static IEnumerable<IRankRequirementDefinition> _definitions;

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
                if (request.RankID != null) apiRequest.RankID = (int)request.RankID;

                apiResponse = Exigo.WebService().GetRankQualifications(apiRequest);
            }
            catch(Exception ex)
            {
                return new GetCustomerRankQualificationsResponse()
                {
                    IsUnavailable = ex.Message.ToUpper() == "UNAVAILABLE",
                    ErrorMessage = ex.Message
                };
            }


            // Create the response
            var response = new GetCustomerRankQualificationsResponse()
            {
                TotalPercentComplete = apiResponse.Score,
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
                foreach (var definition in Definitions)
                {
                    var requirement = GetRequirement(qualificationLeg, definition);
                    if (requirement != null)
                    {
                        requirement.RequirementDescription  = GlobalUtilities.MergeFields(requirement.RequirementDescription, requirement, "UNABLE TO MERGE FIELDS");
                        requirement.QualifiedDescription    = GlobalUtilities.MergeFields(requirement.QualifiedDescription, requirement, "UNABLE TO MERGE FIELDS");
                        requirement.NotQualifiedDescription = GlobalUtilities.MergeFields(requirement.NotQualifiedDescription, requirement, "UNABLE TO MERGE FIELDS");

                        results.Add(requirement);
                    }
                }

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