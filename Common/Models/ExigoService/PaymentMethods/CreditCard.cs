using Common.Api.ExigoWebService;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace ExigoService
{
    public enum TypeOfCreditCard
    {
        Unknown = 1,
        Visa = 2,
        MasterCard = 3,
        AmEx = 4,
        Discover = 5
    }

    public class CreditCard : ICreditCard
    {
        public CreditCard()
        {
            Type = CreditCardType.New;
            BillingAddress = new Address();
            ExpirationMonth = DateTime.Now.Month;
            ExpirationYear = DateTime.Now.Year;
            AutoOrderIDs = new int[0];
            Id = -1;
            _typeOfCard = TypeOfCreditCard.Unknown;
        }
        private CreditCard(CreditCard other)
        {

            Contract.Requires(other != null);

            NameOnCard = other.NameOnCard;
            CardNumber = other.CardNumber;
            CVV = other.CVV;
            ExpirationMonth = other.ExpirationMonth;
            ExpirationYear = other.ExpirationYear;

        }
        public CreditCard(CreditCardType type)
        {
            Type = type;
            BillingAddress = new Address();
            ExpirationMonth = DateTime.Now.Month;
            ExpirationYear = DateTime.Now.Year;

            _typeOfCard = TypeOfCreditCard.Unknown;
        }
        public bool IsNewCard { get; set; }
        public CreditCardType Type { get; set; }
        public bool IsShippingCard { get; set; }
        public int Id { get; set; }
        [Display(Name = "Name on Card")]
        [Required]
        public string NameOnCard { get; set; }

        [Display(Name = "Card Number")]
        [Required]
        [RegularExpression("^[0-9 ]*$", ErrorMessage = "Card number must be numeric")]
        [StringLength(19, MinimumLength = 12, ErrorMessage = "Card number must be between 12 to 19 characters")]

        public string CardNumber { get; set; }

        public TypeOfCreditCard TypeOfCard 
        { 
            get
            {
                if (TypeOfCreditCard.Unknown.Equals(_typeOfCard))
                    _typeOfCard = GetTypeOfCreditCard();

                return _typeOfCard;
            }
            set
            {
                _typeOfCard = value;
            }
        }
        private TypeOfCreditCard _typeOfCard;

        [Display(Name = "Expiration Month")]
        [Required]
        public int ExpirationMonth { get; set; }

        [Display(Name = "Expiration Year")]
        [Required]
        public int ExpirationYear { get; set; }

        public string GetToken()
        {
            if (!IsComplete) return string.Empty;

            return Exigo.Payments().FetchCreditCardToken(
                CardNumber,
                ExpirationMonth,
                ExpirationYear);
        }

        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "CVV number must be numeric")]
        [StringLength(4,  MinimumLength = 3, ErrorMessage = "CVV must be 3 or 4 characters")]
        public string CVV { get; set; }
        
        [Range(1, Double.MaxValue,ErrorMessage= "It needs to be at least $1.00")]
        public decimal Amount { get; set; }
        [Required, DataType("Address")]
        public Address BillingAddress { get; set; }

        public int[] AutoOrderIDs { get; set; }

        public DateTime ExpirationDate
        {
            get { return new DateTime(ExpirationYear, ExpirationMonth, 1); }
        }
        public string ExpirationDateString
        {
            get
            {
                return String.Format("{0}/{1}",
                    ExpirationMonth.ToString().PadLeft(2, '0'),
                    ExpirationYear.ToString());
            }
        }
        public bool IsExpired
        {
            get { return ExpirationDate < new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1); }
        }
        public bool IsComplete
        {
            get
            {
                return !string.IsNullOrEmpty(NameOnCard) && 
                    !string.IsNullOrEmpty(CardNumber) &&
                    ExpirationMonth != 0 && 
                    ExpirationYear != 0 &&
                    BillingAddress.IsComplete;
            }
        }
        public bool IsValid
        {
            get { return IsComplete && !IsExpired; }
        }
        public bool IsUsedInAutoOrders
        {
            get { return AutoOrderIDs.Length > 0; }
        }
        public bool IsTestCreditCard
        {
            get { return CardNumber != "9696"; }
        }

        public AutoOrderPaymentType AutoOrderPaymentType
        {
            get
            {
                switch(Type)
                {
                    case CreditCardType.Primary:
                    default: return AutoOrderPaymentType.PrimaryCreditCard;

                    case CreditCardType.Secondary: return AutoOrderPaymentType.SecondaryCreditCard;
                }
            }
        }

        protected TypeOfCreditCard GetTypeOfCreditCard()
        {
            var result = TypeOfCreditCard.Unknown;
            
            if (CardNumber == null)
            {
                result = TypeOfCreditCard.Unknown;
            }
            else  if (Regex.Match(CardNumber.Replace(" ",""), "^4[0-9]{6,}$").Length != 0)
            {
                result = TypeOfCreditCard.Visa;
            }
            else if (Regex.Match(CardNumber.Replace(" ", ""), "^5[1-5][0-9]{5,}$").Length != 0)
            {
                result = TypeOfCreditCard.MasterCard;
            }
            else if (Regex.Match(CardNumber.Replace(" ", ""), "^3[47][0-9]{5,}$").Length != 0)
            {
                result = TypeOfCreditCard.AmEx;
            }
            else if (Regex.Match(CardNumber.Replace(" ", ""), "^6(?:011|5[0-9]{2})[0-9]{3,}$").Length != 0)
            {
                result = TypeOfCreditCard.Discover;
            }

            return result;
        }
    }
}