using System.Collections.Generic;

namespace Common.ModelsEx.Shopping
{
    public class InternationalShippingRequestModel
    {
        public string carrier { get; set; }
        public string shippingMethod { get; set; }
        public string securityKey { get; set; }
        public string shippingCostTotal { get; set; }
        public string sourceCurrencyCode { get; set; }
        public string targetCurrencyCode { get; set; }
        public string discountTotal { get; set; }
        public string additionalInsuranceTotal { get; set; }
        public string languageCode { get; set; }
        public List<LandedCostAddressModel> addresses { get; set; }
        public List<LandedCostItemModel> items { get; set; }

    }
    public class InternationalShippingResponseModel
    {
        public string carrier { get; set; }
        public string shippingMethod { get; set; } 
        public string shippingCostTotal { get; set; }
        public string sourceCurrencyCode { get; set; }
        public string targetCurrencyCode { get; set; }
        public string discountTotal { get; set; }
        public string additionalInsuranceTotal { get; set; }
        public string  code { get; set; }
        public string  message { get; set; }       
        public string  dutiesTotal { get; set; }
        public string  taxesTotal { get; set; }
        public string  feesTotal { get; set; }
        public string  subTotal { get; set; }
        public string  grandTotal { get; set; }
        public string  landedCostTotal { get; set; }   
    }
    public class LandedCostAddressModel
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string address1 { get; set; }
        public string address2 { get; set; }
        public string city { get; set; }
        public string regionCode { get; set; }
        public string countryCode { get; set; }
        public string postalCode { get; set; }
        public string emailAddress { get; set; }
        public string nationalIdentificationNumber { get; set; }
        public string addressType { get; set; }

    }
    public class LandedCostItemModel
    {
        public string sku { get; set; }
        public string description  { get; set; }
        public string name { get; set; }
        public string price { get; set; }
        public int quantity { get; set; }
        public string category { get; set; }
        public string hsCode { get; set; }
        public int weight { get; set; }
        public string uom { get; set; }
        public string countryOfOrigin { get; set; }
        public string autoClassify { get; set; }   
             
    }
}