using Common.ModelsEx.Shopping;
using Common.ServicesEx;
using ExigoService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Common.ModelsEx.Replicated
{
    [Serializable]
    public class ShoppingCartCheckoutPropertyBag : BasePropertyBag
    {
        private string version = "1.0.0";
        private int expires = 0;

        #region Constructors
        public ShoppingCartCheckoutPropertyBag()
        {
            Expires = expires;
            ShoppingCart = ShoppingCart ?? DependencyResolver.Current.GetService<ShoppingCart>();
            ShoppingCart.ShippingMethodId = ShoppingCart.Configuration.DefaultShipMethodID;
            PersonalInformation = PersonalInformation ?? new PersonalViewModel();
            GuestInformation = GuestInformation ?? new GuestViewModel();
            PaymentInformation = PaymentInformation ?? new PaymentViewModel();
            OrderConfirmation = OrderConfirmation ?? new OrderConfirmation();
        }
        #endregion

        #region Properties

        public Common.ModelsEx.Shopping.IOrder ShoppingCart { get; set; }
        
        public bool UpgradeShipping { get; set; }
        public string GifftMessage { get; set; }
        public string ShipMethodDescription { get; set; }
        #endregion

        #region Personal Information

        public PersonalViewModel PersonalInformation { get; set; }

        #endregion

        #region Guest Information

        public GuestViewModel GuestInformation { get; set; }

        #endregion

        #region Payment Information

        public PaymentViewModel PaymentInformation { get; set; }

        #endregion

        #region Receipt (Thank You)

        public OrderConfirmation OrderConfirmation { get; set; }

        #endregion

        #region Methods
        public override T OnBeforeUpdate<T>(T propertyBag)
        {
            propertyBag.Version = version;

            return propertyBag;
        }
        public override bool IsValid()
        {
            return Version == version;
        }
        #endregion
    }

    public class PaymentViewModel //: ViewModelBase
    {
        public PaymentViewModel()
        {
            MailingAddress = new Address();
            MainAddress = new Address();
            CreditCard = CreditCard ?? new CreditCard();
            //SameAsAddress = "main-address";
        }
        public PaymentViewModel(Address mailingAddress, Address mainAddress, Address billingAddress, CreditCard creditCard)
        {
            MailingAddress = mailingAddress ?? new Address();
            MainAddress = mailingAddress ?? new Address();
            CreditCard = creditCard ?? new CreditCard();
            CreditCard.BillingAddress = billingAddress ?? new Address();
            //SameAsAddress = "main-address";
        }
        public int SponsorId { get; set; }

        public CreditCard CreditCard { get; set; }

        public BankAccount BankAccount { get; set; }

        public string SameAsAddress { get; set; }

        public Address BillingAddress { get; set; }

        [JsonIgnore]
        public Address MailingAddress { get; set; }

        [JsonIgnore]
        public Address MainAddress { get; set; }

        [Required]
        public int PaymentTypeID { get; set; }

        public IPaymentMethod PaymentMethod { get; set; }

        // TODO: gwb:20141226 - What does this mean, exactly?  How do we validate?
        public string StyleAmbassador { get; set; }

        public Dictionary<int, string> Months { get { return ApplicationService.GetMonths(); } }

        public Dictionary<int, string> Years { get { return ApplicationService.GetYears(10); } }
    }

    public class PersonalViewModel
    {
        public PersonalViewModel()
        {
            CustomerStatusId = CustomerStatuses.Active;
        }

        public PersonalViewModel(Customer customer)
            : base()
        {
            LoadFromCustomer(customer);
        }

        public int CustomerId { get; set; }

        public int CustomerTypeId { get; set; }

        public int CustomerStatusId { get; set; }

        public int? SponsorId { get; set; }

        public int? EnrollerId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Address MainAddress { get; set; }


        public Address MailingAddress { get; set; }

        [Required]
        public string PrimaryPhone { get; set; }

        [Required]
        [RegularExpression(@"^[-a-z-A-Z0-9_}{?]+(\.[-a-z-A-Z0-9}{\'?]+)*@([a-z-A-Z0-9_][-a-z0-9_]*)+(\.[-a-z-A-Z0-9}{\'?]+)+$", ErrorMessage = @"Please Enter a valid email")]
        [System.Web.Mvc.Remote("CheckExistingEmail", "Shopping", ErrorMessage = "This email address is already registered. Please click <a href=\"../shopping/loginorregister\">here</a> to login, or click <a href=\"../ForgotPassword\">recover</a> if you've forgotten your password.")]
        public string Email { get; set; }

        [Required]
        [RegularExpression(@"^.{6,}$", ErrorMessage = @"Password must be at least 6 characters")]
        public string Password { get; set; }

        [Required]
        [System.Web.Mvc.Compare("Password", ErrorMessage = @"Confirmation Password and Password do not match")]
        public string ConfirmPassword { get; set; }
        public Customer ToCustomer()
        {
            var customer = new Customer
            {
                CustomerID = CustomerId,
                CustomerTypeID = CustomerTypeId,
                CustomerStatusID = CustomerStatusId,
                SponsorID = SponsorId,
                EnrollerID = EnrollerId,
                FirstName = FirstName,
                LastName = LastName,
                MainAddress = MainAddress,
                MailingAddress = MailingAddress,
                PrimaryPhone = PrimaryPhone,
                Email = Email,
                LoginName = Email,
                Password = Password

            };

            return customer;
        }

        protected virtual void LoadFromCustomer(Customer customer)
        {
            CustomerId = customer.CustomerID;
            CustomerTypeId = customer.CustomerTypeID;
            CustomerStatusId = customer.CustomerStatusID;
            SponsorId = customer.SponsorID;
            EnrollerId = customer.EnrollerID;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            MainAddress = customer.MainAddress;
            MailingAddress = customer.MailingAddress;
            PrimaryPhone = customer.PrimaryPhone;
            Email = customer.Email;

        }
    }
    public class GuestViewModel
    {
        public GuestViewModel()
        {
            CustomerStatusId = CustomerStatuses.Active;
        }

        public GuestViewModel(Customer customer)
            : base()
        {
            LoadFromCustomer(customer);
        }

        public bool IsGuest { get; set; }

        public int CustomerId { get; set; }

        public int CustomerTypeId { get; set; }

        public int CustomerStatusId { get; set; }

        public int? SponsorId { get; set; }

        public int? EnrollerId { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public Address MainAddress { get; set; }

        public Address MailingAddress { get; set; }
        public string SameAsAddress { get; set; }
        [Required]
        public string PrimaryPhone { get; set; }

        [Required]
        [RegularExpression(@"^[-a-z-A-Z0-9_}{?]+(\.[-a-z-A-Z0-9}{\'?]+)*@([a-z-A-Z0-9_][-a-z0-9_]*)+(\.[-a-z-A-Z0-9}{\'?]+)+$", ErrorMessage = @"Please Enter a valid email")]
        public string Email { get; set; }

        public Customer ToCustomer()
        {
            var customer = new Customer
            {
                CustomerID = CustomerId,
                CustomerTypeID = CustomerTypeId,
                CustomerStatusID = CustomerStatusId,
                SponsorID = SponsorId,
                EnrollerID = EnrollerId,
                FirstName = FirstName,
                LastName = LastName,
                MainAddress = MainAddress,
                MailingAddress = MailingAddress,
                PrimaryPhone = PrimaryPhone,
                Email = Email
            };

            return customer;
        }

        protected virtual void LoadFromCustomer(Customer customer)
        {
            CustomerId = customer.CustomerID;
            CustomerTypeId = customer.CustomerTypeID;
            CustomerStatusId = customer.CustomerStatusID;
            SponsorId = customer.SponsorID;
            EnrollerId = customer.EnrollerID;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            MainAddress = customer.MainAddress;
            MailingAddress = customer.MailingAddress;
            PrimaryPhone = customer.PrimaryPhone;
            Email = customer.Email;
            IsGuest = true;
        }
    }
}
