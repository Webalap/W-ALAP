using System;


namespace Common.Logging.Simple
{

    public sealed class SimpleLogItem : IEquatable<SimpleLogItem> {

        private readonly DateTime               _timeStamp      = DateTime.Now;   //REVIEW: Should this be included in Equality and HashCode determination?
        private readonly SimpleLogItemSeverity  _severity;
        private readonly String                 _message;
        private readonly String                 _description;
        private readonly Exception              _exception;


        public SimpleLogItem( SimpleLogItemSeverity severity, String message, String description = null, Exception exception = null ) {

            if ( message == null ) {
                throw new ArgumentNullException( "message" );
            }


            _severity       = severity;
            _message        = message;
            _description    = description;
            _exception      = exception;

        }


        public DateTime TimeStamp {
            get { return _timeStamp; }
        }

        public SimpleLogItemSeverity Severity {
            get { return _severity; }
        }

        public String Message {
            get { return _message; }
        }

        public String Description {
            get { return _description; }
        }

        public Exception Exception {
            get { return _exception; }
        }



        public bool Equals( SimpleLogItem other ) {

            if ( Object.ReferenceEquals( other, null ) ) { return false; }
            if ( Object.ReferenceEquals( other, this ) ) { return true;  }

            if ( Severity != other.Severity ) { return false; }

            if ( !String.Equals( Message,           other.Message,      StringComparison.Ordinal ) ) { return false; }
            if ( !String.Equals( Description,       other.Description,  StringComparison.Ordinal ) ) { return false; }

            if ( !Object.Equals( Exception, other.Exception ) ) { return false; }   //REVIEW: Should we rely on implementation of Equals method on Exception class?

            return true;

        }


        public override bool Equals( Object otherObj ) {

            SimpleLogItem other = otherObj as SimpleLogItem;

            if ( Object.ReferenceEquals( otherObj, null ) ) { return false; }

            return Equals( other );

        }



        public override String ToString() {

            return String.Format( "{0}: {1}", Severity, Message );

        }


        public override int GetHashCode() {

            int hashCode = Severity.GetHashCode() ^ Message.GetHashCode();

            if ( Description != null ) {

                hashCode ^= Description.GetHashCode();

            }

            if ( Exception != null ) {

                hashCode ^= Exception.GetHashCode();

            }

            return hashCode;

        }

    }

}
