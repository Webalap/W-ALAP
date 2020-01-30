using System;
using System.Diagnostics.Contracts;


namespace Common.Models.ExigoService.PointsProgram
{

    public sealed class GetCustomerPointsProgramSummaryResponse {

        private readonly PointsProgramSummary    _summary;
        private readonly decimal                 _totalPercentComplete;
        private readonly String                  _errorMessage;


        public GetCustomerPointsProgramSummaryResponse( 
                                    PointsProgramSummary    summary, 
                                    String                  errorMessage        = null
        ) {
            decimal goal = 100;
            decimal progress = (decimal)(summary.TotalPoints / goal) * 100;
            Contract.Requires( summary != null );

            _summary                = summary;
            _totalPercentComplete   = Math.Min( progress , 100 );
            _errorMessage           = errorMessage;

        }


        public PointsProgramSummary Summary {
            get { return _summary; }
        }

        public decimal TotalPercentComplete {
            get { return _totalPercentComplete; }
        }

        public String ErrorMessage {
            get { return _errorMessage; }
        }



        public override String ToString() {

            return String.Format( "CustomerId = {0}, TotalPercentComplete = {1}, ErrorMessage = {2}", Summary.CustomerId, TotalPercentComplete, ErrorMessage );

        }

    }

}
