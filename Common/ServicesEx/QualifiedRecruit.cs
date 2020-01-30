using System;

namespace Common.ServicesEx
{
    public class QualifiedRecruit
    {
        public int RecruitID { get; set; }
        public decimal QualifyingVolume { get; set; }
        public DateTime RecruitStartDate { get; set; }
        public DateTime EnrollerStartDate { get; set; }
    }
}