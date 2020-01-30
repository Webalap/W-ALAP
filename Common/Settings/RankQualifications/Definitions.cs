using System.Collections.Generic;

namespace ExigoService
{
    public static partial class Exigo
    {
        private static readonly IEnumerable<IRankRequirementDefinition> RankQualificationDefinitions = new List<IRankRequirementDefinition>
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
        };
    }
}