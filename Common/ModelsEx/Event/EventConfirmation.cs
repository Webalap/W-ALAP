namespace Common.ModelsEx.Event
{
    public class EventConfirmation : Base.ServiceResponse
    {
        public EventConfirmation() 
            : base()
        {
        }

        public int CustomerID { get; set; }
    }
}