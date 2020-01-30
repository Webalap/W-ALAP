using ExigoService;

namespace Common.Api.ExigoWebService
{
    public partial class SetAccountCreditCardTokenRequest
    {
        public SetAccountCreditCardTokenRequest() { }

        public SetAccountCreditCardTokenRequest(CreditCard card, string token, int customerId, bool useMainAddress = false) 
        {
            this.CustomerID = customerId;
            this.BillingName = card.NameOnCard;

            this.BillingAddress = card.BillingAddress.Address1;
            this.BillingAddress2 = card.BillingAddress.Address2;
            this.BillingCity = card.BillingAddress.City;
            this.BillingState = card.BillingAddress.State;
            this.BillingZip = card.BillingAddress.Zip;
            this.BillingCountry = card.BillingAddress.Country;
            
            this.CreditCardToken = token;
            this.ExpirationMonth = card.ExpirationMonth;
            this.ExpirationYear = card.ExpirationYear;

            this.UseMainAddress = false;
        }
        
    }
}