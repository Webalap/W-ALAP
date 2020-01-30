using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExigoService
{
    public interface IRankRequirementDefinition
    {
        string Expression { get; set; }
        string Label { get; set; }
        RankQualificationType Type { get; set; }
        string RequirementDescription { get; set; }
        string QualifiedDescription { get; set; }
        string NotQualifiedDescription { get; set; }
        QualificationResponse Data { get; set; }
    }

    public enum RankQualificationType
    {
        Boolean = 1,
        Decimal = 2
    }
}