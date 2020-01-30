using System;


namespace Common.Paging
{

    public sealed class PagingParams : IEquatable<PagingParams> {

        public static PagingParams CreateConstrained( int page, int pageSize ) {

            page = Math.Max( page, 1 );   // page must have a minimum value of 1

            pageSize = Math.Max( pageSize, PagingConstants.MIN_PAGE_SIZE );

            pageSize = Math.Min( pageSize, PagingConstants.MAX_PAGE_SIZE );

            return new PagingParams( pageSize, page );

        }




        private readonly int _pageSize;
        private readonly int _pageNumber;


        public PagingParams( int pageSize = PagingConstants.DEFAULT_PAGE_SIZE, int pageNumber = PagingConstants.DEFAULT_PAGE_NUM ) {

            if ( pageSize < 1 ) {
                throw new ArgumentOutOfRangeException( "pageSize", pageSize, "Argument must be greater than zero." );
            }

            if ( pageNumber < 1 ) {
                throw new ArgumentOutOfRangeException( "pageNumber", pageNumber, "Argument must be greater than zero." );
            }

            _pageSize      = pageSize;
            _pageNumber    = pageNumber;

        }


        public PagingParams CreateForPageOne() {

            return new PagingParams( _pageSize, pageNumber: 1 );

        }



        public int PageSize {
            get { return _pageSize; }
        }

        public int PageNumber {
            get { return _pageNumber; }
        }

        public int SkipCount {
            get { return ( PageNumber - 1 ) * PageSize; }
        }

        public int TakeCount {
            get { return PageSize; }
        }


        public override String ToString() {

            return String.Format( "PageSize = {0}, PageNumber = {1}", PageSize, PageNumber );

        }



        public override bool Equals( Object otherObj ) {

            var other = otherObj as PagingParams;

            if ( Object.ReferenceEquals( other, null ) ) { return false; }

            return Equals( other  );

        }


        public bool Equals( PagingParams other ) {

            if ( Object.ReferenceEquals( other, null ) ) { return false; }
            if ( Object.ReferenceEquals( other, this ) ) { return true;  }

            if ( PageSize   != other.PageSize   ) { return false; }
            if ( PageNumber != other.PageNumber ) { return false; }

            return true;

        }



        public override int GetHashCode() {

            return ( PageSize.GetHashCode() ^ PageNumber.GetHashCode() );

        }

    }

}
