using System;
using System.Diagnostics.Contracts;


namespace ExigoService
{

    public sealed class Rank : IRank {

        public Rank() {

            RankID = 0;
            RankDescription = null;

        }

        public Rank( int id, String description ) {

            RankID = id;
            RankDescription = description;

        }

        private Rank( Rank other ) {

            Contract.Requires( other != null );

            RankID = other.RankID;
            RankDescription = other.RankDescription;

        }


        public Rank DeepClone() {

            return new Rank( this );

        }



        public int RankID { get; set; }
        public String RankDescription { get; set; }



        public override String ToString() {

            return String.Format( "{0}:{1}", RankID, RankDescription );

        }



        public static Rank Default {

            get {

                return new Rank {
                    RankDescription = "Unknown"
                };

            }

        }

    }

}
