﻿using System;

namespace ExigoService
{
    public class VolumeCollection : IVolumeCollection
    {
        public VolumeCollection()
        {
            this.Period = Period.Default;
            this.HighestAchievedRankThisPeriod =Rank.Default; //Rank
            this.PayableAsRank = Rank.Default; //PaidRank
            this.PaidAsRank = Rank.Default;
        }
        public int CustomerID { get; set; }

        public Period Period { get; set; }
        public Rank HighestAchievedRankThisPeriod { get; set; }
        public Rank PayableAsRank { get; set; }
        public Rank PaidAsRank { get; set; }

        #region New Custom Properties
        public int PaidRankID { get; set; }
        public string PaidRankDescription { get; set; }
        public int RankID { get; set; }
        public string RankDescription { get; set; }
        public string ModifiedDate { get; set; }
        public int PeriodID { get; set; }
        public int PeriodTypeID { get; set; }
        public string PeriodDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public Rank Rank
        {
            get
            {
                Rank rank = new Rank(this.RankID, this.RankDescription);
                return rank;
            }
            set { }
        }
        public Rank PaidAsRank1
        {
            get
            {
                Rank rank = new Rank(this.PaidRankID, this.PaidRankDescription);
                return rank;
            }
        }
        public Period Period1
        {
            get
            {
                Period period = new Period();
                period.StartDate = this.StartDate;
                period.EndDate = this.EndDate;
                period.PeriodID = this.PeriodID;
                period.PeriodTypeID = this.PeriodTypeID;
                period.PeriodDescription = this.PeriodDescription;
                return period;
            }
        }

        public static VolumeCollection Default
        {
            get
            {
                return new VolumeCollection
                {
                    PeriodDescription = "Unknown"
                };
            }
        }

        #endregion



        public decimal Volume1 { get; set; }
        public decimal Volume2 { get; set; }
        public decimal Volume3 { get; set; }
        public decimal Volume4 { get; set; }
        public decimal Volume5 { get; set; }
        public decimal Volume6 { get; set; }
        public decimal Volume7 { get; set; }
        public decimal Volume8 { get; set; }
        public decimal Volume9 { get; set; }
        public decimal Volume10 { get; set; }
        public decimal Volume11 { get; set; }
        public decimal Volume12 { get; set; }
        public decimal Volume13 { get; set; }
        public decimal Volume14 { get; set; }
        public decimal Volume15 { get; set; }
        public decimal Volume16 { get; set; }
        public decimal Volume17 { get; set; }
        public decimal Volume18 { get; set; }
        public decimal Volume19 { get; set; }
        public decimal Volume20 { get; set; }
        public decimal Volume21 { get; set; }
        public decimal Volume22 { get; set; }
        public decimal Volume23 { get; set; }
        public decimal Volume24 { get; set; }
        public decimal Volume25 { get; set; }
        public decimal Volume26 { get; set; }
        public decimal Volume27 { get; set; }
        public decimal Volume28 { get; set; }
        public decimal Volume29 { get; set; }
        public decimal Volume30 { get; set; }
        public decimal Volume31 { get; set; }
        public decimal Volume32 { get; set; }
        public decimal Volume33 { get; set; }
        public decimal Volume34 { get; set; }
        public decimal Volume35 { get; set; }
        public decimal Volume36 { get; set; }
        public decimal Volume37 { get; set; }
        public decimal Volume38 { get; set; }
        public decimal Volume39 { get; set; }
        public decimal Volume40 { get; set; }
        public decimal Volume41 { get; set; }
        public decimal Volume42 { get; set; }
        public decimal Volume43 { get; set; }
        public decimal Volume44 { get; set; }
        public decimal Volume45 { get; set; }
        public decimal Volume46 { get; set; }
        public decimal Volume47 { get; set; }
        public decimal Volume48 { get; set; }
        public decimal Volume49 { get; set; }
        public decimal Volume50 { get; set; }
        public decimal Volume51 { get; set; }
        public decimal Volume52 { get; set; }
        public decimal Volume53 { get; set; }
        public decimal Volume54 { get; set; }
        public decimal Volume55 { get; set; }
        public decimal Volume56 { get; set; }
        public decimal Volume57 { get; set; }
        public decimal Volume58 { get; set; }
        public decimal Volume59 { get; set; }
        public decimal Volume60 { get; set; }
        public decimal Volume61 { get; set; }
        public decimal Volume62 { get; set; }
        public decimal Volume63 { get; set; }
        public decimal Volume64 { get; set; }
        public decimal Volume65 { get; set; }
        public decimal Volume66 { get; set; }
        public decimal Volume67 { get; set; }
        public decimal Volume68 { get; set; }
        public decimal Volume69 { get; set; }
        public decimal Volume70 { get; set; }
        public decimal Volume71 { get; set; }
        public decimal Volume72 { get; set; }
        public decimal Volume73 { get; set; }
        public decimal Volume74 { get; set; }
        public decimal Volume75 { get; set; }
        public decimal Volume76 { get; set; }
        public decimal Volume77 { get; set; }
        public decimal Volume78 { get; set; }
        public decimal Volume79 { get; set; }
        public decimal Volume80 { get; set; }
        public decimal Volume81 { get; set; }
        public decimal Volume82 { get; set; }
        public decimal Volume83 { get; set; }
        public decimal Volume84 { get; set; }
        public decimal Volume85 { get; set; }
        public decimal Volume86 { get; set; }
        public decimal Volume87 { get; set; }
        public decimal Volume88 { get; set; }
        public decimal Volume89 { get; set; }
        public decimal Volume90 { get; set; }
        public decimal Volume91 { get; set; }
        public decimal Volume92 { get; set; }
        public decimal Volume93 { get; set; }
        public decimal Volume94 { get; set; }
        public decimal Volume95 { get; set; }
        public decimal Volume96 { get; set; }
        public decimal Volume97 { get; set; }
        public decimal Volume98 { get; set; }
        public decimal Volume99 { get; set; }
        public decimal Volume100 { get; set; }
        public decimal Volume101 { get; set; }
        public decimal Volume102 { get; set; }
        public decimal Volume103 { get; set; }
        public decimal Volume104 { get; set; }
        public decimal Volume105 { get; set; }
        public decimal Volume106 { get; set; }
        public decimal Volume107 { get; set; }
        public decimal Volume108 { get; set; }
        public decimal Volume109 { get; set; }
        public decimal Volume110 { get; set; }
        public decimal Volume111 { get; set; }
        public decimal Volume112 { get; set; }
        public decimal Volume113 { get; set; }
        public decimal Volume114 { get; set; }
        public decimal Volume115 { get; set; }
        public decimal Volume116 { get; set; }
        public decimal Volume117 { get; set; }
        public decimal Volume118 { get; set; }
        public decimal Volume119 { get; set; }
        public decimal Volume120 { get; set; }
        public decimal Volume121 { get; set; }
        public decimal Volume122 { get; set; }
        public decimal Volume123 { get; set; }
        public decimal Volume124 { get; set; }
        public decimal Volume125 { get; set; }
        public decimal Volume126 { get; set; }
        public decimal Volume127 { get; set; }
        public decimal Volume128 { get; set; }
        public decimal Volume129 { get; set; }
        public decimal Volume130 { get; set; }
        public decimal Volume131 { get; set; }
        public decimal Volume132 { get; set; }
        public decimal Volume133 { get; set; }
        public decimal Volume134 { get; set; }
        public decimal Volume135 { get; set; }
        public decimal Volume136 { get; set; }
        public decimal Volume137 { get; set; }
        public decimal Volume138 { get; set; }
        public decimal Volume139 { get; set; }
        public decimal Volume140 { get; set; }
        public decimal Volume141 { get; set; }
        public decimal Volume142 { get; set; }
        public decimal Volume143 { get; set; }
        public decimal Volume144 { get; set; }
        public decimal Volume145 { get; set; }
        public decimal Volume146 { get; set; }
        public decimal Volume147 { get; set; }
        public decimal Volume148 { get; set; }
        public decimal Volume149 { get; set; }
        public decimal Volume150 { get; set; }
        public decimal Volume151 { get; set; }
        public decimal Volume152 { get; set; }
        public decimal Volume153 { get; set; }
        public decimal Volume154 { get; set; }
        public decimal Volume155 { get; set; }
        public decimal Volume156 { get; set; }
        public decimal Volume157 { get; set; }
        public decimal Volume158 { get; set; }
        public decimal Volume159 { get; set; }
        public decimal Volume160 { get; set; }
        public decimal Volume161 { get; set; }
        public decimal Volume162 { get; set; }
        public decimal Volume163 { get; set; }
        public decimal Volume164 { get; set; }
        public decimal Volume165 { get; set; }
        public decimal Volume166 { get; set; }
        public decimal Volume167 { get; set; }
        public decimal Volume168 { get; set; }
        public decimal Volume169 { get; set; }
        public decimal Volume170 { get; set; }
        public decimal Volume171 { get; set; }
        public decimal Volume172 { get; set; }
        public decimal Volume173 { get; set; }
        public decimal Volume174 { get; set; }
        public decimal Volume175 { get; set; }
        public decimal Volume176 { get; set; }
        public decimal Volume177 { get; set; }
        public decimal Volume178 { get; set; }
        public decimal Volume179 { get; set; }
        public decimal Volume180 { get; set; }
        public decimal Volume181 { get; set; }
        public decimal Volume182 { get; set; }
        public decimal Volume183 { get; set; }
        public decimal Volume184 { get; set; }
        public decimal Volume185 { get; set; }
        public decimal Volume186 { get; set; }
        public decimal Volume187 { get; set; }
        public decimal Volume188 { get; set; }
        public decimal Volume189 { get; set; }
        public decimal Volume190 { get; set; }
        public decimal Volume191 { get; set; }
        public decimal Volume192 { get; set; }
        public decimal Volume193 { get; set; }
        public decimal Volume194 { get; set; }
        public decimal Volume195 { get; set; }
        public decimal Volume196 { get; set; }
        public decimal Volume197 { get; set; }
        public decimal Volume198 { get; set; }
        public decimal Volume199 { get; set; }
        public decimal Volume200 { get; set; }
    }
}