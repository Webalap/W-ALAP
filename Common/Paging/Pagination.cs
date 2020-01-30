using System;
using System.Collections;
using System.Collections.Generic;


namespace Common.Paging
{

    public sealed class Pagination : IEquatable<Pagination>, IEnumerable<PaginationPage> {

        private readonly PagingResult   _pagingResult;
        private readonly int            _pageCount;
        private readonly int            _lastPageRecordCount;
        private readonly bool           _isLastPagePartial; 


        public Pagination( PagingResult pagingResult ) {

            if ( pagingResult == null ) {
                throw new ArgumentNullException( "pagingResult" );
            }

            _pagingResult = pagingResult;


            int totalRecords = pagingResult.TotalRecordCount;

            int pageSize = pagingResult.PagingParams.PageSize;


            if ( totalRecords < 1 ) {

                _pageCount = 0;
                _isLastPagePartial = false;
                _lastPageRecordCount = 0;

            } else {

                _pageCount = totalRecords / pageSize;

                int lastPageRemainder = totalRecords % pageSize;

                _isLastPagePartial = lastPageRemainder > 0;

                if ( _isLastPagePartial ) {   // less than a full page of records on the last page...

                    _lastPageRecordCount = lastPageRemainder;

                    // ...so we add one more page at the end for the partial page
                    _pageCount++;

                } else {

                    _lastPageRecordCount = pageSize;

                }

            }

        }


        public PagingResult PagingResult {
            get { return _pagingResult; }
        }

        public int PageCount {
            get { return _pageCount; }
        }

        public int PageSize {
            get { return PagingResult.PagingParams.PageSize; }
        }

        public int TotalRecordCount {
            get { return PagingResult.TotalRecordCount; }
        }

        public int LastPageRecordCount {
            get { return _lastPageRecordCount; }
        }

        public bool IsLastPagePartial {
            get { return _isLastPagePartial; }
        }



        public PaginationPage GetPage( int pageNumber ) {

            if ( ( pageNumber < 1 ) || ( pageNumber > PageCount ) ) {
                throw new ArgumentOutOfRangeException( "pageNumber", pageNumber, "Argument must be greater than or equal to 1 and less than or equal to 'PageCount'." );
            }

            bool isFirstPage    = ( pageNumber == 1         );
            bool isLastPage     = ( pageNumber == PageCount );

            int recordsOnPage = isLastPage ? LastPageRecordCount : PageSize;

            return new PaginationPage( 
                                pageNumber, 
                                PageSize, 
                                recordsOnPage, 
                                isFirstPage, 
                                isLastPage 
            );

        }



        public IEnumerator<PaginationPage> GetEnumerator() {

            for ( int pageNum = 1; pageNum <= PageCount; pageNum++ ) {

                yield return GetPage( pageNum );

            }

        }


        IEnumerator IEnumerable.GetEnumerator() {

            return ( (IEnumerable<PaginationPage>)this ).GetEnumerator();

        }



        public override String ToString() {

            return String.Format( "PagingResult = [{0}], PageCount = {1}, TotalRecordCount = {2}", PagingResult, PageCount, TotalRecordCount );

        }



        public override bool Equals( Object otherObj ) {

            var other = otherObj as Pagination;

            if ( Object.ReferenceEquals( other, null ) ) { return false; }

            return Equals( other );

        }


        public bool Equals( Pagination other ) {

            if ( Object.ReferenceEquals( other, null ) ) { return false; }
            if ( Object.ReferenceEquals( other, this ) ) { return true;  }


            if ( !PagingResult.Equals( other.PagingResult )             ) { return false; }

            if ( PageCount              != other.PageCount              ) { return false; }
            if ( PageSize               != other.PageSize               ) { return false; }
            if ( TotalRecordCount       != other.TotalRecordCount       ) { return false; }
            if ( LastPageRecordCount    != other.LastPageRecordCount    ) { return false; }
            if ( IsLastPagePartial      != other.IsLastPagePartial      ) { return false; }

            return true;

        }



        public override int GetHashCode() {

            return ( 
                PagingResult.GetHashCode()          ^ 
                PageCount.GetHashCode()             ^ 
                PageSize.GetHashCode()              ^ 
                TotalRecordCount.GetHashCode()      ^ 
                LastPageRecordCount.GetHashCode()   ^ 
                IsLastPagePartial.GetHashCode() 
            );

        }

    }

}
