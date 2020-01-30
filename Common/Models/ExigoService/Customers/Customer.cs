using Common;
using Common.Filters;
using Common.ModelBinders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace ExigoService
{

    public class Customer : ICustomer
    {

        public Customer()
        {

            MainAddress = new Address() { AddressType = AddressType.Main };
            MailingAddress = new Address() { AddressType = AddressType.Mailing };
            OtherAddress = new Address() { AddressType = AddressType.Other };

        }

        public Customer(Customer other)
        {

            CustomerID = other.CustomerID;

            CustomerTypeID = other.CustomerTypeID;
            CustomerStatusID = other.CustomerStatusID;
            DefaultWarehouseID = other.DefaultWarehouseID;
            LanguageID = other.LanguageID;

            CreatedDate = other.CreatedDate;

            BirthDate = other.BirthDate;

            TaxID = other.TaxID;

            PayableToName = other.PayableToName;

            PayableTypeID = other.PayableTypeID;

            IsOptedIn = other.IsOptedIn;

            CanLogin = other.CanLogin;

            LoginName = other.LoginName;
            Password = other.Password;


            FirstName = other.FirstName;
            LeadFirstName = other.LeadFirstName;

            MiddleName = other.MiddleName;

            LastName = other.LastName;
            LeadLastName = other.LeadLastName;

            NameSuffix = other.NameSuffix;


            Company = other.Company;


            PrimaryPhone = other.PrimaryPhone;
            LeadPrimaryPhone = other.LeadPrimaryPhone;

            SecondaryPhone = other.SecondaryPhone;
            MobilePhone = other.MobilePhone;

            Fax = other.Fax;

            Email = other.Email;
            LeadEmail = other.LeadEmail;


            MainAddress = (other.MainAddress == null) ? null : other.MainAddress.DeepClone();
            MailingAddress = (other.MailingAddress == null) ? null : other.MailingAddress.DeepClone();
            OtherAddress = (other.OtherAddress == null) ? null : other.OtherAddress.DeepClone();


            CustomerType = (other.CustomerType == null) ? null : other.CustomerType.DeepClone();
            CustomerStatus = (other.CustomerStatus == null) ? null : other.CustomerStatus.DeepClone();


            EnrollerID = other.EnrollerID;
            SponsorID = other.SponsorID;

            Enroller = other.Enroller; //TODO: Need to deepclone or re-retrieve this customer object.
            Sponsor = other.Sponsor;  //TODO: Need to deepclone or re-retrieve this customer object.

            WebAlias = other.WebAlias;

            HighestAchievedRankId = other.HighestAchievedRankId;
            HighestAchievedRank = (other.HighestAchievedRank == null) ? null : other.HighestAchievedRank.DeepClone();

            Field1 = other.Field1;
            Field2 = other.Field2;
            Field3 = other.Field3;
            Field4 = other.Field4;
            Field5 = other.Field5;
            Field6 = other.Field6;
            Field7 = other.Field7;
            Field8 = other.Field8;
            Field9 = other.Field9;
            Field10 = other.Field10;
            Field11 = other.Field11;
            Field12 = other.Field12;
            Field13 = other.Field13;
            Field14 = other.Field14;
            Field15 = other.Field15;


            Date1 = other.Date1;
            Date2 = other.Date2;
            Date3 = other.Date3;
            Date4 = other.Date4;
            Date5 = other.Date5;

        }

        #region CustomerAddresses
        public string Phone { get; set; }
        public string MainAddress1 { get; set; }
        public string MainAddress2 { get; set; }
        public string MainCity { get; set; }
        public string MainState { get; set; }
        public string MainZip { get; set; }
        public string MainCountry { get; set; }
        public string MailAddress1 { get; set; }
        public string MailAddress2 { get; set; }
        public string MailCity { get; set; }
        public string MailState { get; set; }
        public string MailZip { get; set; }
        public string MailCountry { get; set; }
        public string OtherAddress1 { get; set; }
        public string OtherAddress2 { get; set; }
        public string OtherCity { get; set; }
        public string OtherState { get; set; }
        public string OtherZip { get; set; }
        public string OtherCountry { get; set; }
        #endregion


        public int CustomerID { get; set; }


        [Required(ErrorMessage = "First Name is required"), Display(Name = "First Name")]
        public string FirstName { get; set; }
        public string LeadFirstName { get; set; }

        // gwb:20141206 - Added.
        public string MiddleName { get; set; }

        [Required(ErrorMessage = "Last Name is required"), Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string LeadLastName { get; set; }

        public string NameSuffix { get; set; }


        [Display(Name = "Company")]
        public string Company { get; set; }


        public int CustomerTypeID { get; set; }
        public int CustomerStatusID { get; set; }
        public int DefaultWarehouseID { get; set; }
        public int LanguageID { get; set; }

        public DateTime CreatedDate { get; set; }


        [PropertyBinder(typeof(BirthDateModelBinder)), DataType("BirthDate")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "SSN/TaxID is required"), Display(Name = "SSN/Tax ID"), DataType("TaxID")]
        public string TaxID { get; set; }

        [Required]
        public string PayableToName { get; set; }

        [Required]
        public int PayableTypeID { get; set; }

        public bool IsOptedIn { get; set; }

        public bool CanLogin { get; set; }
        public string LeadId { get; set; }
        public string Notes { get; set; }
        public string LeadStatus { get; set; }


        [Required, DataType(DataType.EmailAddress), RegularExpression(GlobalSettings.RegularExpressions.EmailAddresses, ErrorMessage = "This email doesn't look right - can you check it again?"), Display(Name = "Email")]
        public string Email { get; set; }

        [DataType(DataType.EmailAddress), RegularExpression(GlobalSettings.RegularExpressions.EmailAddresses, ErrorMessage = "This email doesn't look right - can you check it again?"), Display(Name = "Email")]
        public string LeadEmail { get; set; }


        [Required, DataType(DataType.PhoneNumber), Display(Name = "Daytime Phone")]
        public string PrimaryPhone { get; set; }

        [DataType(DataType.PhoneNumber), Display(Name = "Daytime Phone")]
        public string LeadPrimaryPhone { get; set; }


        [DataType(DataType.PhoneNumber), Display(Name = "Evening Phone")]
        public string SecondaryPhone { get; set; }


        [DataType(DataType.PhoneNumber), Display(Name = "Mobile Phone")]
        public string MobilePhone { get; set; }


        [DataType(DataType.PhoneNumber), Display(Name = "Fax Number")]
        public string Fax { get; set; }


        [DataType("Address")]
        public Address MainAddress { get; set; }

        [DataType("Address")]
        public Address MailingAddress { get; set; }

        [DataType("Address")]
        public Address OtherAddress { get; set; }


        public List<Address> Addresses
        {
            get
            {
                var collection = new List<Address>();
                if (this.MainAddress != null && this.MainAddress.IsComplete) collection.Add(this.MainAddress);
                if (this.MailingAddress != null && this.MailingAddress.IsComplete) collection.Add(this.MailingAddress);
                if (this.OtherAddress != null && this.OtherAddress.IsComplete) collection.Add(this.OtherAddress);
                return collection;
            }
            set { }
        }


        [Required, DataType("LoginName"), Remote("IsLoginNameAvailable", "App", ErrorMessage = "This username isn't available - try another one."), RegularExpression(GlobalSettings.RegularExpressions.LoginName, ErrorMessage = "Make sure your username doesn't contain any spaces or special characters."), Display(Name = "Username")]
        public string LoginName { get; set; }

        [Required, DataType("NewPassword"), RegularExpression(GlobalSettings.RegularExpressions.Password, ErrorMessage = "Make sure your password isn't more than 50 characters long."), Display(Name = "Password")]
        public string Password { get; set; }


        public int? EnrollerID { get; set; }
        public int? SponsorID { get; set; }


        public int HighestAchievedRankId { get; set; }
        public Rank HighestAchievedRank { get; set; }


        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
        public string Field4 { get; set; }
        public string Field5 { get; set; }
        public string Field6 { get; set; }
        public string Field7 { get; set; }
        public string Field8 { get; set; }
        public string Field9 { get; set; }
        public string Field10 { get; set; }
        public string Field11 { get; set; }
        public string Field12 { get; set; }
        public string Field13 { get; set; }
        public string Field14 { get; set; }
        public string Field15 { get; set; }


        public DateTime? Date1 { get; set; }
        public DateTime? Date2 { get; set; }
        public DateTime? Date3 { get; set; }
        public DateTime? Date4 { get; set; }
        public DateTime? Date5 { get; set; }


        public CustomerType CustomerType { get; set; }
        public CustomerStatus CustomerStatus { get; set; }


        public Customer Enroller { get; set; }
        public Customer Sponsor { get; set; }


        public string WebAlias { get; set; }


        public string FullName
        {
            get { return String.Join(" ", FirstName, LastName); }
        }

        public string AvatarUrl
        {
            get { return GlobalUtilities.GetCustomerAvatarUrl(CustomerID); }
        }

    }

}
