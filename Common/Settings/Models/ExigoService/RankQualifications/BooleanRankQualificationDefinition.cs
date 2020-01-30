﻿using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public class BooleanRankRequirementDefinition : IRankRequirementDefinition
    {
        public BooleanRankRequirementDefinition()
        {
            this.Type = RankQualificationType.Boolean;
        }

        public string Expression { get; set; }
        public string Label { get; set; }
        public RankQualificationType Type { get; set; }
        public string RequirementDescription { get; set; }
        public string QualifiedDescription { get; set; }
        public string NotQualifiedDescription { get; set; }
        public QualificationResponse Data { get; set; }
    }
}