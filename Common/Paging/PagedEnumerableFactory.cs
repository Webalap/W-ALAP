using System;
using System.Collections.Generic;
using System.Linq;


namespace Common.Paging
{

    public sealed class PagedEnumerableFactory<TRecord> where TRecord : class {

        private readonly IEnumerable<TRecord>   _allRecords;


        public PagedEnumerableFactory( IEnumerable<TRecord> allRecords ) {

            if ( allRecords == null ) {
                throw new ArgumentNullException( "allRecords" );
            }


            _allRecords     = allRecords;

        }


        public IPagedEnumerable<TRecord> Create( PagingParams pagingParams = null ) {

            int totalRecordCount = _allRecords.Count();

            if ( pagingParams == null ) {   // no paging, show all records on page 1

                pagingParams = new PagingParams( pageSize: Math.Max( totalRecordCount, 1 ), pageNumber: 1 );

                return new PagedEnumerable<TRecord>( pagingParams, totalRecordCount, _allRecords );

            } else {   // do paging

                IEnumerable<TRecord> pageOfRecords = _allRecords.Skip( pagingParams.SkipCount )
                                                                .Take( pagingParams.TakeCount );

                if ( !pageOfRecords.Any() ) {   // no records on specified page, so re-do paging for page 1

                    pagingParams = pagingParams.CreateForPageOne();

                    pageOfRecords = _allRecords.Skip( pagingParams.SkipCount )
                                               .Take( pagingParams.TakeCount );

                }

                return new PagedEnumerable<TRecord>( 
                                                pagingParams, 
                                                totalRecordCount, 
                                                pageOfRecords 
                );

            }

        }

    }

}
