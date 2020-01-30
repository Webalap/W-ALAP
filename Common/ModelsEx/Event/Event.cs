using Common.ModelsEx.Shopping.Discounts;
using ExigoService;
using System;
using System.Linq;

namespace Common.ModelsEx.Event
{
    public class Event
    {
        public int Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the event date.
        /// </summary>
        public DateTime ActualDate { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets the end date.
        /// </summary>
        public DateTime CloseDate { get; set; }

        /// <summary>
        /// Gets the lockout date.
        /// </summary>
        public DateTime LockoutDate { get; set; }

        public string WebAlias { get; set; }
        
        public string CreatorId { get; set; }
        public string CreatorFullName { get; set; }
        public string HostId { get; set; }
        public string HostFullName { get; set; }
        public string HostEmail { get; set; }
        public string HostPhone { get; set; }
        public string MainAddress1 { get; set; }
        public string MainAddress2 { get; set; }
        public string MainCity { get; set; }
        public string MainState { get; set; }
        public string MainZip { get; set; }
        public string MainCountry { get; set; }
        public Address Location { get; set; }
        public bool IsNewGT { get; set; }


        /// <summary>
        /// Gets the location of selected event.
        /// </summary>
        public Address EventLocation 
        {
            get {
                Address loc = new Address();
                loc.Address1 = this.MainAddress1;
                loc.Address2 = this.MainAddress2;
                loc.City = this.MainCity;
                loc.State = this.MainState;
                loc.Zip = this.MainZip;
                loc.Country = this.MainCountry;
                return loc; }
        }

        public Customer Creator { get; set; }

        public Customer Host { get; set; }


        public string EventStatus { get; set; }
        public bool DeletedEvents { get; set; }

        public decimal HalfOffCredits 
        { 
            get
            {
                if (null != EventPointAccounts)
                {
                    return (from pa in this.EventPointAccounts
                            where PointAccounts.Host12offcredits.Equals(pa.PointAccountID)
                            select pa.Balance).FirstOrDefault();
                }

                return 0M;
            }
        }
        public decimal RewardsCashAmount { get; set; }
        public decimal HalfOffCreditsAmount { get; set; }
        public decimal RewardsCash
        {
            get
            {
                if (null != EventPointAccounts)
                {
                    return (from pa in this.EventPointAccounts
                            where PointAccounts.HostRewardsCash.Equals(pa.PointAccountID)
                            select pa.Balance).FirstOrDefault();
                }

                return 0M;
            }
        }

        //DO WE NEED THIS?
        //public decimal RecruitingRewards
        //{
        //    get
        //    {
        //        if (null != EventPointAccounts)
        //        {
        //            return (from pa in this.EventPointAccounts
        //                    where PointAccounts.RecruitingRewards.Equals(pa.PointAccountID)
        //                    select pa.Balance).FirstOrDefault();
        //        }

        //        return 0M;
        //    }
        //}

        ////DO WE NEED THIS?
        //public decimal EnrolleeRewards
        //{
        //    get
        //    {
        //        if (null != EventPointAccounts)
        //        {
        //            return (from pa in this.EventPointAccounts
        //                    where PointAccounts.EnrolleeRewards.Equals(pa.PointAccountID)
        //                    select pa.Balance).FirstOrDefault();
        //        }

        //        return 0M;
        //    }
        //}

        public decimal PartySalesTotal
        {
            get
            {
                if (null != EventPointAccounts)
                {
                    RewardsAccount partySales = EventPointAccounts.Where(p => p.PointAccountID == PointAccounts.PartySalesTotal).FirstOrDefault();

                    if (partySales != null)
                    {
                        return partySales.Balance;
                    }
                }

                return 0M;
            }
        }

        public decimal TotalSales { get; set; }

        public RewardsAccount[] EventPointAccounts { get; set; }

        //public BookingReward[] EventBookingRewards { get; set; }

        //public int BookingRewardsOwnerId { get; set; }

        // event for charity 
        public int Charitable { get; set; }

        public string CharityName { get; set; }
        public decimal CharityContribution { get; set; }

        public string  EventItemsCount { get; set; }
        public string EventCartUrl { get; set; }
        public int IhContribution{ get; set; }

        public bool IsPartyLockedOut
        {
            get
            {
                return (DateTime.Now > this.LockoutDate);
            }
        }

        public HostSpecialDiscount HostSpecialReward { get; set; }
        public DateTime PartyDate { get; set; }
        public DateTime PartyStartTime { get; set; }
        public DateTime PartyEndTime { get; set; }
        public string TimeZone { get; set; }
    }
}