using System;
using System.Diagnostics.Contracts;


namespace Common.Models.ExigoService.PointsProgram
{

    public sealed class PointsProgramSummary {

        private readonly int _customerId;
        private readonly int _startPeriodId;

        private readonly int _salesPoints;
        private readonly int _recruitmentPoints;
        private readonly int _promotionPoints;
        private readonly int _developmentPoints;
        private readonly int _totalPoints;


        public PointsProgramSummary(
                        int customerId, 
                        int startPeriodId, 

                        int salesPoints, 
                        int recruitmentPoints, 
                        int promotionPoints, 
                        int developmentPoints, 
                        int totalPoints 
        ) {

            Contract.Requires( customerId > 0 );

            _customerId         = customerId;
            _startPeriodId      = startPeriodId;

            _salesPoints        = salesPoints;
            _recruitmentPoints  = recruitmentPoints;
            _promotionPoints    = promotionPoints;
            _developmentPoints  = developmentPoints;
            _totalPoints        = totalPoints;

        }


        public int CustomerId           { get { return _customerId;         } }
        public int StartPeriodId        { get { return _startPeriodId;      } }

        public int SalesPoints          { get { return _salesPoints;        } }
        public int RecruitmentPoints    { get { return _recruitmentPoints;  } }
        public int PromotionPoints      { get { return _promotionPoints;    } }
        public int DevelopmentPoints    { get { return _developmentPoints;  } }
        public int TotalPoints          { get { return _totalPoints;        } }



        public override String ToString() {

            return String.Format( "CustomerId = {0}, StartPeriodId = {1}, TotalPoints = {2}", CustomerId, StartPeriodId, TotalPoints );

        }

    }

}
