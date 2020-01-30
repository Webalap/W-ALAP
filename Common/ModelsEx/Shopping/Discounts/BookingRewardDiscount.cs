namespace Common.ModelsEx.Shopping.Discounts
{
    public class BookingRewardDiscount : FixedDiscount
    {
        public BookingRewardDiscount() : base() { }

        internal BookingRewardDiscount(decimal discountAmount)
            : base(DiscountType.BookingRewards, discountAmount)
        { }

        public override string Description { get { return "Booking Rewards Cash"; } }
        public override string DisplayText { get { return "Bookings"; } }
        public override bool OverrideTaxableAmount { get { return false; } }
    }
}