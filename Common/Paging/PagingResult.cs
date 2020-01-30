using System;
using System.Diagnostics.Contracts;


namespace Common.Paging
{

    public sealed class PagingResult : IEquatable<PagingResult> {

        public static PagingResult Create( PagingParams pagingParams, int recordCount, int totalRecordCount ) {

            Contract.Requires( recordCount      >= 0 );
            Contract.Requires( totalRecordCount >= 0 );

            if ( totalRecordCount < 1 ) {

                return new PagingResult( pagingParams );

            } else {

                return new PagingResult( pagingParams, recordCount, totalRecordCount );

            }

        }




        private readonly PagingParams   _pagingParams;
        private readonly int            _recordCount;
        private readonly int            _totalRecordCount;
        private readonly int            _firstRecordNum;
        private readonly int            _lastRecordNum;


        private PagingResult( PagingParams pagingParams ) {

            if ( pagingParams == null ) {
                throw new ArgumentNullException( "pagingParams" );
            }


            _pagingParams       = pagingParams;
            _recordCount        = 0;
            _totalRecordCount   = 0;

            _firstRecordNum     = 0;
            _lastRecordNum      = 0;

        }


        private PagingResult( PagingParams pagingParams, int recordCount, int totalRecordCount ) {

            if ( pagingParams == null ) {
                throw new ArgumentNullException( "pagingParams" );
            }

            _pagingParams = pagingParams;


            int pageNumber = _pagingParams.PageNumber;
            int pageSize   = _pagingParams.PageSize;


            if ( totalRecordCount < 1 ) {
                throw new ArgumentOutOfRangeException( "totalRecordCount", totalRecordCount, "Argument must be greater than zero." );
            }

            if ( recordCount < 1 ) {
                throw new ArgumentOutOfRangeException( "recordCount", recordCount, "Argument must be greater than or equal to zero." );
            }

            if ( recordCount > pageSize ) {
                throw new ArgumentOutOfRangeException( "recordCount", recordCount, "Argument must be less than or equal to page size." );
            }

            if ( recordCount > totalRecordCount ) {
                throw new ArgumentOutOfRangeException( "recordCount", "Argument cannot be greater than 'totalRecordCount'." );
            }


            _recordCount        = recordCount;
            _totalRecordCount   = totalRecordCount;

            _firstRecordNum     = ( ( pageNumber - 1 ) * pageSize ) + 1;

            _lastRecordNum      = _firstRecordNum + _recordCount - 1;


            if ( _firstRecordNum > _totalRecordCount ) {
                throw new ApplicationException( "'_firstRecordNum' cannot be greater than 'totalRecordCount'." );
            }

            if ( _lastRecordNum > _totalRecordCount ) {
                throw new ApplicationException( "'_lastRecordNum' cannot be greater than 'totalRecordCount'." );
            }

        }


        public PagingParams PagingParams {
            get { return _pagingParams; }
        }

        public int RecordCount {
            get { return _recordCount; }
        }

        public int TotalRecordCount {
            get { return _totalRecordCount; }
        }

        public int FirstRecordNum {
            get { return _firstRecordNum; }
        }

        public int LastRecordNum {
            get { return _lastRecordNum; }
        }



        public override String ToString() {

            return String.Format( 
                            "PagingParams = [{0}], RecordCount = {1}, TotalRecordCount = {2}, FirstRecordNum = {3}, LastRecordNum = {4}", 
                            PagingParams, 
                            RecordCount, 
                            TotalRecordCount, 
                            FirstRecordNum, 
                            LastRecordNum 
            );

        }



        public override bool Equals( Object otherObj ) {

            var other = otherObj as PagingResult;

            if ( Object.ReferenceEquals( other, null ) ) {  return false; }

            return Equals( other  );

        }


        public bool Equals( PagingResult other ) {

            if ( Object.ReferenceEquals( other, null ) ) {  return false; }
            if ( Object.ReferenceEquals( other, this ) ) {  return true;  }


            if ( !PagingParams.Equals( other.PagingParams )     ) { return false; }

            if ( RecordCount        != other.RecordCount        ) { return false; }
            if ( TotalRecordCount   != other.TotalRecordCount   ) { return false; }
            if ( FirstRecordNum     != other.FirstRecordNum     ) { return false; }
            if ( LastRecordNum      != other.LastRecordNum      ) { return false; }

            return true;

        }



        public override int GetHashCode() {

            return ( 
                PagingParams.GetHashCode()      ^ 
                RecordCount.GetHashCode()       ^ 
                TotalRecordCount.GetHashCode()  ^ 
                FirstRecordNum.GetHashCode()    ^ 
                LastRecordNum.GetHashCode() 
            );

        }

    }

}

