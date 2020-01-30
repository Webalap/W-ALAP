namespace Common.ModelsEx.Email
{
    public class PendingOrderNotificationEmail
    {

        public string Subject { get; set; }
        public string EmailHeading { get; set; } //emailheading
        public string CardDetails { get; set; } // cardDetails

        public string PendingOrderUrl // PendingOrder
        {
            get;
            set;
        }
    }
}