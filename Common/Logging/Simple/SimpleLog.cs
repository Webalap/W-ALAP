using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace Common.Logging.Simple
{

    public sealed class SimpleLog : IEquatable<SimpleLog>, IEnumerable<SimpleLogItem> {

        private readonly List<SimpleLogItem>   _items;


        public SimpleLog() {

            _items = new List<SimpleLogItem>();

        }


        public SimpleLog( int capacity ) {

            _items = new List<SimpleLogItem>( capacity );

        }


        public SimpleLog( IEnumerable<SimpleLogItem> items ) {

            if ( Object.ReferenceEquals( items, null ) ) {
                throw new ArgumentNullException( "items" );
            }

            _items = new List<SimpleLogItem>( items );

        }


        //NOTE: This constructor must ensure that the new instance COPIES all item references from the (ValidationItemCollection) argument.
        public SimpleLog( SimpleLog itemCollection ) : this( (IEnumerable<SimpleLogItem>)itemCollection ) {}



        public int Count {
            get { return _items.Count; }
        }



        public void Add( SimpleLogItem item ) {

            if ( item == null ) {
                throw new ArgumentNullException( "item" );
            }

            _items.Add( item );

        }


        public void AddRange( IEnumerable<SimpleLogItem> items ) {

            if ( items == null ) {
                throw new ArgumentNullException( "items" );
            }

            foreach ( SimpleLogItem item in items ) {

                Add( item );

            }

        }


        public void Clear() {

            _items.Clear();

        }


        public IEnumerable<SimpleLogItem> Infos() {

            return _items.Where( item => ( item.Severity == SimpleLogItemSeverity.Info ) );

        }


        public IEnumerable<SimpleLogItem> Warnings() {

            return _items.Where( item => ( item.Severity == SimpleLogItemSeverity.Warn ) );

        }


        public IEnumerable<SimpleLogItem> Errors() {

            return _items.Where( item => ( item.Severity == SimpleLogItemSeverity.Error ) );

        }



        public bool HasInfos() {

            return Infos().Any();

        }

        public bool HasWarnings() {

            return Warnings().Any();

        }

        public bool HasErrors() {

            return Errors().Any();

        }



        public IEnumerator<SimpleLogItem> GetEnumerator() {

            return _items.GetEnumerator();

        }


        IEnumerator IEnumerable.GetEnumerator() {

            return ( (IEnumerable<SimpleLogItem>)this ).GetEnumerator();

        }



        public bool Equals( SimpleLog other ) {

            if ( Object.ReferenceEquals( other, null ) ) { return false; }
            if ( Object.ReferenceEquals( other, this ) ) { return true;  }


            return _items.Equals( other._items );

        }


        public override bool Equals( Object otherObj ) {

            SimpleLog other = otherObj as SimpleLog;

            if ( Object.ReferenceEquals( otherObj, null ) ) { return false; }

            return Equals( other );

        }



        public override String ToString() {

            return String.Format( "Count = {0}", _items.Count );

        }


        public override int GetHashCode() {

            return _items.GetHashCode();

        }

    }

}
