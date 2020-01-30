using System;


namespace Common.Paging
{

    public sealed class PaginationPage {

        private readonly int    _pageNumber;
        private readonly int    _pageSize;
        private readonly int    _recordCount;
        private readonly bool   _isFirstPage;
        private readonly bool   _isLastPage;


        internal PaginationPage( int pageNumber, int pageSize, int recordCount, bool isFirstPage, bool isLastPage ) {

            if ( pageNumber < 1 ) {
                throw new ArgumentOutOfRangeException( "pageNumber", pageNumber, "Argument must be greater than zero." );
            }

            if ( pageSize < 1 ) {
                throw new ArgumentOutOfRangeException( "pageSize", pageSize, "Argument must be greater than zero." );
            }

            if ( recordCount < 0 ) {
                throw new ArgumentOutOfRangeException( "recordCount", recordCount, "Argument must be greater than or equal to zero." );
            }

            if ( recordCount > pageSize ) {
                throw new ArgumentOutOfRangeException( "recordCount", recordCount, "Argument must be less than or equal to 'pageSize'." );
            }


            _pageNumber     = pageNumber;
            _pageSize       = pageSize;
            _recordCount    = recordCount;
            _isFirstPage    = isFirstPage;
            _isLastPage     = isLastPage;

        }


        public int PageNumber {
            get { return _pageNumber; }
        }

        public int PageSize {
            get { return _pageSize; }
        }

        public int RecordCount {
            get { return _recordCount; }
        }

        public bool IsFirstPage {
            get { return _isFirstPage; }
        }

        public bool IsLastPage {
            get { return _isLastPage; }
        }

        public bool IsOnlyPage {
            get { return IsFirstPage && IsLastPage; }
        }



        public override String ToString() {

            return String.Format( 
                            "PageNumber = {0}, PageSize = {1}, RecordCount = {2}", 
                            PageNumber, 
                            PageSize, 
                            RecordCount 
            );

        }



        public override bool Equals( Object otherObj ) {

            var other = otherObj as PaginationPage;

            if ( Object.ReferenceEquals( other, null ) ) {  return false; }

            return Equals( other  );

        }


        public bool Equals( PaginationPage other ) {

            if ( Object.ReferenceEquals( other, null ) ) { return false; }
            if ( Object.ReferenceEquals( other, this ) ) { return true;  }

            if ( PageNumber     != other.PageNumber     ) { return false; }
            if ( PageSize       != other.PageSize       ) { return false; }
            if ( RecordCount    != other.RecordCount    ) { return false; }
            if ( IsFirstPage    != other.IsFirstPage    ) { return false; }
            if ( IsLastPage     != other.IsLastPage     ) { return false; }

            return true;

        }



        public override int GetHashCode() {

            return ( 
                PageNumber.GetHashCode()    ^ 
                PageSize.GetHashCode()      ^ 
                RecordCount.GetHashCode()   ^ 
                IsFirstPage.GetHashCode()   ^ 
                IsLastPage.GetHashCode() 
            );

        }

    }

}
