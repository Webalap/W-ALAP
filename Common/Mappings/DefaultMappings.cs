using AutoMapper;
using Common.Api.ExigoWebService;
using Common.Extensions;
using Common.ModelsEx.Shopping;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Reflection;
using ExigoCustomer = ExigoService.Customer;
using IOrder = Common.ModelsEx.Shopping.IOrder;


namespace Common.Mappings
{

    public class DefaultMappings : Profile {

        public override String ProfileName {
            get { return "Default"; }
        }


        protected override void Configure() {

            BindingFlags = BindingFlags.IgnoreCase |
                           BindingFlags.Public;

            CreateProductMappings();

            CreateIOrderMappings();

            CreateCustomerRequestMappings();
            CreateCustomerResponseMappings();

            CreateOrderResponseMappings();

            CreateCreditCardMappings();

            CreateAddressMappings();

            //CreatODataCustomerMappings();

            //CreatRankMapping();

        }


        //private void CreatODataCustomerMappings() {

        //    Mapper.CreateMap<ODataCustomer, ExigoCustomer>()
        //       .ForMember(dest => dest.CustomerID, ops => ops.MapFrom(src => src.CustomerID))
        //       .ForMember(dest => dest.CustomerTypeID, ops => ops.MapFrom(src => src.CustomerTypeID))
        //       .ForMember(dest => dest.CustomerStatusID, ops => ops.MapFrom(src => src.CustomerStatusID))
        //       .ForMember(dest => dest.FirstName, ops => ops.MapFrom(src => src.FirstName))
        //       .ForMember(dest => dest.LastName, ops => ops.MapFrom(src => src.LastName))
        //       .ForMember(dest => dest.MiddleName, ops => ops.MapFrom(src => src.MiddleName))
        //       .ForMember(dest => dest.NameSuffix, ops => ops.MapFrom(src => src.NameSuffix))
        //       .ForMember(dest => dest.LanguageID, ops => ops.MapFrom(src => src.LanguageID))
        //       .ForMember(dest => dest.SponsorID, ops => ops.MapFrom(src => src.SponsorID))
        //       .ForMember(dest => dest.EnrollerID, ops => ops.MapFrom(src => src.EnrollerID))
        //       .ForMember(dest => dest.Company, ops => ops.MapFrom(src => src.Company))
        //       .ForMember(dest => dest.Date1, ops => ops.MapFrom(src => src.Date1))
        //       .ForMember(dest => dest.BirthDate, ops => ops.MapFrom(src => src.BirthDate))
        //       .ForMember(dest => dest.CreatedDate, ops => ops.MapFrom(src => src.CreatedDate))
        //       .ForMember(dest => dest.HighestAchievedRankId, ops => ops.MapFrom(src => src.RankID))
        //       .ForMember(dest => dest.HighestAchievedRank, ops => ops.MapFrom(src => src.Rank))
        //       .ForMember(dest => dest.Email, ops => ops.MapFrom(src => src.Email))
        //       .ForMember(dest => dest.PrimaryPhone, ops => ops.MapFrom(src => src.Phone))
        //       .ForMember(dest => dest.SecondaryPhone, ops => ops.MapFrom(src => src.Phone2))
        //       .ForMember(dest => dest.MobilePhone, ops => ops.MapFrom(src => src.MobilePhone))
        //       .ForMember(dest => dest.MainAddress, ops => ops.MapFrom(src => new Address 
        //        {
        //            AddressType = AddressType.Main,
        //            Address1 = src.MainAddress1,
        //            Address2 = src.MainAddress2,
        //            City = src.MainCity,
        //            State = src.MainState,
        //            Zip = src.MainZip,
        //            Country = src.MainCountry
        //        }))
        //        .ForMember(dest => dest.MailingAddress, ops => ops.MapFrom(src => new Address
        //        {
        //            AddressType = AddressType.Mailing,
        //            Address1 = src.MailAddress1,
        //            Address2 = src.MailAddress2,
        //            City = src.MailCity,
        //            State = src.MailState,
        //            Zip = src.MailZip,
        //            Country = src.MailCountry
        //        }))
        //        .ForMember(dest => dest.OtherAddress, ops => ops.MapFrom(src => new Address
        //        {
        //            AddressType = AddressType.Other,
        //            Address1 = src.OtherAddress1,
        //            Address2 = src.OtherAddress2,
        //            City = src.OtherCity,
        //            State = src.OtherState,
        //            Zip = src.OtherZip,
        //            Country = src.OtherCountry
        //        }))
        //        .IgnoreAllNonExisting();

        //}


        private void CreateOrderResponseMappings() {

            Mapper.CreateMap<OrderDetailResponse, OrderDetail>()
                  .ForMember(dest => dest.ItemDescription, ops => ops.MapFrom(src => src.Description))
                  .ForMember(dest => dest.BV, ops => ops.MapFrom(src => src.BusinesVolume))
                  .IgnoreAllNonExisting();

            Mapper.CreateMap<OrderDetailResponse, AutoOrderDetail>()
                  .ForMember(dest => dest.ItemDescription, ops => ops.MapFrom(src => src.Description))
                  .ForMember(dest => dest.BV, ops => ops.MapFrom(src => src.BusinesVolume))
                  .IgnoreAllNonExisting();

            Mapper.CreateMap<CreateOrderResponse, Order>()
                  .ForMember(dest => dest.Details, ops => ops.MapFrom(src => Mapper.Map<ICollection<OrderDetail>>(src.Details)))
                  .IgnoreAllNonExisting();

            Mapper.CreateMap<CreateAutoOrderResponse, AutoOrder>()
                  .ForMember(dest => dest.Details, ops => ops.MapFrom(src => Mapper.Map<ICollection<AutoOrderDetail>>(src.Details)))
                  .IgnoreAllNonExisting();

        }


        private void CreateCreditCardMappings() {

            // Required to set the 
            CreateMap<CreditCard, ChargeCreditCardTokenRequest>()
                .ForMember(dest => dest.CvcCode, ops => ops.MapFrom(src => src.CVV))
                .ForMember(dest => dest.BillingName, ops => ops.MapFrom(src => src.NameOnCard))
                .ForMember(dest => dest.BillingAddress, ops => ops.MapFrom(src => src.BillingAddress.Address1))
                .ForMember(dest => dest.BillingCity, ops => ops.MapFrom(src => src.BillingAddress.City))
                .ForMember(dest => dest.BillingState, ops => ops.MapFrom(src => src.BillingAddress.State))
                .ForMember(dest => dest.BillingZip, ops => ops.MapFrom(src => src.BillingAddress.Zip))
                .ForMember(dest => dest.BillingCountry, ops => ops.MapFrom(src => src.BillingAddress.Country))
                ;

        }


        private void CreateCustomerRequestMappings() {

            CreateMap<ExigoCustomer, CreateCustomerRequest>()
                .ForMember(dest => dest.CustomerType, ops => ops.MapFrom(src => src.CustomerTypeID))
                .ForMember(dest => dest.CustomerStatus, ops => ops.MapFrom(src => src.CustomerStatusID))
                .ForMember(dest => dest.FirstName, ops => ops.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, ops => ops.MapFrom(src => src.LastName))
                .ForMember(dest => dest.SponsorID, ops => ops.MapFrom(src => src.SponsorID))
                .ForMember(dest => dest.EnrollerID, ops => ops.MapFrom(src => src.EnrollerID))
                .ForMember(dest => dest.Phone, ops => ops.MapFrom(src => src.PrimaryPhone))
                .ForMember(dest => dest.Phone2, ops => ops.MapFrom(src => src.SecondaryPhone))
                .ForMember(dest => dest.MobilePhone, ops => ops.MapFrom(src => src.MobilePhone))
                .ForMember(dest => dest.Email, ops => ops.MapFrom(src => src.Email))
                .ForMember(dest => dest.MainAddress1, ops => ops.MapFrom(src => src.MainAddress.Address1))
                .ForMember(dest => dest.MainAddress2, ops => ops.MapFrom(src => src.MainAddress.Address2))
                .ForMember(dest => dest.MainCity, ops => ops.MapFrom(src => src.MainAddress.City))
                .ForMember(dest => dest.MainState, ops => ops.MapFrom(src => src.MainAddress.State))
                .ForMember(dest => dest.MainZip, ops => ops.MapFrom(src => src.MainAddress.Zip))
                .ForMember(dest => dest.MainCountry, ops => ops.MapFrom(src => src.MainAddress.Country))
                .ForMember(dest => dest.MailAddress1, ops => ops.MapFrom(src => src.MailingAddress.Address1))
                .ForMember(dest => dest.MailAddress2, ops => ops.MapFrom(src => src.MailingAddress.Address1))
                .ForMember(dest => dest.MailCity, ops => ops.MapFrom(src => src.MailingAddress.City))
                .ForMember(dest => dest.MailState, ops => ops.MapFrom(src => src.MailingAddress.State))
                .ForMember(dest => dest.MailZip, ops => ops.MapFrom(src => src.MailingAddress.Zip))
                .ForMember(dest => dest.MailCountry, ops => ops.MapFrom(src => src.MailingAddress.Country))
                .ForMember(dest => dest.OtherAddress1, ops => ops.MapFrom(src => src.OtherAddress.Address1))
                .ForMember(dest => dest.OtherAddress2, ops => ops.MapFrom(src => src.OtherAddress.Address1))
                .ForMember(dest => dest.OtherCity, ops => ops.MapFrom(src => src.OtherAddress.City))
                .ForMember(dest => dest.OtherState, ops => ops.MapFrom(src => src.OtherAddress.State))
                .ForMember(dest => dest.OtherZip, ops => ops.MapFrom(src => src.OtherAddress.Zip))
                .ForMember(dest => dest.OtherCountry, ops => ops.MapFrom(src => src.OtherAddress.Country))
                .ForMember(dest => dest.LoginName, ops => ops.MapFrom(src => src.LoginName))
                .ForMember(dest => dest.LoginPassword, ops => ops.MapFrom(src => src.Password))
                ;

        }


        private void CreateCustomerResponseMappings() {

            CreateMap<CustomerResponse, ExigoCustomer>()
                .ForMember(dest => dest.CustomerID, ops => ops.MapFrom(src => src.CustomerID))
                .ForMember(dest => dest.CustomerTypeID, ops => ops.MapFrom(src => src.CustomerType))
                .ForMember(dest => dest.CustomerStatusID, ops => ops.MapFrom(src => src.CustomerStatus))
                .ForMember(dest => dest.FirstName, ops => ops.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, ops => ops.MapFrom(src => src.LastName))
                .ForMember(dest => dest.SponsorID, ops => ops.MapFrom(src => src.SponsorID))
                .ForMember(dest => dest.EnrollerID, ops => ops.MapFrom(src => src.EnrollerID))
                .ForMember(dest => dest.PrimaryPhone, ops => ops.MapFrom(src => src.Phone))
                .ForMember(dest => dest.SecondaryPhone, ops => ops.MapFrom(src => src.Phone2))
                .ForMember(dest => dest.MobilePhone, ops => ops.MapFrom(src => src.MobilePhone))
                .ForMember(dest => dest.Email, ops => ops.MapFrom(src => src.Email))
                .ForMember(dest => dest.LoginName, ops => ops.MapFrom(src => src.LoginName))
                .ForMember(dest => dest.MainAddress, ops => ops.MapFrom(src => new Address
               {
                   AddressType = AddressType.Main,
                   Address1 = src.MainAddress1,
                   Address2 = src.MainAddress2,
                   City = src.MainCity,
                   State = src.MainState,
                   Zip = src.MainZip,
                   Country = src.MainCountry
               }))
                .ForMember(dest => dest.MailingAddress, ops => ops.MapFrom(src => new Address
                {
                    AddressType = AddressType.Mailing,
                    Address1 = src.MailAddress1,
                    Address2 = src.MailAddress2,
                    City = src.MailCity,
                    State = src.MailState,
                    Zip = src.MailZip,
                    Country = src.MailCountry
                }))
                .ForMember(dest => dest.OtherAddress, ops => ops.MapFrom(src => new Address
                {
                    AddressType = AddressType.Other,
                    Address1 = src.OtherAddress1,
                    Address2 = src.OtherAddress2,
                    City = src.OtherCity,
                    State = src.OtherState,
                    Zip = src.OtherZip,
                    Country = src.OtherCountry
                }))
                .IgnoreAllNonExisting();

        }


        private void CreateProductMappings() {

            CreateMap<Product, OrderDetailRequest>()
                .ForMember(dest => dest.ItemCode, ops => ops.MapFrom(src => src.ItemCode))
                .ForMember(dest => dest.Quantity, ops => ops.MapFrom(src => src.Quantity))
                .ForMember(dest => dest.PriceEachOverride, ops => ops.MapFrom(src => src.PriceEachOverride))
            ;

            CreateMap<ItemResponse, Product>()
                .ForMember(dest => dest.ItemCode, ops => ops.MapFrom(src => src.ItemCode))
                .ForMember(dest => dest.Description, ops => ops.MapFrom(src => src.Description))
                .ForMember(dest => dest.ShortDetail, ops => ops.MapFrom(src => src.ShortDetail))
                .ForMember(dest => dest.ShortDetail4, ops => ops.MapFrom(src => src.ShortDetail4))
                .ForMember(dest => dest.LongDetail1, ops => ops.MapFrom(src => src.LongDetail))
                .ForMember(dest => dest.CategoryId, ops => ops.MapFrom(src => src.CategoryID))
                .ForMember(dest => dest.Price, ops => ops.MapFrom(src => src.Price))
                .ForMember(dest => dest.LargePicture, ops => ops.MapFrom(
                    src => GlobalUtilities.GetProductImagePath(src.LargePicture, src.ItemCode)))
                .ForMember(dest => dest.SmallPicture, ops => ops.MapFrom(
                    src => GlobalUtilities.GetProductImagePath(src.SmallPicture, src.ItemCode)))
                .ForMember(dest => dest.TinyPicture, ops => ops.MapFrom(
                    src => GlobalUtilities.GetProductImagePath(src.TinyPicture, src.ItemCode)))
                .ForMember(dest => dest.Field4, ops => ops.MapFrom(src => src.Field4))
                .ForMember(dest => dest.Field5, ops => ops.MapFrom(src => src.Field5))
                .ForMember(dest => dest.Field6, ops => ops.MapFrom(src => src.Field6))
                ;

        }


        private void CreateIOrderMappings() {

            CreateMap<IOrder, CalculateOrderRequest>()
                .ForMember(dest => dest.Address1, ops => ops.MapFrom(src => src.ShippingAddress.Address1))
                .ForMember(dest => dest.City, ops => ops.MapFrom(src => src.ShippingAddress.City))
                .ForMember(dest => dest.State, ops => ops.MapFrom(src => src.ShippingAddress.State))
                .ForMember(dest => dest.Zip, ops => ops.MapFrom(src => src.ShippingAddress.Zip))
                .ForMember(dest => dest.Country, ops => ops.MapFrom(src => src.ShippingAddress.Country))
                .ForMember(dest => dest.ShipMethodID, ops => ops.MapFrom(src => src.ShippingMethodId))
                .ForMember(dest => dest.Details, ops => ops.MapFrom(src => src.Products))
                .ForMember(dest => dest.PriceType, ops => ops.MapFrom(src => src.Configuration.PriceTypeID))
                .ForMember(dest => dest.CurrencyCode, ops => ops.MapFrom(src => src.Configuration.CurrencyCode))
                .ForMember(dest => dest.WarehouseID, ops => ops.MapFrom(src => src.Configuration.WarehouseID))
            ;

        }


        private void CreateAddressMappings() {

            CreateMap<Address, ShippingAddress>()
                .ForMember(dest => dest.Address1, ops => ops.MapFrom(src => src.Address1))
                .ForMember(dest => dest.Address2, ops => ops.MapFrom(src => src.Address2))
                .ForMember(dest => dest.City, ops => ops.MapFrom(src => src.City))
                .ForMember(dest => dest.State, ops => ops.MapFrom(src => src.State))
                .ForMember(dest => dest.Zip, ops => ops.MapFrom(src => src.Zip))
                .ForMember(dest => dest.Country, ops => ops.MapFrom(src => src.Country));

        }


        //private void CreatRankMapping() {

        //    Mapper.CreateMap<ODataRank, ExigoRank>()
        //          .ForMember( dest => dest.RankID, ops => ops.MapFrom( src => src.RankID ) )
        //          .ForMember( dest => dest.RankDescription, ops => ops.MapFrom( src => src.RankDescription ) )
        //          .IgnoreAllNonExisting();

        //}

    }

}
