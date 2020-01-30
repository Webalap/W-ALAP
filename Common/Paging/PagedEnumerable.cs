using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Common.Paging
{

    public abstract class PagedEnumerable : IPagedEnumerable {

        protected PagedEnumerable() {}


        public abstract PagingResult PagingResult { get; }

        public int PageSize {
            get { return PagingResult.PagingParams.PageSize; }
        }

        public int PageNumber {
            get { return PagingResult.PagingParams.PageNumber; }
        }

        public int RecordCount {
            get { return PagingResult.RecordCount; }
        }

        public int TotalRecordCount {
            get { return PagingResult.TotalRecordCount; }
        }



        public override String ToString() {

            return String.Format( "PagingResult = [{0}], RecordCount = {1}, TotalRecordCount = {2}", PagingResult, RecordCount, TotalRecordCount );

        }



        public override bool Equals( Object otherObj ) {

            var other = otherObj as IPagedEnumerable;

            if ( Object.ReferenceEquals( other, null ) ) {  return false; }

            return Equals( other  );

        }


        public bool Equals( IPagedEnumerable other ) {

            if ( Object.ReferenceEquals( other, null ) ) {  return false; }
            if ( Object.ReferenceEquals( other, this ) ) {  return true;  }


            if ( !PagingResult.Equals( other.PagingResult )     ) { return false; }

            if ( PageSize           != other.PageSize           ) { return false; }
            if ( PageNumber         != other.PageNumber         ) { return false; }
            if ( RecordCount        != other.RecordCount        ) { return false; }
            if ( TotalRecordCount   != other.TotalRecordCount   ) { return false; }

            return true;

        }



        public override int GetHashCode() {

            return ( 
                PagingResult.GetHashCode()  ^ 
                RecordCount.GetHashCode()   ^ 
                TotalRecordCount.GetHashCode() 
            );

        }

    }




    public sealed class PagedEnumerable<TRecord> : PagedEnumerable, IPagedEnumerable<TRecord> {

        private readonly TRecord[]      _records;
        private readonly PagingResult   _pagingResult;


        public PagedEnumerable( PagingParams pagingParams, int totalRecordCount, IEnumerable<TRecord> records ) {

            if ( pagingParams == null ) {
                throw new ArgumentNullException( "pagingParams" );
            }

            if ( records == null ) {
                throw new ArgumentNullException( "records" );
            }


            _records        = records.ToArray();

            _pagingResult   = PagingResult.Create( pagingParams, _records.Length, totalRecordCount );

        }


        public override PagingResult PagingResult {
            get { return _pagingResult; }
        }



        public IEnumerator<TRecord> GetEnumerator() {

            return ( (IEnumerable<TRecord>)_records ).GetEnumerator();

        }


        IEnumerator IEnumerable.GetEnumerator() {

            return _records.GetEnumerator();

        }

    }

}
