using ExigoService;

namespace Common.ModelsEx.Shopping
{
    public class Enrollment : ShoppingExperience
    {
        public Enrollment()
        {
        }

        public CustomerExtendedDetails CustomerEx { get; set; }
        public CustomerExtendedDetails CustomerExAgreements { get; set; }
        public CustomerSite CustomerSite { get; set; }
        public bool CanLogin { get; set; }    
    }
}