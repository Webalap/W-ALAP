using System;

namespace ExigoService
{
    public class Payment : IPayment
    {
        public int PaymentID { get; set; }
        public int CustomerID { get; set; }
        public int OrderID { get; set; }
        public int PaymentTypeID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int WarehouseID { get; set; }
        public string BillingName { get; set; }
        public Address BillingAddress { get; set; }
        public int? CreditCardTypeID { get; set; }
        public string CreditCardNumber { get; set; }
        public string AuthorizationCode { get; set; }
        public int ExpirationMonth { get; set; }
        public int ExpirationYear { get; set; }
        public string Memo { get; set; }
        public int DeclineOrderAmountID { get; set; }
        #region PaymentMethodForLoginUser
        public string BankNameOnAccount { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankRoutingNumber { get; set; }
        public string BankAccountAddress { get; set; }
        public string BankAccountCity { get; set; }
        public string BankAccountState { get; set; }
        public string BankAccountZip { get; set; }
        public string BankAccountCountry { get; set; }
        public string PrimaryBillingName { get; set; }
        public string PrimaryCreditCardDisplay { get; set; }
        public int PrimaryExpirationMonth { get; set; }
        public int PrimaryExpirationYear { get; set; }
        public string PrimaryBillingAddress { get; set; }
        public string PrimaryBillingCity { get; set; }
        public string PrimaryBillingState { get; set; }
        public string PrimaryBillingZip { get; set; }
        public string PrimaryBillingCountry { get; set; }
        public string SecondaryBillingName { get; set; }
        public string SecondaryCreditCardDisplay { get; set; }
        public int SecondaryExpirationMonth { get; set; }
        public int SecondaryExpirationYear { get; set; }
        public string SecondaryBillingAddress { get; set; }
        public string SecondaryBillingCity { get; set; }
        public string SecondaryBillingState { get; set; }
        public string SecondaryBillingZip { get; set; }
        public string SecondaryBillingCountry { get; set; }

        #endregion

        public string ExpirationDate
        {
            get
            {
                return String.Format("{0}/{1}", 
                    ExpirationMonth.ToString().PadLeft(2, '0'), 
                    ExpirationYear.ToString());
            }
        }

        public string TypeOfCard
        {
            get
            {
                if (this.CreditCardTypeID.HasValue)
                {
                    switch (this.CreditCardTypeID.Value)
                    {
                        case (int)TypeOfCreditCard.Visa:
                            return "Visa";
                        case (int)TypeOfCreditCard.MasterCard:
                            return "Master Card";
                        case (int)TypeOfCreditCard.AmEx:
                            return "American Express";
                        case (int)TypeOfCreditCard.Discover:
                            return "Discover";
                        default:
                            return string.Empty;
                    }
                }

                return string.Empty;
            }
        }

        public PaymentType PaymentType { get; set; }
    }
}
