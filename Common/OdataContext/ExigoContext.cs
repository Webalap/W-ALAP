using System;
using System.Collections.Generic;
namespace Common.Api.ExigoOData
{

    /// <summary>
    /// There are no comments for ExigoContext in the schema.
    /// </summary>
    /// <summary>
    public partial class ExigoContext
    {

    }
    /// There are no comments for Exigo.Api.Models.Customer in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerID
    /// </KeyProperties>
    public partial class Customer
    {
        /// <summary>
        /// Create a new Customer object.
        /// </summary>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="customerTypeID">Initial value of CustomerTypeID.</param>
        /// <param name="customerStatusID">Initial value of CustomerStatusID.</param>
        /// <param name="rankID">Initial value of RankID.</param>
        /// <param name="defaultWarehouseID">Initial value of DefaultWarehouseID.</param>
        /// <param name="payableTypeID">Initial value of PayableTypeID.</param>
        /// <param name="checkThreshold">Initial value of CheckThreshold.</param>
        /// <param name="languageID">Initial value of LanguageID.</param>
        /// <param name="isSalesTaxExempt">Initial value of IsSalesTaxExempt.</param>
        /// <param name="useBinaryHoldingTank">Initial value of UseBinaryHoldingTank.</param>
        /// <param name="isInBinaryHoldingTank">Initial value of IsInBinaryHoldingTank.</param>
        /// <param name="createdDate">Initial value of CreatedDate.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        /// <param name="canLogin">Initial value of CanLogin.</param>
        /// <param name="binaryPlacementTypeID">Initial value of BinaryPlacementTypeID.</param>
        /// <param name="isEmailSubscribed">Initial value of IsEmailSubscribed.</param>
        public static Customer CreateCustomer(
                    int customerID,
                    int customerTypeID,
                    int customerStatusID,
                    int rankID,
                    int defaultWarehouseID,
                    int payableTypeID,
                    decimal checkThreshold,
                    int languageID,
                    bool isSalesTaxExempt,
                    bool useBinaryHoldingTank,
                    bool isInBinaryHoldingTank,
                    global::System.DateTime createdDate,
                    global::System.DateTime modifiedDate,
                    bool canLogin,
                    int binaryPlacementTypeID,
                    bool isEmailSubscribed)
        {
            Customer customer = new Customer();
            customer.CustomerID = customerID;
            customer.CustomerTypeID = customerTypeID;
            customer.CustomerStatusID = customerStatusID;
            customer.RankID = rankID;
            customer.DefaultWarehouseID = defaultWarehouseID;
            customer.PayableTypeID = payableTypeID;
            customer.CheckThreshold = checkThreshold;
            customer.LanguageID = languageID;
            customer.IsSalesTaxExempt = isSalesTaxExempt;
            customer.UseBinaryHoldingTank = useBinaryHoldingTank;
            customer.IsInBinaryHoldingTank = isInBinaryHoldingTank;
            customer.CreatedDate = createdDate;
            customer.ModifiedDate = modifiedDate;
            customer.CanLogin = canLogin;
            customer.BinaryPlacementTypeID = binaryPlacementTypeID;
            customer.IsEmailSubscribed = isEmailSubscribed;
            return customer;
        }
        /// <summary>
        /// There are no comments for Property CustomerID in the schema.
        /// </summary>
        public int CustomerID { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string NameSuffix { get; set; }
        public string Company { get; set; }
        public int CustomerTypeID { get; set; }
        public int CustomerStatusID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string MobilePhone { get; set; }
        public string Fax { get; set; }
        public string MainAddress1 { get; set; }
        public string MainAddress2 { get; set; }
        public string MainAddress3 { get; set; }
        public string MainCity { get; set; }
        public string MainState { get; set; }
        public string MainZip { get; set; }
        public string MainCountry { get; set; }
        public string MainCounty { get; set; }
        public string MailAddress1 { get; set; }
        public string MailAddress2 { get; set; }
        public string MailAddress3 { get; set; }
        public string MailCity { get; set; }
        public string MailState { get; set; }
        public string MailZip { get; set; }
        public string MailCountry { get; set; }
        public string MailCounty { get; set; }
        public string OtherAddress1 { get; set; }
        public string OtherAddress2 { get; set; }
        public string OtherAddress3 { get; set; }
        public string OtherCity { get; set; }
        public string OtherState { get; set; }
        public string OtherZip { get; set; }
        public string OtherCountry { get; set; }
        public string OtherCounty { get; set; }
        public string LoginName { get; set; }
        public int RankID { get; set; }
        public global::System.Nullable<int> EnrollerID { get; set; }
        public global::System.Nullable<int> SponsorID { get; set; }
        public global::System.Nullable<global::System.DateTime> BirthDate { get; set; }
        public string CurrencyCode { get; set; }
        public string PayableToName { get; set; }
        public int DefaultWarehouseID { get; set; }
        public int PayableTypeID { get; set; }
        public decimal CheckThreshold { get; set; }
        public int LanguageID { get; set; }
        public string Gender { get; set; }
        public string SalesTaxCode { get; set; }
        public bool IsSalesTaxExempt { get; set; }
        public bool UseBinaryHoldingTank { get; set; }
        public bool IsInBinaryHoldingTank { get; set; }
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
        public global::System.Nullable<global::System.DateTime> Date1 { get; set; }
        public global::System.Nullable<global::System.DateTime> Date2 { get; set; }
        public global::System.Nullable<global::System.DateTime> Date3 { get; set; }
        public DateTime? Date4 { get; set; }
        public DateTime? Date5 { get; set; }
        public global::System.DateTime CreatedDate { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public string Notes { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public bool CanLogin { get; set; }
        public int BinaryPlacementTypeID { get; set; }
        public string VatRegistration { get; set; }
        public bool IsEmailSubscribed { get; set; }
        public string EmailSubscribeIP { get; set; }
        public Rank Rank { get; set; }
        public CustomerType CustomerType { get; set; }
        public CustomerStatus CustomerStatus { get; set; }
        public Customer Enroller { get; set; }
        public Customer Sponsor { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.Rank in the schema.
    /// </summary>
    /// <KeyProperties>
    /// RankID
    /// </KeyProperties>
    public partial class Rank
    {
        public int RankID { get; set; }
        public string RankDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerTypeID
    /// </KeyProperties>
    public partial class CustomerType
    {
        public int CustomerTypeID { get; set; }
        public string CustomerTypeDescription { get; set; }
        public int PriceTypeID { get; set; }
        public PriceType PriceType { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.PriceType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PriceTypeID
    /// </KeyProperties>
    public partial class PriceType
    {
        public int PriceTypeID { get; set; }
        public string PriceTypeDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerStatus in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerStatusID
    /// </KeyProperties>
    public partial class CustomerStatus
    {
        public int CustomerStatusID { get; set; }
        public string CustomerStatusDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerSocialNetwork in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerID
    /// SocialNetworkID
    /// </KeyProperties>
    public partial class CustomerSocialNetwork
    {
        /// <summary>
        /// Create a new CustomerSocialNetwork object.
        /// </summary>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="socialNetworkID">Initial value of SocialNetworkID.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        public static CustomerSocialNetwork CreateCustomerSocialNetwork(int customerID, int socialNetworkID, global::System.DateTime modifiedDate)
        {
            CustomerSocialNetwork customerSocialNetwork = new CustomerSocialNetwork();
            customerSocialNetwork.CustomerID = customerID;
            customerSocialNetwork.SocialNetworkID = socialNetworkID;
            customerSocialNetwork.ModifiedDate = modifiedDate;
            return customerSocialNetwork;
        }
        /// <summary>
        /// There are no comments for Property CustomerID in the schema.
        /// </summary>
        public int CustomerID { get; set; }
        public int SocialNetworkID { get; set; }
        public string Url { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public Customer Customer { get; set; }
        public SocialNetwork SocialNetwork { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.SocialNetwork in the schema.
    /// </summary>
    /// <KeyProperties>
    /// SocialNetworkID
    /// </KeyProperties>
    public partial class SocialNetwork 
    {
        /// <summary>
        /// Create a new SocialNetwork object.
        /// </summary>
        /// <param name="socialNetworkID">Initial value of SocialNetworkID.</param>
        public static SocialNetwork CreateSocialNetwork(int socialNetworkID)
        {
            SocialNetwork socialNetwork = new SocialNetwork();
            socialNetwork.SocialNetworkID = socialNetworkID;
            return socialNetwork;
        }
        /// <summary>
        /// There are no comments for Property SocialNetworkID in the schema.
        /// </summary>
        public int SocialNetworkID { get; set; }
        public string SocialNetworkDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.Order in the schema.
    /// </summary>
    /// <KeyProperties>
    /// OrderID
    /// </KeyProperties>
    public partial class Order 
    {
        /// <summary>
        /// Create a new Order object.
        /// </summary>
        /// <param name="orderID">Initial value of OrderID.</param>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="orderStatusID">Initial value of OrderStatusID.</param>
        /// <param name="orderDate">Initial value of OrderDate.</param>
        /// <param name="warehouseID">Initial value of WarehouseID.</param>
        /// <param name="shipMethodID">Initial value of ShipMethodID.</param>
        /// <param name="orderTypeID">Initial value of OrderTypeID.</param>
        /// <param name="priceTypeID">Initial value of PriceTypeID.</param>
        /// <param name="total">Initial value of Total.</param>
        /// <param name="subTotal">Initial value of SubTotal.</param>
        /// <param name="taxTotal">Initial value of TaxTotal.</param>
        /// <param name="shippingTotal">Initial value of ShippingTotal.</param>
        /// <param name="discountTotal">Initial value of DiscountTotal.</param>
        /// <param name="discountPercent">Initial value of DiscountPercent.</param>
        /// <param name="weightTotal">Initial value of WeightTotal.</param>
        /// <param name="businessVolumeTotal">Initial value of BusinessVolumeTotal.</param>
        /// <param name="commissionableVolumeTotal">Initial value of CommissionableVolumeTotal.</param>
        /// <param name="other1Total">Initial value of Other1Total.</param>
        /// <param name="other2Total">Initial value of Other2Total.</param>
        /// <param name="other3Total">Initial value of Other3Total.</param>
        /// <param name="other4Total">Initial value of Other4Total.</param>
        /// <param name="other5Total">Initial value of Other5Total.</param>
        /// <param name="other6Total">Initial value of Other6Total.</param>
        /// <param name="other7Total">Initial value of Other7Total.</param>
        /// <param name="other8Total">Initial value of Other8Total.</param>
        /// <param name="other9Total">Initial value of Other9Total.</param>
        /// <param name="other10Total">Initial value of Other10Total.</param>
        /// <param name="shippingTax">Initial value of ShippingTax.</param>
        /// <param name="orderTax">Initial value of OrderTax.</param>
        /// <param name="fedTaxTotal">Initial value of FedTaxTotal.</param>
        /// <param name="stateTaxTotal">Initial value of StateTaxTotal.</param>
        /// <param name="fedShippingTax">Initial value of FedShippingTax.</param>
        /// <param name="stateShippingTax">Initial value of StateShippingTax.</param>
        /// <param name="cityShippingTax">Initial value of CityShippingTax.</param>
        /// <param name="cityLocalShippingTax">Initial value of CityLocalShippingTax.</param>
        /// <param name="countyShippingTax">Initial value of CountyShippingTax.</param>
        /// <param name="countyLocalShippingTax">Initial value of CountyLocalShippingTax.</param>
        /// <param name="declineCount">Initial value of DeclineCount.</param>
        /// <param name="createdDate">Initial value of CreatedDate.</param>
        /// <param name="taxFedRate">Initial value of TaxFedRate.</param>
        /// <param name="taxStateRate">Initial value of TaxStateRate.</param>
        /// <param name="taxCityRate">Initial value of TaxCityRate.</param>
        /// <param name="taxCityLocalRate">Initial value of TaxCityLocalRate.</param>
        /// <param name="taxCountyRate">Initial value of TaxCountyRate.</param>
        /// <param name="taxCountyLocalRate">Initial value of TaxCountyLocalRate.</param>
        /// <param name="taxManualRate">Initial value of TaxManualRate.</param>
        /// <param name="taxIsExempt">Initial value of TaxIsExempt.</param>
        /// <param name="taxIsOverRide">Initial value of TaxIsOverRide.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        public static Order CreateOrder(
                    int orderID,
                    int customerID,
                    int orderStatusID,
                    global::System.DateTime orderDate,
                    int warehouseID,
                    int shipMethodID,
                    int orderTypeID,
                    int priceTypeID,
                    decimal total,
                    decimal subTotal,
                    decimal taxTotal,
                    decimal shippingTotal,
                    decimal discountTotal,
                    decimal discountPercent,
                    decimal weightTotal,
                    decimal businessVolumeTotal,
                    decimal commissionableVolumeTotal,
                    decimal other1Total,
                    decimal other2Total,
                    decimal other3Total,
                    decimal other4Total,
                    decimal other5Total,
                    decimal other6Total,
                    decimal other7Total,
                    decimal other8Total,
                    decimal other9Total,
                    decimal other10Total,
                    decimal shippingTax,
                    decimal orderTax,
                    decimal fedTaxTotal,
                    decimal stateTaxTotal,
                    decimal fedShippingTax,
                    decimal stateShippingTax,
                    decimal cityShippingTax,
                    decimal cityLocalShippingTax,
                    decimal countyShippingTax,
                    decimal countyLocalShippingTax,
                    int declineCount,
                    global::System.DateTime createdDate,
                    decimal taxFedRate,
                    decimal taxStateRate,
                    decimal taxCityRate,
                    decimal taxCityLocalRate,
                    decimal taxCountyRate,
                    decimal taxCountyLocalRate,
                    decimal taxManualRate,
                    bool taxIsExempt,
                    bool taxIsOverRide,
                    global::System.DateTime modifiedDate)
        {
            Order order = new Order();
            order.OrderID = orderID;
            order.CustomerID = customerID;
            order.OrderStatusID = orderStatusID;
            order.OrderDate = orderDate;
            order.WarehouseID = warehouseID;
            order.ShipMethodID = shipMethodID;
            order.OrderTypeID = orderTypeID;
            order.PriceTypeID = priceTypeID;
            order.Total = total;
            order.SubTotal = subTotal;
            order.TaxTotal = taxTotal;
            order.ShippingTotal = shippingTotal;
            order.DiscountTotal = discountTotal;
            order.DiscountPercent = discountPercent;
            order.WeightTotal = weightTotal;
            order.BusinessVolumeTotal = businessVolumeTotal;
            order.CommissionableVolumeTotal = commissionableVolumeTotal;
            order.Other1Total = other1Total;
            order.Other2Total = other2Total;
            order.Other3Total = other3Total;
            order.Other4Total = other4Total;
            order.Other5Total = other5Total;
            order.Other6Total = other6Total;
            order.Other7Total = other7Total;
            order.Other8Total = other8Total;
            order.Other9Total = other9Total;
            order.Other10Total = other10Total;
            order.ShippingTax = shippingTax;
            order.OrderTax = orderTax;
            order.FedTaxTotal = fedTaxTotal;
            order.StateTaxTotal = stateTaxTotal;
            order.FedShippingTax = fedShippingTax;
            order.StateShippingTax = stateShippingTax;
            order.CityShippingTax = cityShippingTax;
            order.CityLocalShippingTax = cityLocalShippingTax;
            order.CountyShippingTax = countyShippingTax;
            order.CountyLocalShippingTax = countyLocalShippingTax;
            order.DeclineCount = declineCount;
            order.CreatedDate = createdDate;
            order.TaxFedRate = taxFedRate;
            order.TaxStateRate = taxStateRate;
            order.TaxCityRate = taxCityRate;
            order.TaxCityLocalRate = taxCityLocalRate;
            order.TaxCountyRate = taxCountyRate;
            order.TaxCountyLocalRate = taxCountyLocalRate;
            order.TaxManualRate = taxManualRate;
            order.TaxIsExempt = taxIsExempt;
            order.TaxIsOverRide = taxIsOverRide;
            order.ModifiedDate = modifiedDate;
            return order;
        }
        /// <summary>
        /// There are no comments for Property OrderID in the schema.
        /// </summary>
        public int OrderID { get; set; }
        public int CustomerID { get; set; }
        public int OrderStatusID { get; set; }
        public global::System.DateTime OrderDate { get; set; }
        public string CurrencyCode { get; set; }
        public int WarehouseID { get; set; }
        public int ShipMethodID { get; set; }
        public int OrderTypeID { get; set; }
        public int PriceTypeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NameSuffix { get; set; }
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal DiscountPercent { get; set; }
        public decimal WeightTotal { get; set; }
        public decimal BusinessVolumeTotal { get; set; }
        public decimal CommissionableVolumeTotal { get; set; }
        public string TrackingNumber1 { get; set; }
        public string TrackingNumber2 { get; set; }
        public string TrackingNumber3 { get; set; }
        public string TrackingNumber4 { get; set; }
        public string TrackingNumber5 { get; set; }
        public decimal Other1Total { get; set; }
        public decimal Other2Total { get; set; }
        public decimal Other3Total { get; set; }
        public decimal Other4Total { get; set; }
        public decimal Other5Total { get; set; }
        public decimal Other6Total { get; set; }
        public decimal Other7Total { get; set; }
        public decimal Other8Total { get; set; }
        public decimal Other9Total { get; set; }
        public decimal Other10Total { get; set; }
        public decimal ShippingTax { get; set; }
        public decimal OrderTax { get; set; }
        public decimal FedTaxTotal { get; set; }
        public decimal StateTaxTotal { get; set; }
        public decimal FedShippingTax { get; set; }
        public decimal StateShippingTax { get; set; }
        public decimal CityShippingTax { get; set; }
        public decimal CityLocalShippingTax { get; set; }
        public decimal CountyShippingTax { get; set; }
        public decimal CountyLocalShippingTax { get; set; }
        public string Other11 { get; set; }
        public string Other12 { get; set; }
        public string Other13 { get; set; }
        public string Other14 { get; set; }
        public string Other15 { get; set; }
        public string Other16 { get; set; }
        public string Other17 { get; set; }
        public string Other18 { get; set; }
        public string Other19 { get; set; }
        public string Other20 { get; set; }
        public global::System.Nullable<int> AutoOrderID { get; set; }
        public global::System.Nullable<int> ReturnOrderID { get; set; }
        public global::System.Nullable<int> ParentOrderID { get; set; }
        public global::System.Nullable<int> TransferToCustomerID { get; set; }
        public int DeclineCount { get; set; }
        public global::System.Nullable<global::System.DateTime> ShippedDate { get; set; }
        public global::System.DateTime CreatedDate { get; set; }
        public decimal TaxFedRate { get; set; }
        public decimal TaxStateRate { get; set; }
        public decimal TaxCityRate { get; set; }
        public decimal TaxCityLocalRate { get; set; }
        public decimal TaxCountyRate { get; set; }
        public decimal TaxCountyLocalRate { get; set; }
        public decimal TaxManualRate { get; set; }
        public string TaxCity { get; set; }
        public string TaxCounty { get; set; }
        public string TaxState { get; set; }
        public string TaxZip { get; set; }
        public string TaxCountry { get; set; }
        public bool TaxIsExempt { get; set; }
        public bool TaxIsOverRide { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public global::System.Nullable<int> PartyID { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<OrderDetail> Details { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<Payment> Payments { get; set; }
        public Customer Customer { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public Warehouse Warehouse { get; set; }
        public ShipMethod ShipMethod { get; set; }
        public PriceType PriceType { get; set; }
        public OrderType OrderType { get; set; }
        public Party Party { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.OrderDetail in the schema.
    /// </summary>
    /// <KeyProperties>
    /// OrderDetailID
    /// </KeyProperties>
    public partial class OrderDetail
    {
        /// <summary>
        /// Create a new OrderDetail object.
        /// </summary>
        /// <param name="orderDetailID">Initial value of OrderDetailID.</param>
        /// <param name="orderID">Initial value of OrderID.</param>
        /// <param name="itemID">Initial value of ItemID.</param>
        /// <param name="quantity">Initial value of Quantity.</param>
        /// <param name="priceEach">Initial value of PriceEach.</param>
        /// <param name="priceTotal">Initial value of PriceTotal.</param>
        /// <param name="tax">Initial value of Tax.</param>
        /// <param name="weightEach">Initial value of WeightEach.</param>
        /// <param name="weight">Initial value of Weight.</param>
        /// <param name="businessVolumeEach">Initial value of BusinessVolumeEach.</param>
        /// <param name="businessVolume">Initial value of BusinessVolume.</param>
        /// <param name="commissionableVolumeEach">Initial value of CommissionableVolumeEach.</param>
        /// <param name="commissionableVolume">Initial value of CommissionableVolume.</param>
        /// <param name="other1Each">Initial value of Other1Each.</param>
        /// <param name="other1">Initial value of Other1.</param>
        /// <param name="other2Each">Initial value of Other2Each.</param>
        /// <param name="other2">Initial value of Other2.</param>
        /// <param name="other3Each">Initial value of Other3Each.</param>
        /// <param name="other3">Initial value of Other3.</param>
        /// <param name="other4Each">Initial value of Other4Each.</param>
        /// <param name="other4">Initial value of Other4.</param>
        /// <param name="other5Each">Initial value of Other5Each.</param>
        /// <param name="other5">Initial value of Other5.</param>
        /// <param name="other6Each">Initial value of Other6Each.</param>
        /// <param name="other6">Initial value of Other6.</param>
        /// <param name="other7Each">Initial value of Other7Each.</param>
        /// <param name="other7">Initial value of Other7.</param>
        /// <param name="other8Each">Initial value of Other8Each.</param>
        /// <param name="other8">Initial value of Other8.</param>
        /// <param name="other9Each">Initial value of Other9Each.</param>
        /// <param name="other9">Initial value of Other9.</param>
        /// <param name="other10Each">Initial value of Other10Each.</param>
        /// <param name="other10">Initial value of Other10.</param>
        /// <param name="taxable">Initial value of Taxable.</param>
        /// <param name="fedTax">Initial value of FedTax.</param>
        /// <param name="stateTax">Initial value of StateTax.</param>
        /// <param name="cityTax">Initial value of CityTax.</param>
        /// <param name="cityLocalTax">Initial value of CityLocalTax.</param>
        /// <param name="countyTax">Initial value of CountyTax.</param>
        /// <param name="countyLocalTax">Initial value of CountyLocalTax.</param>
        /// <param name="manualTax">Initial value of ManualTax.</param>
        /// <param name="isStateTaxOverride">Initial value of IsStateTaxOverride.</param>
        public static OrderDetail CreateOrderDetail(
                    int orderDetailID,
                    int orderID,
                    int itemID,
                    decimal quantity,
                    decimal priceEach,
                    decimal priceTotal,
                    decimal tax,
                    decimal weightEach,
                    decimal weight,
                    decimal businessVolumeEach,
                    decimal businessVolume,
                    decimal commissionableVolumeEach,
                    decimal commissionableVolume,
                    decimal other1Each,
                    decimal other1,
                    decimal other2Each,
                    decimal other2,
                    decimal other3Each,
                    decimal other3,
                    decimal other4Each,
                    decimal other4,
                    decimal other5Each,
                    decimal other5,
                    decimal other6Each,
                    decimal other6,
                    decimal other7Each,
                    decimal other7,
                    decimal other8Each,
                    decimal other8,
                    decimal other9Each,
                    decimal other9,
                    decimal other10Each,
                    decimal other10,
                    decimal taxable,
                    decimal fedTax,
                    decimal stateTax,
                    decimal cityTax,
                    decimal cityLocalTax,
                    decimal countyTax,
                    decimal countyLocalTax,
                    decimal manualTax,
                    bool isStateTaxOverride)
        {
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.OrderDetailID = orderDetailID;
            orderDetail.OrderID = orderID;
            orderDetail.ItemID = itemID;
            orderDetail.Quantity = quantity;
            orderDetail.PriceEach = priceEach;
            orderDetail.PriceTotal = priceTotal;
            orderDetail.Tax = tax;
            orderDetail.WeightEach = weightEach;
            orderDetail.Weight = weight;
            orderDetail.BusinessVolumeEach = businessVolumeEach;
            orderDetail.BusinessVolume = businessVolume;
            orderDetail.CommissionableVolumeEach = commissionableVolumeEach;
            orderDetail.CommissionableVolume = commissionableVolume;
            orderDetail.Other1Each = other1Each;
            orderDetail.Other1 = other1;
            orderDetail.Other2Each = other2Each;
            orderDetail.Other2 = other2;
            orderDetail.Other3Each = other3Each;
            orderDetail.Other3 = other3;
            orderDetail.Other4Each = other4Each;
            orderDetail.Other4 = other4;
            orderDetail.Other5Each = other5Each;
            orderDetail.Other5 = other5;
            orderDetail.Other6Each = other6Each;
            orderDetail.Other6 = other6;
            orderDetail.Other7Each = other7Each;
            orderDetail.Other7 = other7;
            orderDetail.Other8Each = other8Each;
            orderDetail.Other8 = other8;
            orderDetail.Other9Each = other9Each;
            orderDetail.Other9 = other9;
            orderDetail.Other10Each = other10Each;
            orderDetail.Other10 = other10;
            orderDetail.Taxable = taxable;
            orderDetail.FedTax = fedTax;
            orderDetail.StateTax = stateTax;
            orderDetail.CityTax = cityTax;
            orderDetail.CityLocalTax = cityLocalTax;
            orderDetail.CountyTax = countyTax;
            orderDetail.CountyLocalTax = countyLocalTax;
            orderDetail.ManualTax = manualTax;
            orderDetail.IsStateTaxOverride = isStateTaxOverride;
            return orderDetail;
        }
        /// <summary>
        /// There are no comments for Property OrderDetailID in the schema.
        /// </summary>
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceEach { get; set; }
        public decimal PriceTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal WeightEach { get; set; }
        public decimal Weight { get; set; }
        public decimal BusinessVolumeEach { get; set; }
        public decimal BusinessVolume { get; set; }
        public decimal CommissionableVolumeEach { get; set; }
        public decimal CommissionableVolume { get; set; }
        public decimal Other1Each { get; set; }
        public decimal Other1 { get; set; }
        public decimal Other2Each { get; set; }
        public decimal Other2 { get; set; }
        public decimal Other3Each { get; set; }
        public decimal Other3 { get; set; }
        public decimal Other4Each { get; set; }
        public decimal Other4 { get; set; }
        public decimal Other5Each { get; set; }

        public decimal Other5 { get; set; }
        public decimal Other6Each { get; set; }
        public decimal Other6 { get; set; }
        public decimal Other7Each { get; set; }
        public decimal Other7 { get; set; }
        public decimal Other8Each { get; set; }
        public decimal Other8 { get; set; }
        public decimal Other9Each { get; set; }
        public decimal Other9 { get; set; }
        public decimal Other10Each { get; set; }
        public decimal Other10 { get; set; }
        public decimal Taxable { get; set; }
        public decimal FedTax { get; set; }
        public decimal StateTax { get; set; }
        public decimal CityTax { get; set; }
        public decimal CityLocalTax { get; set; }
        public decimal CountyTax { get; set; }
        public decimal CountyLocalTax { get; set; }
        public decimal ManualTax { get; set; }
        public bool IsStateTaxOverride { get; set; }
        public global::System.Nullable<int> ParentItemID { get; set; }
        public string ParentItemCode { get; set; }
        public string Reference1 { get; set; }
        public Order Order { get; set; }
    }
    public partial class Payment 
    {
        /// <summary>
        /// Create a new Payment object.
        /// </summary>
        /// <param name="paymentID">Initial value of PaymentID.</param>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="orderID">Initial value of OrderID.</param>
        /// <param name="paymentTypeID">Initial value of PaymentTypeID.</param>
        /// <param name="paymentDate">Initial value of PaymentDate.</param>
        /// <param name="amount">Initial value of Amount.</param>
        /// <param name="warehouseID">Initial value of WarehouseID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Payment CreatePayment(int paymentID, int customerID, int orderID, int paymentTypeID, global::System.DateTime paymentDate, decimal amount, int warehouseID)
        {
            Payment payment = new Payment();
            payment.PaymentID = paymentID;
            payment.CustomerID = customerID;
            payment.OrderID = orderID;
            payment.PaymentTypeID = paymentTypeID;
            payment.PaymentDate = paymentDate;
            payment.Amount = amount;
            payment.WarehouseID = warehouseID;
            return payment;
        }
        public int PaymentID { get; set; }
        public int CustomerID { get; set; }
        public int OrderID { get; set; }
        public int PaymentTypeID { get; set; }
        public global::System.DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string CurrencyCode { get; set; }
        public int WarehouseID { get; set; }
        public string BillingName { get; set; }
        public global::System.Nullable<int> CreditCardTypeID { get; set; }
        public string CreditCardNumber { get; set; }
        public string AuthorizationCode { get; set; }
        public string Memo { get; set; }
        public PaymentType PaymentType { get; set; }
        public CreditCardType CreditCardType { get; set; }
        public Customer Customer { get; set; }
        public Order Order { get; set; }
    }
    public partial class PaymentType 
    {

        public static PaymentType CreatePaymentType(int paymentTypeID)
        {
            PaymentType paymentType = new PaymentType();
            paymentType.PaymentTypeID = paymentTypeID;
            return paymentType;
        }
        public int PaymentTypeID { get; set; }
        public string PaymentTypeDescription { get; set; }
    }
    public partial class CreditCardType 
    {
        public static CreditCardType CreateCreditCardType(int creditCardTypeID)
        {
            CreditCardType creditCardType = new CreditCardType();
            creditCardType.CreditCardTypeID = creditCardTypeID;
            return creditCardType;
        }
        public int CreditCardTypeID { get; set; }
        public string CreditCardTypeDescription { get; set; }
    }
    public class OrderStatus
    {
        public static OrderStatus CreateOrderStatus(int orderStatusID)
        {
            OrderStatus orderStatus = new OrderStatus();
            orderStatus.OrderStatusID = orderStatusID;
            return orderStatus;
        }
        public int OrderStatusID { get; set; }
        public string OrderStatusDescription { get; set; }
    }
    public partial class Warehouse 
    {
        public static Warehouse CreateWarehouse(int warehouseID, int timeZoneID)
        {
            Warehouse warehouse = new Warehouse();
            warehouse.WarehouseID = warehouseID;
            warehouse.TimeZoneID = timeZoneID;
            return warehouse;
        }
        public int WarehouseID { get; set; }
        public string WarehouseDescription { get; set; }
        public string WarehouseAddress1 { get; set; }
        public string WarehouseAddress2 { get; set; }
        public string WarehouseCity { get; set; }
        public string WarehouseState { get; set; }
        public string WarehouseZip { get; set; }
        public string WarehouseCountry { get; set; }
        public int TimeZoneID { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<WarehouseCurrency> Currencies { get; set; }
    }
    public partial class WarehouseCurrency 
    {
        public static WarehouseCurrency CreateWarehouseCurrency(int warehouseID, string currencyCode, int sortOrder)
        {
            WarehouseCurrency warehouseCurrency = new WarehouseCurrency();
            warehouseCurrency.WarehouseID = warehouseID;
            warehouseCurrency.CurrencyCode = currencyCode;
            warehouseCurrency.SortOrder = sortOrder;
            return warehouseCurrency;
        }
        public int WarehouseID { get; set; }
        public string CurrencyCode { get; set; }
        public int SortOrder { get; set; }
        public Warehouse Warehouse { get; set; }
        public Currency Currency { get; set; }
    }
    public partial class Currency
    {
        public static Currency CreateCurrency(string currencyCode)
        {
            Currency currency = new Currency();
            currency.CurrencyCode = currencyCode;
            return currency;
        }
        public string CurrencyCode { get; set; }
        public string CurrencyDescription { get; set; }
        public string CurrencySymbol { get; set; }
    }
    public partial class ShipMethod 
    {
        public static ShipMethod CreateShipMethod(int shipMethodID, int warehouseID, int shipCarrierID, bool displayOnWeb)
        {
            ShipMethod shipMethod = new ShipMethod();
            shipMethod.ShipMethodID = shipMethodID;
            shipMethod.WarehouseID = warehouseID;
            shipMethod.ShipCarrierID = shipCarrierID;
            shipMethod.DisplayOnWeb = displayOnWeb;
            return shipMethod;
        }
        public int ShipMethodID { get; set; }
        public string ShipMethodDescription { get; set; }
        public int WarehouseID { get; set; }
        public int ShipCarrierID { get; set; }
        public bool DisplayOnWeb { get; set; }
        public ShipCarrier ShipCarrier { get; set; }
        public Warehouse Warehouse { get; set; }
    }
    public partial class ShipCarrier 
    {
        public static ShipCarrier CreateShipCarrier(int shipCarrierID)
        {
            ShipCarrier shipCarrier = new ShipCarrier();
            shipCarrier.ShipCarrierID = shipCarrierID;
            return shipCarrier;
        }
        public int ShipCarrierID { get; set; }
        public string ShipCarrierDescription { get; set; }
    }
    public partial class OrderType 
    {
        public static OrderType CreateOrderType(int orderTypeID)
        {
            OrderType orderType = new OrderType();
            orderType.OrderTypeID = orderTypeID;
            return orderType;
        }
        public int OrderTypeID { get; set; }
        public string OrderTypeDescription { get; set; }
    }
    public partial class Party 
    {
        public static Party CreateParty(int partyID, int hostID, int distributorID, global::System.DateTime startDate, int partyTypeID, int partyStatusID, int languageID, global::System.DateTime modifiedDate)
        {
            Party party = new Party();
            party.PartyID = partyID;
            party.HostID = hostID;
            party.DistributorID = distributorID;
            party.StartDate = startDate;
            party.PartyTypeID = partyTypeID;
            party.PartyStatusID = partyStatusID;
            party.LanguageID = languageID;
            party.ModifiedDate = modifiedDate;
            return party;
        }
        public int PartyID { get; set; }
        public int HostID { get; set; }
        public int DistributorID { get; set; }
        public global::System.DateTime StartDate { get; set; }
        public global::System.Nullable<global::System.DateTime> CloseDate { get; set; }
        public string Description { get; set; }
        public global::System.Nullable<global::System.DateTime> EventStartDate { get; set; }
        public global::System.Nullable<global::System.DateTime> EventEndDate { get; set; }
        public int PartyTypeID { get; set; }
        public int PartyStatusID { get; set; }
        public int LanguageID { get; set; }
        public string Information { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public PartyType PartyType { get; set; }
        public PartyStatus PartyStatus { get; set; }
        public Language Language { get; set; }
        public Customer Distributor { get; set; }
        public Customer Host { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<Guest> Guests { get; set; }
    }
    public partial class PartyType 
    {
        public static PartyType CreatePartyType(int partyTypeID)
        {
            PartyType partyType = new PartyType();
            partyType.PartyTypeID = partyTypeID;
            return partyType;
        }
        public int PartyTypeID { get; set; }
        public string PartyTypeDescription { get; set; }
    }
    public partial class PartyStatus 
    {
        public static PartyStatus CreatePartyStatus(int partyStatusID)
        {
            PartyStatus partyStatus = new PartyStatus();
            partyStatus.PartyStatusID = partyStatusID;
            return partyStatus;
        }
        public int PartyStatusID { get; set; }
        public string PartyStatusDescription { get; set; }
    }
    public partial class Language 
    {
        public static Language CreateLanguage(int languageID)
        {
            Language language = new Language();
            language.LanguageID = languageID;
            return language;
        }
        public int LanguageID { get; set; }
        public string LanguageDescription { get; set; }
        public string CultureCode { get; set; }
    }
    public partial class Guest 
    {
        public static Guest CreateGuest(int guestID, int hostID, int guestStatusID, global::System.DateTime createdDate, global::System.DateTime modifiedDate)
        {
            Guest guest = new Guest();
            guest.GuestID = guestID;
            guest.HostID = hostID;
            guest.GuestStatusID = guestStatusID;
            guest.CreatedDate = createdDate;
            guest.ModifiedDate = modifiedDate;
            return guest;
        }
        public int GuestID { get; set; }
        public global::System.Nullable<int> CustomerID { get; set; }
        public int HostID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NameSuffix { get; set; }
        public string Company { get; set; }
        public string Gender { get; set; }
        public int GuestStatusID { get; set; }
        public global::System.Nullable<int> LanguageID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string MobilePhone { get; set; }
        public string Email { get; set; }
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
        public global::System.Nullable<global::System.DateTime> Date1 { get; set; }
        public global::System.Nullable<global::System.DateTime> Date2 { get; set; }
        public global::System.Nullable<global::System.DateTime> Date3 { get; set; }
        public global::System.Nullable<global::System.DateTime> Date4 { get; set; }
        public global::System.Nullable<global::System.DateTime> Date5 { get; set; }
        public string Notes { get; set; }
        public global::System.DateTime CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Customer Customer { get; set; }
        public Customer Host { get; set; }
        public Language Language { get; set; }
        public GuestStatus GuestStatus { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<GuestSocialNetwork> SocialNetworks { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<Party> Parties { get; set; }
    }
    public partial class GuestStatus 
    {
        public int GuestStatusID { get; set; }
        public string GuestStatusDescription { get; set; }
    }
    public partial class GuestSocialNetwork 
    {
        public static GuestSocialNetwork CreateGuestSocialNetwork(int guestID, int socialNetworkID, global::System.DateTime modifiedDate)
        {
            GuestSocialNetwork guestSocialNetwork = new GuestSocialNetwork();
            guestSocialNetwork.GuestID = guestID;
            guestSocialNetwork.SocialNetworkID = socialNetworkID;
            guestSocialNetwork.ModifiedDate = modifiedDate;
            return guestSocialNetwork;
        }
        public int GuestID { get; set; }
        public int SocialNetworkID { get; set; }
        public string Url { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public Guest Guest { get; set; }
        public SocialNetwork SocialNetwork { get; set; }
    }
    public partial class EmailAttachment 
    {
        public static EmailAttachment CreateEmailAttachment(int attachmentID, int mailID, int contentLength)
        {
            EmailAttachment emailAttachment = new EmailAttachment();
            emailAttachment.AttachmentID = attachmentID;
            emailAttachment.MailID = mailID;
            emailAttachment.ContentLength = contentLength;
            return emailAttachment;
        }
        public int AttachmentID { get; set; }
        public int MailID { get; set; }
        public string FileName { get; set; }
        public int ContentLength { get; set; }
        public Email Email { get; set; }
    }
    public partial class Email 
    {
        public static Email CreateEmail(int mailID, int customerID, int mailFolderID, int mailSize, int mailStatusTypeID, global::System.DateTime mailDate, bool hasAttachment, int importance)
        {
            Email email = new Email();
            email.MailID = mailID;
            email.CustomerID = customerID;
            email.MailFolderID = mailFolderID;
            email.MailSize = mailSize;
            email.MailStatusTypeID = mailStatusTypeID;
            email.MailDate = mailDate;
            email.HasAttachment = hasAttachment;
            email.Importance = importance;
            return email;
        }
        public int MailID { get; set; }
        public int CustomerID { get; set; }
        public int MailFolderID { get; set; }
        public int MailSize { get; set; }
        public int MailStatusTypeID { get; set; }
        public global::System.DateTime MailDate { get; set; }
        public bool HasAttachment { get; set; }
        public int Importance { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string MailFromDisplay { get; set; }
        public string MailFrom { get; set; }
        public string MailTo { get; set; }
        public string ReplyTo { get; set; }
        public string MailCC { get; set; }
        public string MailBCC { get; set; }
        public EmailFolder MailFolder { get; set; }
        public EmailStatusType MailStatusType { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<EmailAttachment> Attachments { get; set; }
    }
    public partial class EmailFolder 
    {
        public static EmailFolder CreateEmailFolder(int mailFolderID, int customerID, int mailCount, int unreadCount, int folderSize, int mailFolderTypeID)
        {
            EmailFolder emailFolder = new EmailFolder();
            emailFolder.MailFolderID = mailFolderID;
            emailFolder.CustomerID = customerID;
            emailFolder.MailCount = mailCount;
            emailFolder.UnreadCount = unreadCount;
            emailFolder.FolderSize = folderSize;
            emailFolder.MailFolderTypeID = mailFolderTypeID;
            return emailFolder;
        }
        public int MailFolderID { get; set; }
        public int CustomerID { get; set; }
        public int MailCount { get; set; }
        public int UnreadCount { get; set; }
        public string Name { get; set; }
        public int FolderSize { get; set; }
        public int MailFolderTypeID { get; set; }
        public EmailFolderType MailFolderType { get; set; }
    }
    public partial class EmailFolderType 
    {
        public static EmailFolderType CreateEmailFolderType(int mailFolderTypeID)
        {
            EmailFolderType emailFolderType = new EmailFolderType();
            emailFolderType.MailFolderTypeID = mailFolderTypeID;
            return emailFolderType;
        }
        public int MailFolderTypeID { get; set; }
        public string MailFolderTypeDescription { get; set; }
    }
    public partial class EmailStatusType 
    {
        public static EmailStatusType CreateEmailStatusType(int mailStatusTypeID)
        {
            EmailStatusType emailStatusType = new EmailStatusType();
            emailStatusType.MailStatusTypeID = mailStatusTypeID;
            return emailStatusType;
        }
        public int MailStatusTypeID { get; set; }
        public string MailStatusTypeDescription { get; set; }
    }
    public partial class AutoOrder 
    {
        public static AutoOrder CreateAutoOrder(
                    int autoOrderID,
                    int customerID,
                    int autoOrderStatusID,
                    int frequencyTypeID,
                    global::System.DateTime startDate,
                    int warehouseID,
                    int shipMethodID,
                    int autoOrderPaymentTypeID,
                    int autoOrderProcessTypeID,
                    decimal total,
                    decimal subTotal,
                    decimal taxTotal,
                    decimal shippingTotal,
                    decimal discountTotal,
                    decimal businessVolumeTotal,
                    decimal commissionableVolumeTotal,
                    global::System.DateTime createdDate,
                    global::System.DateTime modifiedDate)
        {
            AutoOrder autoOrder = new AutoOrder();
            autoOrder.AutoOrderID = autoOrderID;
            autoOrder.CustomerID = customerID;
            autoOrder.AutoOrderStatusID = autoOrderStatusID;
            autoOrder.FrequencyTypeID = frequencyTypeID;
            autoOrder.StartDate = startDate;
            autoOrder.WarehouseID = warehouseID;
            autoOrder.ShipMethodID = shipMethodID;
            autoOrder.AutoOrderPaymentTypeID = autoOrderPaymentTypeID;
            autoOrder.AutoOrderProcessTypeID = autoOrderProcessTypeID;
            autoOrder.Total = total;
            autoOrder.SubTotal = subTotal;
            autoOrder.TaxTotal = taxTotal;
            autoOrder.ShippingTotal = shippingTotal;
            autoOrder.DiscountTotal = discountTotal;
            autoOrder.BusinessVolumeTotal = businessVolumeTotal;
            autoOrder.CommissionableVolumeTotal = commissionableVolumeTotal;
            autoOrder.CreatedDate = createdDate;
            autoOrder.ModifiedDate = modifiedDate;
            return autoOrder;
        }
        public int AutoOrderID { get; set; }
        public int CustomerID { get; set; }
        public int AutoOrderStatusID { get; set; }
        public int FrequencyTypeID { get; set; }
        public global::System.DateTime StartDate { get; set; }
        public global::System.Nullable<global::System.DateTime> StopDate { get; set; }
        public global::System.Nullable<global::System.DateTime> CancelledDate { get; set; }
        public global::System.Nullable<global::System.DateTime> LastRunDate { get; set; }
        public global::System.Nullable<global::System.DateTime> NextRunDate { get; set; }
        public string CurrencyCode { get; set; }
        public int WarehouseID { get; set; }
        public int ShipMethodID { get; set; }
        public int AutoOrderPaymentTypeID { get; set; }
        public int AutoOrderProcessTypeID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string NameSuffix { get; set; }
        public string Company { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string County { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Notes { get; set; }
        public decimal Total { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxTotal { get; set; }
        public decimal ShippingTotal { get; set; }
        public decimal DiscountTotal { get; set; }
        public decimal BusinessVolumeTotal { get; set; }
        public decimal CommissionableVolumeTotal { get; set; }
        public string AutoOrderDescription { get; set; }
        public global::System.DateTime CreatedDate { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public string Other11 { get; set; }
        public string Other12 { get; set; }
        public string Other13 { get; set; }
        public string Other14 { get; set; }
        public string Other15 { get; set; }
        public string Other16 { get; set; }
        public string Other17 { get; set; }
        public string Other18 { get; set; }
        public string Other19 { get; set; }
        public string Other20 { get; set; }
        public List<AutoOrderDetail> Details { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<AutoOrderSchedule> Schedules { get; set; }
        public Customer Customer { get; set; }
        public AutoOrderStatus AutoOrderStatus { get; set; }
        public FrequencyType FrequencyType { get; set; }
        public Warehouse Warehouse { get; set; }
        public ShipMethod ShipMethod { get; set; }
        public AutoOrderPaymentType AutoOrderPaymentType { get; set; }
        public AutoOrderProcessType AutoOrderProcessType { get; set; }
    }
    public partial class AutoOrderDetail 
    {
        public static AutoOrderDetail CreateAutoOrderDetail(int autoOrderDetailID, int autoOrderID, int itemID, decimal quantity, decimal priceEach, decimal priceTotal, decimal businessVolumeEach, decimal businessVolume, decimal commissionableVolumeEach, decimal commissionableVolume)
        {
            AutoOrderDetail autoOrderDetail = new AutoOrderDetail();
            autoOrderDetail.AutoOrderDetailID = autoOrderDetailID;
            autoOrderDetail.AutoOrderID = autoOrderID;
            autoOrderDetail.ItemID = itemID;
            autoOrderDetail.Quantity = quantity;
            autoOrderDetail.PriceEach = priceEach;
            autoOrderDetail.PriceTotal = priceTotal;
            autoOrderDetail.BusinessVolumeEach = businessVolumeEach;
            autoOrderDetail.BusinessVolume = businessVolume;
            autoOrderDetail.CommissionableVolumeEach = commissionableVolumeEach;
            autoOrderDetail.CommissionableVolume = commissionableVolume;
            return autoOrderDetail;
        }
        public int AutoOrderDetailID { get; set; }
        public int AutoOrderID { get; set; }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceEach { get; set; }
        public decimal PriceTotal { get; set; }
        public decimal BusinessVolumeEach { get; set; }
        public decimal BusinessVolume { get; set; }
        public decimal CommissionableVolumeEach { get; set; }
        public decimal CommissionableVolume { get; set; }
        public global::System.Nullable<decimal> PriceEachOverride { get; set; }
        public global::System.Nullable<decimal> TaxableEachOverride { get; set; }
        public global::System.Nullable<decimal> ShippingPriceEachOverride { get; set; }
        public global::System.Nullable<decimal> BusinessVolumeEachOverride { get; set; }
        public global::System.Nullable<decimal> CommissionableVolumeEachOverride { get; set; }
        public global::System.Nullable<int> ParentItemID { get; set; }
        public string ParentItemCode { get; set; }
        public string Reference1 { get; set; }
        public AutoOrder AutoOrder { get; set; }
    }
    public partial class AutoOrderSchedule 
    {
        public static AutoOrderSchedule CreateAutoOrderSchedule(int autoOrderID, global::System.DateTime scheduledDate, bool isEnabled)
        {
            AutoOrderSchedule autoOrderSchedule = new AutoOrderSchedule();
            autoOrderSchedule.AutoOrderID = autoOrderID;
            autoOrderSchedule.ScheduledDate = scheduledDate;
            autoOrderSchedule.IsEnabled = isEnabled;
            return autoOrderSchedule;
        }
        public int AutoOrderID { get; set; }
        public global::System.DateTime ScheduledDate { get; set; }
        public bool IsEnabled { get; set; }
        public global::System.Nullable<global::System.DateTime> ProcessedDate { get; set; }
        public global::System.Nullable<int> OrderID { get; set; }
        public AutoOrder AutoOrder { get; set; }
        public Order Order { get; set; }
    }
    public partial class AutoOrderStatus 
    {
        public static AutoOrderStatus CreateAutoOrderStatus(int autoOrderStatusID)
        {
            AutoOrderStatus autoOrderStatus = new AutoOrderStatus();
            autoOrderStatus.AutoOrderStatusID = autoOrderStatusID;
            return autoOrderStatus;
        }
        public int AutoOrderStatusID { get; set; }
        public string AutoOrderStatusDescription { get; set; }
    }
    public partial class FrequencyType 
    {
        public static FrequencyType CreateFrequencyType(int frequencyTypeID)
        {
            FrequencyType frequencyType = new FrequencyType();
            frequencyType.FrequencyTypeID = frequencyTypeID;
            return frequencyType;
        }
        public int FrequencyTypeID { get; set; }
        public string FrequencyTypeDescription { get; set; }
    }
    public partial class AutoOrderPaymentType 
    {
        public static AutoOrderPaymentType CreateAutoOrderPaymentType(int autoOrderPaymentTypeID)
        {
            AutoOrderPaymentType autoOrderPaymentType = new AutoOrderPaymentType();
            autoOrderPaymentType.AutoOrderPaymentTypeID = autoOrderPaymentTypeID;
            return autoOrderPaymentType;
        }
        public int AutoOrderPaymentTypeID { get; set; }
        public string AutoOrderPaymentTypeDescription { get; set; }
    }
    public partial class AutoOrderProcessType 
    {
        public static AutoOrderProcessType CreateAutoOrderProcessType(int autoOrderProcessTypeID)
        {
            AutoOrderProcessType autoOrderProcessType = new AutoOrderProcessType();
            autoOrderProcessType.AutoOrderProcessTypeID = autoOrderProcessTypeID;
            return autoOrderProcessType;
        }
        public int AutoOrderProcessTypeID { get; set; }
        public string AutoOrderProcessTypeDescription { get; set; }
    }
    public partial class Item 
    {
        public static Item CreateItem(int itemID, int itemTypeID, decimal weight, bool isVirtual, bool allowOnAutoOrder, bool otherCheck1, bool otherCheck2, bool otherCheck3, bool otherCheck4, bool otherCheck5, bool isGroupMaster, bool isDynamicKitMaster, bool availableAllCountryRegions, bool allowPartialAmounts)
        {
            Item item = new Item();
            item.ItemID = itemID;
            item.ItemTypeID = itemTypeID;
            item.Weight = weight;
            item.IsVirtual = isVirtual;
            item.AllowOnAutoOrder = allowOnAutoOrder;
            item.OtherCheck1 = otherCheck1;
            item.OtherCheck2 = otherCheck2;
            item.OtherCheck3 = otherCheck3;
            item.OtherCheck4 = otherCheck4;
            item.OtherCheck5 = otherCheck5;
            item.IsGroupMaster = isGroupMaster;
            item.IsDynamicKitMaster = isDynamicKitMaster;
            item.AvailableAllCountryRegions = availableAllCountryRegions;
            item.AllowPartialAmounts = allowPartialAmounts;
            return item;
        }
        public int ItemID { get; set; }
        public string ItemCode { get; set; }
        public string ItemDescription { get; set; }
        public string ShortDetail { get; set; }
        public string ShortDetail2 { get; set; }
        public string ShortDetail3 { get; set; }
        public string ShortDetail4 { get; set; }
        public string LongDetail { get; set; }
        public string LongDetail2 { get; set; }
        public string LongDetail3 { get; set; }
        public string LongDetail4 { get; set; }
        public string TinyImageUrl { get; set; }
        public string SmallImageUrl { get; set; }
        public string LargeImageUrl { get; set; }
        public int ItemTypeID { get; set; }
        public decimal Weight { get; set; }
        public bool IsVirtual { get; set; }
        public bool AllowOnAutoOrder { get; set; }
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
        public bool OtherCheck1 { get; set; }
        public bool OtherCheck2 { get; set; }
        public bool OtherCheck3 { get; set; }
        public bool OtherCheck4 { get; set; }
        public bool OtherCheck5 { get; set; }
        public bool IsGroupMaster { get; set; }
        public string GroupDescription { get; set; }
        public string GroupMembersDescription { get; set; }
        public bool IsDynamicKitMaster { get; set; }
        public bool AvailableAllCountryRegions { get; set; }
        public bool AllowPartialAmounts { get; set; }
        public string Auto1 { get; set; }
        public string Auto2 { get; set; }
        public string Auto3 { get; set; }
        public ItemType ItemType { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<ItemGroupMember> GroupMembers { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<ItemDynamicKitCategoryMember> DynamicKitCategoryMembers { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<ItemPointAccount> PointAccounts { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<ItemSubscription> Subscriptions { get; set; }
    }
    public partial class ItemType 
    {
        public static ItemType CreateItemType(int itemTypeID)
        {
            ItemType itemType = new ItemType();
            itemType.ItemTypeID = itemTypeID;
            return itemType;
        }
        public int ItemTypeID { get; set; }
        public string ItemTypeDescription { get; set; }
    }
    public partial class ItemGroupMember 
    {
        public static ItemGroupMember CreateItemGroupMember(int itemID, int masterItemID, string itemCode, string masterItemCode, int sortOrder)
        {
            ItemGroupMember itemGroupMember = new ItemGroupMember();
            itemGroupMember.ItemID = itemID;
            itemGroupMember.MasterItemID = masterItemID;
            itemGroupMember.ItemCode = itemCode;
            itemGroupMember.MasterItemCode = masterItemCode;
            itemGroupMember.SortOrder = sortOrder;
            return itemGroupMember;
        }
        public int ItemID { get; set; }
        public int MasterItemID { get; set; }
        public string ItemCode { get; set; }
        public string MasterItemCode { get; set; }
        public string MemberDescription { get; set; }
        public int SortOrder { get; set; }
        public Item Item { get; set; }
    }
    public partial class ItemDynamicKitCategoryMember 
    {
        public static ItemDynamicKitCategoryMember CreateItemDynamicKitCategoryMember(int masterItemID, int dynamicKitCategoryID, decimal quantity)
        {
            ItemDynamicKitCategoryMember itemDynamicKitCategoryMember = new ItemDynamicKitCategoryMember();
            itemDynamicKitCategoryMember.MasterItemID = masterItemID;
            itemDynamicKitCategoryMember.DynamicKitCategoryID = dynamicKitCategoryID;
            itemDynamicKitCategoryMember.Quantity = quantity;
            return itemDynamicKitCategoryMember;
        }
        public int MasterItemID { get; set; }
        public int DynamicKitCategoryID { get; set; }
        public decimal Quantity { get; set; }
        public Item MasterItem { get; set; }
        public ItemDynamicKitCategory DynamicKitCategory { get; set; }
    }
    public partial class ItemDynamicKitCategory 
    {
        public static ItemDynamicKitCategory CreateItemDynamicKitCategory(int dynamicKitCategoryID)
        {
            ItemDynamicKitCategory itemDynamicKitCategory = new ItemDynamicKitCategory();
            itemDynamicKitCategory.DynamicKitCategoryID = dynamicKitCategoryID;
            return itemDynamicKitCategory;
        }
        public int DynamicKitCategoryID { get; set; }
        public string DynamicKitCategoryDescription { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<ItemDynamicKitCategoryItemMember> DynamicKitCategoryItemMembers { get; set; }
    }
    public partial class ItemDynamicKitCategoryItemMember 
    {
        public static ItemDynamicKitCategoryItemMember CreateItemDynamicKitCategoryItemMember(int dynamicKitCategoryID, int itemID)
        {
            ItemDynamicKitCategoryItemMember itemDynamicKitCategoryItemMember = new ItemDynamicKitCategoryItemMember();
            itemDynamicKitCategoryItemMember.DynamicKitCategoryID = dynamicKitCategoryID;
            itemDynamicKitCategoryItemMember.ItemID = itemID;
            return itemDynamicKitCategoryItemMember;
        }
        public int DynamicKitCategoryID { get; set; }
        public int ItemID { get; set; }
        public ItemDynamicKitCategory DynamicKitCategory { get; set; }
        public Item Item { get; set; }
    }
    public partial class ItemPointAccount 
    {
        public static ItemPointAccount CreateItemPointAccount(int itemID, int pointAccountID, decimal increment)
        {
            ItemPointAccount itemPointAccount = new ItemPointAccount();
            itemPointAccount.ItemID = itemID;
            itemPointAccount.PointAccountID = pointAccountID;
            itemPointAccount.Increment = increment;
            return itemPointAccount;
        }
        public int ItemID { get; set; }
        public int PointAccountID { get; set; }
        public decimal Increment { get; set; }
        public Item Item { get; set; }
        public PointAccount PointAccount { get; set; }
    }
    public partial class PointAccount 
    {
        public static PointAccount CreatePointAccount(int pointAccountID)
        {
            PointAccount pointAccount = new PointAccount();
            pointAccount.PointAccountID = pointAccountID;
            return pointAccount;
        }
        public int PointAccountID { get; set; }
        public string PointAccountDescription { get; set; }
    }
    public partial class ItemSubscription 
    {
        public static ItemSubscription CreateItemSubscription(int itemID, int subscriptionID, int daysEach)
        {
            ItemSubscription itemSubscription = new ItemSubscription();
            itemSubscription.ItemID = itemID;
            itemSubscription.SubscriptionID = subscriptionID;
            itemSubscription.DaysEach = daysEach;
            return itemSubscription;
        }
        public int ItemID { get; set; }
        public int SubscriptionID { get; set; }
        public int DaysEach { get; set; }
        public Item Item { get; set; }
        public Subscription Subscription { get; set; }
    }
    public partial class Subscription 
    {
        public static Subscription CreateSubscription(int subscriptionID)
        {
            Subscription subscription = new Subscription();
            subscription.SubscriptionID = subscriptionID;
            return subscription;
        }
        public int SubscriptionID { get; set; }
        public string SubscriptionDescription { get; set; }
    }
    public partial class ItemPrice 
    {
        public static ItemPrice CreateItemPrice(
                    int itemID,
                    string currencyCode,
                    int priceTypeID,
                    decimal price,
                    decimal commissionableVolume,
                    decimal businessVolume,
                    decimal other1Price,
                    decimal other2Price,
                    decimal other3Price,
                    decimal other4Price,
                    decimal other5Price,
                    decimal other6Price,
                    decimal other7Price,
                    decimal other8Price,
                    decimal other9Price,
                    decimal other10Price)
        {
            ItemPrice itemPrice = new ItemPrice();
            itemPrice.ItemID = itemID;
            itemPrice.CurrencyCode = currencyCode;
            itemPrice.PriceTypeID = priceTypeID;
            itemPrice.Price = price;
            itemPrice.CommissionableVolume = commissionableVolume;
            itemPrice.BusinessVolume = businessVolume;
            itemPrice.Other1Price = other1Price;
            itemPrice.Other2Price = other2Price;
            itemPrice.Other3Price = other3Price;
            itemPrice.Other4Price = other4Price;
            itemPrice.Other5Price = other5Price;
            itemPrice.Other6Price = other6Price;
            itemPrice.Other7Price = other7Price;
            itemPrice.Other8Price = other8Price;
            itemPrice.Other9Price = other9Price;
            itemPrice.Other10Price = other10Price;
            return itemPrice;
        }
        public int ItemID { get; set; }
        public string CurrencyCode { get; set; }
        public int PriceTypeID { get; set; }
        public decimal Price { get; set; }
        public decimal CommissionableVolume { get; set; }
        public decimal BusinessVolume { get; set; }
        public decimal Other1Price { get; set; }
        public decimal Other2Price { get; set; }
        public decimal Other3Price { get; set; }
        public decimal Other4Price { get; set; }
        public decimal Other5Price { get; set; }
        public decimal Other6Price { get; set; }
        public decimal Other7Price { get; set; }
        public decimal Other8Price { get; set; }
        public decimal Other9Price { get; set; }
        public decimal Other10Price { get; set; }
        public Item Item { get; set; }
    }
    public partial class ItemWarehousePrice 
    {
        /// <summary>
        /// Create a new ItemWarehousePrice object.
        /// </summary>
        /// <param name="itemID">Initial value of ItemID.</param>
        /// <param name="warehouseID">Initial value of WarehouseID.</param>
        /// <param name="currencyCode">Initial value of CurrencyCode.</param>
        /// <param name="priceTypeID">Initial value of PriceTypeID.</param>
        /// <param name="price">Initial value of Price.</param>
        /// <param name="commissionableVolume">Initial value of CommissionableVolume.</param>
        /// <param name="businessVolume">Initial value of BusinessVolume.</param>
        /// <param name="other1Price">Initial value of Other1Price.</param>
        /// <param name="other2Price">Initial value of Other2Price.</param>
        /// <param name="other3Price">Initial value of Other3Price.</param>
        /// <param name="other4Price">Initial value of Other4Price.</param>
        /// <param name="other5Price">Initial value of Other5Price.</param>
        /// <param name="other6Price">Initial value of Other6Price.</param>
        /// <param name="other7Price">Initial value of Other7Price.</param>
        /// <param name="other8Price">Initial value of Other8Price.</param>
        /// <param name="other9Price">Initial value of Other9Price.</param>
        /// <param name="other10Price">Initial value of Other10Price.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static ItemWarehousePrice CreateItemWarehousePrice(
                    int itemID,
                    int warehouseID,
                    string currencyCode,
                    int priceTypeID,
                    decimal price,
                    decimal commissionableVolume,
                    decimal businessVolume,
                    decimal other1Price,
                    decimal other2Price,
                    decimal other3Price,
                    decimal other4Price,
                    decimal other5Price,
                    decimal other6Price,
                    decimal other7Price,
                    decimal other8Price,
                    decimal other9Price,
                    decimal other10Price)
        {
            ItemWarehousePrice itemWarehousePrice = new ItemWarehousePrice();
            itemWarehousePrice.ItemID = itemID;
            itemWarehousePrice.WarehouseID = warehouseID;
            itemWarehousePrice.CurrencyCode = currencyCode;
            itemWarehousePrice.PriceTypeID = priceTypeID;
            itemWarehousePrice.Price = price;
            itemWarehousePrice.CommissionableVolume = commissionableVolume;
            itemWarehousePrice.BusinessVolume = businessVolume;
            itemWarehousePrice.Other1Price = other1Price;
            itemWarehousePrice.Other2Price = other2Price;
            itemWarehousePrice.Other3Price = other3Price;
            itemWarehousePrice.Other4Price = other4Price;
            itemWarehousePrice.Other5Price = other5Price;
            itemWarehousePrice.Other6Price = other6Price;
            itemWarehousePrice.Other7Price = other7Price;
            itemWarehousePrice.Other8Price = other8Price;
            itemWarehousePrice.Other9Price = other9Price;
            itemWarehousePrice.Other10Price = other10Price;
            return itemWarehousePrice;
        }
        public int ItemID { get; set; }
        public int WarehouseID { get; set; }
        public string CurrencyCode { get; set; }
        public int PriceTypeID { get; set; }
        public decimal Price { get; set; }
        public decimal CommissionableVolume { get; set; }
        public decimal BusinessVolume { get; set; }
        public decimal Other1Price { get; set; }
        public decimal Other2Price { get; set; }
        public decimal Other3Price { get; set; }
        public decimal Other4Price { get; set; }
        public decimal Other5Price { get; set; }
        public decimal Other6Price { get; set; }
        public decimal Other7Price { get; set; }
        public decimal Other8Price { get; set; }
        public decimal Other9Price { get; set; }
        public decimal Other10Price { get; set; }
        public Item Item { get; set; }
    }
    public partial class ItemLanguage 
    {
        public static ItemLanguage CreateItemLanguage(int itemID, int languageID)
        {
            ItemLanguage itemLanguage = new ItemLanguage();
            itemLanguage.ItemID = itemID;
            itemLanguage.LanguageID = languageID;
            return itemLanguage;
        }
        public int ItemID { get; set; }
        public int LanguageID { get; set; }
        public string ItemDescription { get; set; }
        public string ShortDetail { get; set; }
        public string ShortDetail2 { get; set; }
        public string ShortDetail3 { get; set; }
        public string ShortDetail4 { get; set; }
        public string LongDetail { get; set; }
        public string LongDetail2 { get; set; }
        public string LongDetail3 { get; set; }
        public string LongDetail4 { get; set; }
        public Item Item { get; set; }
        public Language Language { get; set; }
    }
    public partial class ItemStaticKitMember 
    {
        public static ItemStaticKitMember CreateItemStaticKitMember(int masterItemID, int itemID, decimal quantity)
        {
            ItemStaticKitMember itemStaticKitMember = new ItemStaticKitMember();
            itemStaticKitMember.MasterItemID = masterItemID;
            itemStaticKitMember.ItemID = itemID;
            itemStaticKitMember.Quantity = quantity;
            return itemStaticKitMember;
        }
        public int MasterItemID { get; set; }
        public int ItemID { get; set; }
        public decimal Quantity { get; set; }
        public Item Item { get; set; }
    }
    public partial class ItemWarehouse 
    {
        public static ItemWarehouse CreateItemWarehouse(int itemID, int warehouseID, int maxAllowedOnOrder)
        {
            ItemWarehouse itemWarehouse = new ItemWarehouse();
            itemWarehouse.ItemID = itemID;
            itemWarehouse.WarehouseID = warehouseID;
            itemWarehouse.MaxAllowedOnOrder = maxAllowedOnOrder;
            return itemWarehouse;
        }
        public int ItemID { get; set; }
        public int WarehouseID { get; set; }
        public int MaxAllowedOnOrder { get; set; }
        public Item Item { get; set; }
        public Warehouse Warehouse { get; set; }
    }
    public partial class EnrollerNode 
    {
        public static EnrollerNode CreateEnrollerNode(int topCustomerID, int customerID, int enrollerID, int level, int indentedSort, int childCount)
        {
            EnrollerNode enrollerNode = new EnrollerNode();
            enrollerNode.TopCustomerID = topCustomerID;
            enrollerNode.CustomerID = customerID;
            enrollerNode.EnrollerID = enrollerID;
            enrollerNode.Level = level;
            enrollerNode.IndentedSort = indentedSort;
            enrollerNode.ChildCount = childCount;
            return enrollerNode;
        }
        public int TopCustomerID { get; set; }
        public int CustomerID { get; set; }
        public int EnrollerID { get; set; }
        public int Level { get; set; }
        public int IndentedSort { get; set; }
        public int ChildCount { get; set; }
        public Customer Customer { get; set; }
    }
    public partial class EnrollerNodePeriodVolume 
    {
        public static EnrollerNodePeriodVolume CreateEnrollerNodePeriodVolume(int topCustomerID, int periodTypeID, int periodID, int customerID, int enrollerID, int level, int indentedSort, int childCount)
        {
            EnrollerNodePeriodVolume enrollerNodePeriodVolume = new EnrollerNodePeriodVolume();
            enrollerNodePeriodVolume.TopCustomerID = topCustomerID;
            enrollerNodePeriodVolume.PeriodTypeID = periodTypeID;
            enrollerNodePeriodVolume.PeriodID = periodID;
            enrollerNodePeriodVolume.CustomerID = customerID;
            enrollerNodePeriodVolume.EnrollerID = enrollerID;
            enrollerNodePeriodVolume.Level = level;
            enrollerNodePeriodVolume.IndentedSort = indentedSort;
            enrollerNodePeriodVolume.ChildCount = childCount;
            return enrollerNodePeriodVolume;
        }
        public int TopCustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public int PeriodID { get; set; }
        public int CustomerID { get; set; }
        public int EnrollerID { get; set; }
        public int Level { get; set; }
        public int IndentedSort { get; set; }
        public int ChildCount { get; set; }
        public Customer Customer { get; set; }
        public PeriodVolume PeriodVolume { get; set; }
        public Period Period { get; set; }
    }
    public partial class PeriodVolume 
    {
        public static PeriodVolume CreatePeriodVolume(
                    int periodTypeID,
                    int periodID,
                    int customerID,
                    int rankID,
                    int paidRankID,
                    decimal volume1,
                    decimal volume2,
                    decimal volume3,
                    decimal volume4,
                    decimal volume5,
                    decimal volume6,
                    decimal volume7,
                    decimal volume8,
                    decimal volume9,
                    decimal volume10,
                    decimal volume11,
                    decimal volume12,
                    decimal volume13,
                    decimal volume14,
                    decimal volume15,
                    decimal volume16,
                    decimal volume17,
                    decimal volume18,
                    decimal volume19,
                    decimal volume20,
                    decimal volume21,
                    decimal volume22,
                    decimal volume23,
                    decimal volume24,
                    decimal volume25,
                    decimal volume26,
                    decimal volume27,
                    decimal volume28,
                    decimal volume29,
                    decimal volume30,
                    decimal volume31,
                    decimal volume32,
                    decimal volume33,
                    decimal volume34,
                    decimal volume35,
                    decimal volume36,
                    decimal volume37,
                    decimal volume38,
                    decimal volume39,
                    decimal volume40,
                    decimal volume41,
                    decimal volume42,
                    decimal volume43,
                    decimal volume44,
                    decimal volume45,
                    decimal volume46,
                    decimal volume47,
                    decimal volume48,
                    decimal volume49,
                    decimal volume50,
                    decimal volume51,
                    decimal volume52,
                    decimal volume53,
                    decimal volume54,
                    decimal volume55,
                    decimal volume56,
                    decimal volume57,
                    decimal volume58,
                    decimal volume59,
                    decimal volume60,
                    decimal volume61,
                    decimal volume62,
                    decimal volume63,
                    decimal volume64,
                    decimal volume65,
                    decimal volume66,
                    decimal volume67,
                    decimal volume68,
                    decimal volume69,
                    decimal volume70,
                    decimal volume71,
                    decimal volume72,
                    decimal volume73,
                    decimal volume74,
                    decimal volume75,
                    decimal volume76,
                    decimal volume77,
                    decimal volume78,
                    decimal volume79,
                    decimal volume80,
                    decimal volume81,
                    decimal volume82,
                    decimal volume83,
                    decimal volume84,
                    decimal volume85,
                    decimal volume86,
                    decimal volume87,
                    decimal volume88,
                    decimal volume89,
                    decimal volume90,
                    decimal volume91,
                    decimal volume92,
                    decimal volume93,
                    decimal volume94,
                    decimal volume95,
                    decimal volume96,
                    decimal volume97,
                    decimal volume98,
                    decimal volume99,
                    decimal volume100,
                    decimal volume101,
                    decimal volume102,
                    decimal volume103,
                    decimal volume104,
                    decimal volume105,
                    decimal volume106,
                    decimal volume107,
                    decimal volume108,
                    decimal volume109,
                    decimal volume110,
                    decimal volume111,
                    decimal volume112,
                    decimal volume113,
                    decimal volume114,
                    decimal volume115,
                    decimal volume116,
                    decimal volume117,
                    decimal volume118,
                    decimal volume119,
                    decimal volume120,
                    decimal volume121,
                    decimal volume122,
                    decimal volume123,
                    decimal volume124,
                    decimal volume125,
                    decimal volume126,
                    decimal volume127,
                    decimal volume128,
                    decimal volume129,
                    decimal volume130,
                    decimal volume131,
                    decimal volume132,
                    decimal volume133,
                    decimal volume134,
                    decimal volume135,
                    decimal volume136,
                    decimal volume137,
                    decimal volume138,
                    decimal volume139,
                    decimal volume140,
                    decimal volume141,
                    decimal volume142,
                    decimal volume143,
                    decimal volume144,
                    decimal volume145,
                    decimal volume146,
                    decimal volume147,
                    decimal volume148,
                    decimal volume149,
                    decimal volume150,
                    decimal volume151,
                    decimal volume152,
                    decimal volume153,
                    decimal volume154,
                    decimal volume155,
                    decimal volume156,
                    decimal volume157,
                    decimal volume158,
                    decimal volume159,
                    decimal volume160,
                    decimal volume161,
                    decimal volume162,
                    decimal volume163,
                    decimal volume164,
                    decimal volume165,
                    decimal volume166,
                    decimal volume167,
                    decimal volume168,
                    decimal volume169,
                    decimal volume170,
                    decimal volume171,
                    decimal volume172,
                    decimal volume173,
                    decimal volume174,
                    decimal volume175,
                    decimal volume176,
                    decimal volume177,
                    decimal volume178,
                    decimal volume179,
                    decimal volume180,
                    decimal volume181,
                    decimal volume182,
                    decimal volume183,
                    decimal volume184,
                    decimal volume185,
                    decimal volume186,
                    decimal volume187,
                    decimal volume188,
                    decimal volume189,
                    decimal volume190,
                    decimal volume191,
                    decimal volume192,
                    decimal volume193,
                    decimal volume194,
                    decimal volume195,
                    decimal volume196,
                    decimal volume197,
                    decimal volume198,
                    decimal volume199,
                    decimal volume200,
                    global::System.DateTime modifiedDate)
        {
            PeriodVolume periodVolume = new PeriodVolume();
            periodVolume.PeriodTypeID = periodTypeID;
            periodVolume.PeriodID = periodID;
            periodVolume.CustomerID = customerID;
            periodVolume.RankID = rankID;
            periodVolume.PaidRankID = paidRankID;
            periodVolume.Volume1 = volume1;
            periodVolume.Volume2 = volume2;
            periodVolume.Volume3 = volume3;
            periodVolume.Volume4 = volume4;
            periodVolume.Volume5 = volume5;
            periodVolume.Volume6 = volume6;
            periodVolume.Volume7 = volume7;
            periodVolume.Volume8 = volume8;
            periodVolume.Volume9 = volume9;
            periodVolume.Volume10 = volume10;
            periodVolume.Volume11 = volume11;
            periodVolume.Volume12 = volume12;
            periodVolume.Volume13 = volume13;
            periodVolume.Volume14 = volume14;
            periodVolume.Volume15 = volume15;
            periodVolume.Volume16 = volume16;
            periodVolume.Volume17 = volume17;
            periodVolume.Volume18 = volume18;
            periodVolume.Volume19 = volume19;
            periodVolume.Volume20 = volume20;
            periodVolume.Volume21 = volume21;
            periodVolume.Volume22 = volume22;
            periodVolume.Volume23 = volume23;
            periodVolume.Volume24 = volume24;
            periodVolume.Volume25 = volume25;
            periodVolume.Volume26 = volume26;
            periodVolume.Volume27 = volume27;
            periodVolume.Volume28 = volume28;
            periodVolume.Volume29 = volume29;
            periodVolume.Volume30 = volume30;
            periodVolume.Volume31 = volume31;
            periodVolume.Volume32 = volume32;
            periodVolume.Volume33 = volume33;
            periodVolume.Volume34 = volume34;
            periodVolume.Volume35 = volume35;
            periodVolume.Volume36 = volume36;
            periodVolume.Volume37 = volume37;
            periodVolume.Volume38 = volume38;
            periodVolume.Volume39 = volume39;
            periodVolume.Volume40 = volume40;
            periodVolume.Volume41 = volume41;
            periodVolume.Volume42 = volume42;
            periodVolume.Volume43 = volume43;
            periodVolume.Volume44 = volume44;
            periodVolume.Volume45 = volume45;
            periodVolume.Volume46 = volume46;
            periodVolume.Volume47 = volume47;
            periodVolume.Volume48 = volume48;
            periodVolume.Volume49 = volume49;
            periodVolume.Volume50 = volume50;
            periodVolume.Volume51 = volume51;
            periodVolume.Volume52 = volume52;
            periodVolume.Volume53 = volume53;
            periodVolume.Volume54 = volume54;
            periodVolume.Volume55 = volume55;
            periodVolume.Volume56 = volume56;
            periodVolume.Volume57 = volume57;
            periodVolume.Volume58 = volume58;
            periodVolume.Volume59 = volume59;
            periodVolume.Volume60 = volume60;
            periodVolume.Volume61 = volume61;
            periodVolume.Volume62 = volume62;
            periodVolume.Volume63 = volume63;
            periodVolume.Volume64 = volume64;
            periodVolume.Volume65 = volume65;
            periodVolume.Volume66 = volume66;
            periodVolume.Volume67 = volume67;
            periodVolume.Volume68 = volume68;
            periodVolume.Volume69 = volume69;
            periodVolume.Volume70 = volume70;
            periodVolume.Volume71 = volume71;
            periodVolume.Volume72 = volume72;
            periodVolume.Volume73 = volume73;
            periodVolume.Volume74 = volume74;
            periodVolume.Volume75 = volume75;
            periodVolume.Volume76 = volume76;
            periodVolume.Volume77 = volume77;
            periodVolume.Volume78 = volume78;
            periodVolume.Volume79 = volume79;
            periodVolume.Volume80 = volume80;
            periodVolume.Volume81 = volume81;
            periodVolume.Volume82 = volume82;
            periodVolume.Volume83 = volume83;
            periodVolume.Volume84 = volume84;
            periodVolume.Volume85 = volume85;
            periodVolume.Volume86 = volume86;
            periodVolume.Volume87 = volume87;
            periodVolume.Volume88 = volume88;
            periodVolume.Volume89 = volume89;
            periodVolume.Volume90 = volume90;
            periodVolume.Volume91 = volume91;
            periodVolume.Volume92 = volume92;
            periodVolume.Volume93 = volume93;
            periodVolume.Volume94 = volume94;
            periodVolume.Volume95 = volume95;
            periodVolume.Volume96 = volume96;
            periodVolume.Volume97 = volume97;
            periodVolume.Volume98 = volume98;
            periodVolume.Volume99 = volume99;
            periodVolume.Volume100 = volume100;
            periodVolume.Volume101 = volume101;
            periodVolume.Volume102 = volume102;
            periodVolume.Volume103 = volume103;
            periodVolume.Volume104 = volume104;
            periodVolume.Volume105 = volume105;
            periodVolume.Volume106 = volume106;
            periodVolume.Volume107 = volume107;
            periodVolume.Volume108 = volume108;
            periodVolume.Volume109 = volume109;
            periodVolume.Volume110 = volume110;
            periodVolume.Volume111 = volume111;
            periodVolume.Volume112 = volume112;
            periodVolume.Volume113 = volume113;
            periodVolume.Volume114 = volume114;
            periodVolume.Volume115 = volume115;
            periodVolume.Volume116 = volume116;
            periodVolume.Volume117 = volume117;
            periodVolume.Volume118 = volume118;
            periodVolume.Volume119 = volume119;
            periodVolume.Volume120 = volume120;
            periodVolume.Volume121 = volume121;
            periodVolume.Volume122 = volume122;
            periodVolume.Volume123 = volume123;
            periodVolume.Volume124 = volume124;
            periodVolume.Volume125 = volume125;
            periodVolume.Volume126 = volume126;
            periodVolume.Volume127 = volume127;
            periodVolume.Volume128 = volume128;
            periodVolume.Volume129 = volume129;
            periodVolume.Volume130 = volume130;
            periodVolume.Volume131 = volume131;
            periodVolume.Volume132 = volume132;
            periodVolume.Volume133 = volume133;
            periodVolume.Volume134 = volume134;
            periodVolume.Volume135 = volume135;
            periodVolume.Volume136 = volume136;
            periodVolume.Volume137 = volume137;
            periodVolume.Volume138 = volume138;
            periodVolume.Volume139 = volume139;
            periodVolume.Volume140 = volume140;
            periodVolume.Volume141 = volume141;
            periodVolume.Volume142 = volume142;
            periodVolume.Volume143 = volume143;
            periodVolume.Volume144 = volume144;
            periodVolume.Volume145 = volume145;
            periodVolume.Volume146 = volume146;
            periodVolume.Volume147 = volume147;
            periodVolume.Volume148 = volume148;
            periodVolume.Volume149 = volume149;
            periodVolume.Volume150 = volume150;
            periodVolume.Volume151 = volume151;
            periodVolume.Volume152 = volume152;
            periodVolume.Volume153 = volume153;
            periodVolume.Volume154 = volume154;
            periodVolume.Volume155 = volume155;
            periodVolume.Volume156 = volume156;
            periodVolume.Volume157 = volume157;
            periodVolume.Volume158 = volume158;
            periodVolume.Volume159 = volume159;
            periodVolume.Volume160 = volume160;
            periodVolume.Volume161 = volume161;
            periodVolume.Volume162 = volume162;
            periodVolume.Volume163 = volume163;
            periodVolume.Volume164 = volume164;
            periodVolume.Volume165 = volume165;
            periodVolume.Volume166 = volume166;
            periodVolume.Volume167 = volume167;
            periodVolume.Volume168 = volume168;
            periodVolume.Volume169 = volume169;
            periodVolume.Volume170 = volume170;
            periodVolume.Volume171 = volume171;
            periodVolume.Volume172 = volume172;
            periodVolume.Volume173 = volume173;
            periodVolume.Volume174 = volume174;
            periodVolume.Volume175 = volume175;
            periodVolume.Volume176 = volume176;
            periodVolume.Volume177 = volume177;
            periodVolume.Volume178 = volume178;
            periodVolume.Volume179 = volume179;
            periodVolume.Volume180 = volume180;
            periodVolume.Volume181 = volume181;
            periodVolume.Volume182 = volume182;
            periodVolume.Volume183 = volume183;
            periodVolume.Volume184 = volume184;
            periodVolume.Volume185 = volume185;
            periodVolume.Volume186 = volume186;
            periodVolume.Volume187 = volume187;
            periodVolume.Volume188 = volume188;
            periodVolume.Volume189 = volume189;
            periodVolume.Volume190 = volume190;
            periodVolume.Volume191 = volume191;
            periodVolume.Volume192 = volume192;
            periodVolume.Volume193 = volume193;
            periodVolume.Volume194 = volume194;
            periodVolume.Volume195 = volume195;
            periodVolume.Volume196 = volume196;
            periodVolume.Volume197 = volume197;
            periodVolume.Volume198 = volume198;
            periodVolume.Volume199 = volume199;
            periodVolume.Volume200 = volume200;
            periodVolume.ModifiedDate = modifiedDate;
            return periodVolume;
        }
        public int PeriodTypeID { get; set; }
        public int PeriodID { get; set; }
        public int CustomerID { get; set; }
        public int RankID { get; set; }
        public int PaidRankID { get; set; }
        public decimal Volume1 { get; set; }
        public decimal Volume2 { get; set; }
        public decimal Volume3 { get; set; }
        public decimal Volume4 { get; set; }
        public decimal Volume5 { get; set; }
        public decimal Volume6 { get; set; }
        public decimal Volume7 { get; set; }
        public decimal Volume8 { get; set; }
        public decimal Volume9 { get; set; }
        public decimal Volume10 { get; set; }
        public decimal Volume11 { get; set; }
        public decimal Volume12 { get; set; }
        public decimal Volume13 { get; set; }
        public decimal Volume14 { get; set; }
        public decimal Volume15 { get; set; }
        public decimal Volume16 { get; set; }
        public decimal Volume17 { get; set; }
        public decimal Volume18 { get; set; }
        public decimal Volume19 { get; set; }
        public decimal Volume20 { get; set; }
        public decimal Volume21 { get; set; }
        public decimal Volume22 { get; set; }
        public decimal Volume23 { get; set; }
        public decimal Volume24 { get; set; }
        public decimal Volume25 { get; set; }
        public decimal Volume26 { get; set; }
        public decimal Volume27 { get; set; }
        public decimal Volume28 { get; set; }
        public decimal Volume29 { get; set; }
        public decimal Volume30 { get; set; }
        public decimal Volume31 { get; set; }
        public decimal Volume32 { get; set; }
        public decimal Volume33 { get; set; }
        public decimal Volume34 { get; set; }
        public decimal Volume35 { get; set; }
        public decimal Volume36 { get; set; }
        public decimal Volume37 { get; set; }
        public decimal Volume38 { get; set; }
        public decimal Volume39 { get; set; }
        public decimal Volume40 { get; set; }
        public decimal Volume41 { get; set; }
        public decimal Volume42 { get; set; }
        public decimal Volume43 { get; set; }
        public decimal Volume44 { get; set; }
        public decimal Volume45 { get; set; }
        public decimal Volume46 { get; set; }
        public decimal Volume47 { get; set; }
        public decimal Volume48 { get; set; }
        public decimal Volume49 { get; set; }
        public decimal Volume50 { get; set; }
        public decimal Volume51 { get; set; }
        public decimal Volume52 { get; set; }
        public decimal Volume53 { get; set; }
        public decimal Volume54 { get; set; }
        public decimal Volume55 { get; set; }
        public decimal Volume56 { get; set; }
        public decimal Volume57 { get; set; }
        public decimal Volume58 { get; set; }
        public decimal Volume59 { get; set; }
        public decimal Volume60 { get; set; }
        public decimal Volume61 { get; set; }
        public decimal Volume62 { get; set; }
        public decimal Volume63 { get; set; }
        public decimal Volume64 { get; set; }
        public decimal Volume65 { get; set; }
        public decimal Volume66 { get; set; }
        public decimal Volume67 { get; set; }
        public decimal Volume68 { get; set; }
        public decimal Volume69 { get; set; }
        public decimal Volume70 { get; set; }
        public decimal Volume71 { get; set; }
        public decimal Volume72 { get; set; }
        public decimal Volume73 { get; set; }
        public decimal Volume74 { get; set; }
        public decimal Volume75 { get; set; }
        public decimal Volume76 { get; set; }
        public decimal Volume77 { get; set; }
        public decimal Volume78 { get; set; }
        public decimal Volume79 { get; set; }
        public decimal Volume80 { get; set; }
        public decimal Volume81 { get; set; }
        public decimal Volume82 { get; set; }
        public decimal Volume83 { get; set; }
        public decimal Volume84 { get; set; }
        public decimal Volume85 { get; set; }
        public decimal Volume86 { get; set; }
        public decimal Volume87 { get; set; }
        public decimal Volume88 { get; set; }
        public decimal Volume89 { get; set; }
        public decimal Volume90 { get; set; }
        public decimal Volume91 { get; set; }
        public decimal Volume92 { get; set; }
        public decimal Volume93 { get; set; }
        public decimal Volume94 { get; set; }
        public decimal Volume95 { get; set; }
        public decimal Volume96 { get; set; }
        public decimal Volume97 { get; set; }
        public decimal Volume98 { get; set; }
        public decimal Volume99 { get; set; }
        public decimal Volume100 { get; set; }
        public decimal Volume101 { get; set; }
        public decimal Volume102 { get; set; }
        public decimal Volume103 { get; set; }
        public decimal Volume104 { get; set; }
        public decimal Volume105 { get; set; }
        public decimal Volume106 { get; set; }
        public decimal Volume107 { get; set; }
        public decimal Volume108 { get; set; }
        public decimal Volume109 { get; set; }
        public decimal Volume110 { get; set; }
        public decimal Volume111 { get; set; }
        public decimal Volume112 { get; set; }
        public decimal Volume113 { get; set; }
        public decimal Volume114 { get; set; }
        public decimal Volume115 { get; set; }
        public decimal Volume116 { get; set; }
        public decimal Volume117 { get; set; }
        public decimal Volume118 { get; set; }
        public decimal Volume119 { get; set; }
        public decimal Volume120 { get; set; }
        public decimal Volume121 { get; set; }
        public decimal Volume122 { get; set; }
        public decimal Volume123 { get; set; }
        public decimal Volume124 { get; set; }
        public decimal Volume125 { get; set; }
        public decimal Volume126 { get; set; }
        public decimal Volume127 { get; set; }
        public decimal Volume128 { get; set; }
        public decimal Volume129 { get; set; }
        public decimal Volume130 { get; set; }
        public decimal Volume131 { get; set; }
        public decimal Volume132 { get; set; }
        public decimal Volume133 { get; set; }
        public decimal Volume134 { get; set; }
        public decimal Volume135 { get; set; }
        public decimal Volume136 { get; set; }
        public decimal Volume137 { get; set; }
        public decimal Volume138 { get; set; }
        public decimal Volume139 { get; set; }
        public decimal Volume140 { get; set; }
        public decimal Volume141 { get; set; }
        public decimal Volume142 { get; set; }
        public decimal Volume143 { get; set; }
        public decimal Volume144 { get; set; }
        public decimal Volume145 { get; set; }
        public decimal Volume146 { get; set; }
        public decimal Volume147 { get; set; }
        public decimal Volume148 { get; set; }
        public decimal Volume149 { get; set; }
        public decimal Volume150 { get; set; }
        public decimal Volume151 { get; set; }
        public decimal Volume152 { get; set; }
        public decimal Volume153 { get; set; }
        public decimal Volume154 { get; set; }
        public decimal Volume155 { get; set; }
        public decimal Volume156 { get; set; }
        public decimal Volume157 { get; set; }
        public decimal Volume158 { get; set; }
        public decimal Volume159 { get; set; }
        public decimal Volume160 { get; set; }
        public decimal Volume161 { get; set; }
        public decimal Volume162 { get; set; }
        public decimal Volume163 { get; set; }
        public decimal Volume164 { get; set; }
        public decimal Volume165 { get; set; }
        public decimal Volume166 { get; set; }
        public decimal Volume167 { get; set; }
        public decimal Volume168 { get; set; }
        public decimal Volume169 { get; set; }
        public decimal Volume170 { get; set; }
        public decimal Volume171 { get; set; }
        public decimal Volume172 { get; set; }
        public decimal Volume173 { get; set; }
        public decimal Volume174 { get; set; }
        public decimal Volume175 { get; set; }
        public decimal Volume176 { get; set; }
        public decimal Volume177 { get; set; }
        public decimal Volume178 { get; set; }
        public decimal Volume179 { get; set; }
        public decimal Volume180 { get; set; }
        public decimal Volume181 { get; set; }
        public decimal Volume182 { get; set; }
        public decimal Volume183 { get; set; }
        public decimal Volume184 { get; set; }
        public decimal Volume185 { get; set; }
        public decimal Volume186 { get; set; }
        public decimal Volume187 { get; set; }
        public decimal Volume188 { get; set; }
        public decimal Volume189 { get; set; }
        public decimal Volume190 { get; set; }
        public decimal Volume191 { get; set; }
        public decimal Volume192 { get; set; }
        public decimal Volume193 { get; set; }
        public decimal Volume194 { get; set; }
        public decimal Volume195 { get; set; }
        public decimal Volume196 { get; set; }
        public decimal Volume197 { get; set; }
        public decimal Volume198 { get; set; }
        public decimal Volume199 { get; set; }
        public decimal Volume200 { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public Customer Customer { get; set; }
        public Rank Rank { get; set; }
        public Rank PaidRank { get; set; }
        public Period Period { get; set; }
    }
    public partial class Period 
    {
        public static Period CreatePeriod(int periodTypeID, int periodID, bool isCurrentPeriod, global::System.DateTime startDate, global::System.DateTime endDate)
        {
            Period period = new Period();
            period.PeriodTypeID = periodTypeID;
            period.PeriodID = periodID;
            period.IsCurrentPeriod = isCurrentPeriod;
            period.StartDate = startDate;
            period.EndDate = endDate;
            return period;
        }
        public int PeriodTypeID { get; set; }
        public int PeriodID { get; set; }
        public string PeriodDescription { get; set; }
        public bool IsCurrentPeriod { get; set; }
        public global::System.DateTime StartDate { get; set; }
        public global::System.DateTime EndDate { get; set; }
        public global::System.Nullable<global::System.DateTime> AcceptedDate { get; set; }
        public PeriodType PeriodType { get; set; }
    }
    public partial class PeriodType 
    {
        public static PeriodType CreatePeriodType(int periodTypeID)
        {
            PeriodType periodType = new PeriodType();
            periodType.PeriodTypeID = periodTypeID;
            return periodType;
        }
        public int PeriodTypeID { get; set; }
        public string PeriodTypeDescription { get; set; }
    }
    public partial class UniLevelNode 
    {
        public static UniLevelNode CreateUniLevelNode(int topCustomerID, int customerID, int sponsorID, int placement, int level, int indentedSort, int childCount)
        {
            UniLevelNode uniLevelNode = new UniLevelNode();
            uniLevelNode.TopCustomerID = topCustomerID;
            uniLevelNode.CustomerID = customerID;
            uniLevelNode.SponsorID = sponsorID;
            uniLevelNode.Placement = placement;
            uniLevelNode.Level = level;
            uniLevelNode.IndentedSort = indentedSort;
            uniLevelNode.ChildCount = childCount;
            return uniLevelNode;
        }
        public int TopCustomerID { get; set; }
        public int CustomerID { get; set; }
        public int SponsorID { get; set; }
        public int Placement { get; set; }
        public int Level { get; set; }
        public int IndentedSort { get; set; }
        public int ChildCount { get; set; }
        public Customer Customer { get; set; }
    }
    public partial class UniLevelNodePeriodVolume 
    {
        public static UniLevelNodePeriodVolume CreateUniLevelNodePeriodVolume(int topCustomerID, int periodTypeID, int periodID, int customerID, int sponsorID, int placement, int level, int indentedSort, int childCount)
        {
            UniLevelNodePeriodVolume uniLevelNodePeriodVolume = new UniLevelNodePeriodVolume();
            uniLevelNodePeriodVolume.TopCustomerID = topCustomerID;
            uniLevelNodePeriodVolume.PeriodTypeID = periodTypeID;
            uniLevelNodePeriodVolume.PeriodID = periodID;
            uniLevelNodePeriodVolume.CustomerID = customerID;
            uniLevelNodePeriodVolume.SponsorID = sponsorID;
            uniLevelNodePeriodVolume.Placement = placement;
            uniLevelNodePeriodVolume.Level = level;
            uniLevelNodePeriodVolume.IndentedSort = indentedSort;
            uniLevelNodePeriodVolume.ChildCount = childCount;
            return uniLevelNodePeriodVolume;
        }
        public int TopCustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public int PeriodID { get; set; }
        public int CustomerID { get; set; }
        public int SponsorID { get; set; }
        public int Placement { get; set; }
        public int Level { get; set; }
        public int IndentedSort { get; set; }
        public int ChildCount { get; set; }
        public Customer Customer { get; set; }
        public PeriodVolume PeriodVolume { get; set; }
        public Period Period { get; set; }
    }
    public partial class BinaryNode 
    {
        public static BinaryNode CreateBinaryNode(int topCustomerID, int customerID, int parentID, int placement, int level, int indentedSort, int childCount)
        {
            BinaryNode binaryNode = new BinaryNode();
            binaryNode.TopCustomerID = topCustomerID;
            binaryNode.CustomerID = customerID;
            binaryNode.ParentID = parentID;
            binaryNode.Placement = placement;
            binaryNode.Level = level;
            binaryNode.IndentedSort = indentedSort;
            binaryNode.ChildCount = childCount;
            return binaryNode;
        }
        public int TopCustomerID { get; set; }
        public int CustomerID { get; set; }
        public int ParentID { get; set; }
        public int Placement { get; set; }
        public int Level { get; set; }
        public int IndentedSort { get; set; }
        public int ChildCount { get; set; }
        public Customer Customer { get; set; }
    }
    public partial class BinaryNodePeriodVolume 
    {
        public static BinaryNodePeriodVolume CreateBinaryNodePeriodVolume(int topCustomerID, int periodTypeID, int periodID, int customerID, int parentID, int placement, int level, int indentedSort, int childCount)
        {
            BinaryNodePeriodVolume binaryNodePeriodVolume = new BinaryNodePeriodVolume();
            binaryNodePeriodVolume.TopCustomerID = topCustomerID;
            binaryNodePeriodVolume.PeriodTypeID = periodTypeID;
            binaryNodePeriodVolume.PeriodID = periodID;
            binaryNodePeriodVolume.CustomerID = customerID;
            binaryNodePeriodVolume.ParentID = parentID;
            binaryNodePeriodVolume.Placement = placement;
            binaryNodePeriodVolume.Level = level;
            binaryNodePeriodVolume.IndentedSort = indentedSort;
            binaryNodePeriodVolume.ChildCount = childCount;
            return binaryNodePeriodVolume;
        }
        public int TopCustomerID { get; set; }
        public int PeriodTypeID { get; set; }
        public int PeriodID { get; set; }
        public int CustomerID { get; set; }
        public int ParentID { get; set; }
        public int Placement { get; set; }
        public int Level { get; set; }
        public int IndentedSort { get; set; }
        public int ChildCount { get; set; }
        public Customer Customer { get; set; }
        public PeriodVolume PeriodVolume { get; set; }
        public Period Period { get; set; }
    }
    public partial class CustomerContact 
    {
        public static CustomerContact CreateCustomerContact(int customerID, int customerEntityID)
        {
            CustomerContact customerContact = new CustomerContact();
            customerContact.CustomerID = customerID;
            customerContact.CustomerEntityID = customerEntityID;
            return customerContact;
        }
        public int CustomerID { get; set; }
        public int CustomerEntityID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string BusinessPhone { get; set; }
        public string HomePhone { get; set; }
        public string Fax { get; set; }
        public string Mobile { get; set; }
        public string LinkedIn { get; set; }
        public string Facebook { get; set; }
        public string Blog { get; set; }
        public string MySpace { get; set; }
        public string GooglePlus { get; set; }
        public string Twitter { get; set; }
        public global::System.Nullable<global::System.DateTime> Birthday { get; set; }
        public string Notes { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
    }
    public partial class CustomerAccount 
    {
        public static CustomerAccount CreateCustomerAccount(int customerID, int primaryExpirationMonth, int primaryExpirationYear, int primaryCreditCardTypeID, int secondaryExpirationMonth, int secondaryExpirationYear, int secondaryCreditCardTypeID)
        {
            CustomerAccount customerAccount = new CustomerAccount();
            customerAccount.CustomerID = customerID;
            customerAccount.PrimaryExpirationMonth = primaryExpirationMonth;
            customerAccount.PrimaryExpirationYear = primaryExpirationYear;
            customerAccount.PrimaryCreditCardTypeID = primaryCreditCardTypeID;
            customerAccount.SecondaryExpirationMonth = secondaryExpirationMonth;
            customerAccount.SecondaryExpirationYear = secondaryExpirationYear;
            customerAccount.SecondaryCreditCardTypeID = secondaryCreditCardTypeID;
            return customerAccount;
        }
        public int CustomerID { get; set; }
        public string PrimaryCreditCardDisplay { get; set; }
        public int PrimaryExpirationMonth { get; set; }
        public int PrimaryExpirationYear { get; set; }
        public int PrimaryCreditCardTypeID { get; set; }
        public string PrimaryBillingName { get; set; }
        public string PrimaryBillingAddress { get; set; }
        public string PrimaryBillingCity { get; set; }
        public string PrimaryBillingState { get; set; }
        public string PrimaryBillingZip { get; set; }
        public string PrimaryBillingCountry { get; set; }
        public string SecondaryCreditCardDisplay { get; set; }
        public int SecondaryExpirationMonth { get; set; }
        public int SecondaryExpirationYear { get; set; }
        public int SecondaryCreditCardTypeID { get; set; }
        public string SecondaryBillingName { get; set; }
        public string SecondaryBillingAddress { get; set; }
        public string SecondaryBillingCity { get; set; }
        public string SecondaryBillingState { get; set; }
        public string SecondaryBillingZip { get; set; }
        public string SecondaryBillingCountry { get; set; }
        public string BankAccountNumber { get; set; }
        public string BankRoutingNumber { get; set; }
        public string BankNameOnAccount { get; set; }
        public string BankAccountAddress { get; set; }
        public string BankAccountCity { get; set; }
        public string BankAccountState { get; set; }
        public string BankAccountZip { get; set; }
        public string BankAccountCountry { get; set; }
        public string DriversLicenseNumber { get; set; }
        public string DepositNameOnAcount { get; set; }
        public string DepositAccountNumber { get; set; }
        public string DepositRoutingNumber { get; set; }
        public string Iban { get; set; }
        public string SwiftCode { get; set; }
        public string DepositBankName { get; set; }
        public string DepositBankAddress { get; set; }
        public string DepositBankCity { get; set; }
        public string DepositBankState { get; set; }
        public string DepositBankZip { get; set; }
        public string DepositBankCountry { get; set; }
        public Customer Customer { get; set; }
        public CreditCardType PrimaryCreditCardType { get; set; }
        public CreditCardType SecondaryCreditCardType { get; set; }
    }
    public partial class SubscriptionStatus 
    {
        public static SubscriptionStatus CreateSubscriptionStatus(int subscriptionStatusID)
        {
            SubscriptionStatus subscriptionStatus = new SubscriptionStatus();
            subscriptionStatus.SubscriptionStatusID = subscriptionStatusID;
            return subscriptionStatus;
        }
        public int SubscriptionStatusID { get; set; }
        public string SubscriptionStatusDescription { get; set; }
    }
    public partial class CustomerSubscription 
    {
        public static CustomerSubscription CreateCustomerSubscription(int customerID, int subscriptionID, int subscriptionStatusID, global::System.DateTime startDate, global::System.DateTime expireDate)
        {
            CustomerSubscription customerSubscription = new CustomerSubscription();
            customerSubscription.CustomerID = customerID;
            customerSubscription.SubscriptionID = subscriptionID;
            customerSubscription.SubscriptionStatusID = subscriptionStatusID;
            customerSubscription.StartDate = startDate;
            customerSubscription.ExpireDate = expireDate;
            return customerSubscription;
        }
        public int CustomerID { get; set; }
        public int SubscriptionID { get; set; }
        public int SubscriptionStatusID { get; set; }
        public global::System.DateTime StartDate { get; set; }
        public global::System.DateTime ExpireDate { get; set; }
        public Subscription Subscription { get; set; }
        public Customer Customer { get; set; }
        public SubscriptionStatus SubscriptionStatus { get; set; }
    }
    public partial class CustomerPointAccount 
    {
        public static CustomerPointAccount CreateCustomerPointAccount(int customerID, int pointAccountID, decimal pointBalance)
        {
            CustomerPointAccount customerPointAccount = new CustomerPointAccount();
            customerPointAccount.CustomerID = customerID;
            customerPointAccount.PointAccountID = pointAccountID;
            customerPointAccount.PointBalance = pointBalance;
            return customerPointAccount;
        }
        public int CustomerID { get; set; }
        public int PointAccountID { get; set; }
        public decimal PointBalance { get; set; }
        public PointAccount PointAccount { get; set; }
        public Customer Customer { get; set; }
    }
    public partial class CommissionRun 
    {
        public static CommissionRun CreateCommissionRun(int commissionRunID, int periodTypeID, int periodID)
        {
            CommissionRun commissionRun = new CommissionRun();
            commissionRun.CommissionRunID = commissionRunID;
            commissionRun.PeriodTypeID = periodTypeID;
            commissionRun.PeriodID = periodID;
            return commissionRun;
        }
        public int CommissionRunID { get; set; }
        public string CommissionRunDescription { get; set; }
        public int PeriodTypeID { get; set; }
        public int PeriodID { get; set; }
        public global::System.Nullable<global::System.DateTime> AcceptedDate { get; set; }
        public Period Period { get; set; }
    }
    public partial class Commission 
    {
        public static Commission CreateCommission(int commissionRunID, int customerID, decimal earnings, decimal previousBalance, decimal balanceForward, decimal fee, decimal total, int paidRankID)
        {
            Commission commission = new Commission();
            commission.CommissionRunID = commissionRunID;
            commission.CustomerID = customerID;
            commission.Earnings = earnings;
            commission.PreviousBalance = previousBalance;
            commission.BalanceForward = balanceForward;
            commission.Fee = fee;
            commission.Total = total;
            commission.PaidRankID = paidRankID;
            return commission;
        }
        public int CommissionRunID { get; set; }
        public int CustomerID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Earnings { get; set; }
        public decimal PreviousBalance { get; set; }
        public decimal BalanceForward { get; set; }
        public decimal Fee { get; set; }
        public decimal Total { get; set; }
        public int PaidRankID { get; set; }
        public CommissionRun CommissionRun { get; set; }
        public Customer Customer { get; set; }
        public Rank PaidRank { get; set; }
    }
    public partial class CommissionBonus 
    {
        public static CommissionBonus CreateCommissionBonus(int commissionRunID, int customerID, int bonusID, decimal amount)
        {
            CommissionBonus commissionBonus = new CommissionBonus();
            commissionBonus.CommissionRunID = commissionRunID;
            commissionBonus.CustomerID = customerID;
            commissionBonus.BonusID = bonusID;
            commissionBonus.Amount = amount;
            return commissionBonus;
        }
        public int CommissionRunID { get; set; }
        public int CustomerID { get; set; }
        public int BonusID { get; set; }
        public string BonusDescription { get; set; }
        public decimal Amount { get; set; }
        public Commission Commission { get; set; }
        public CommissionRun CommissionRun { get; set; }
    }
    public partial class CommissionDetail 
    {
        public static CommissionDetail CreateCommissionDetail(int commissionRunID, int commissionDetailID, int customerID, int bonusID, int fromCustomerID, int orderID, decimal sourceAmount, decimal percentage, decimal commissionAmount, int level, int paidLevel)
        {
            CommissionDetail commissionDetail = new CommissionDetail();
            commissionDetail.CommissionRunID = commissionRunID;
            commissionDetail.CommissionDetailID = commissionDetailID;
            commissionDetail.CustomerID = customerID;
            commissionDetail.BonusID = bonusID;
            commissionDetail.FromCustomerID = fromCustomerID;
            commissionDetail.OrderID = orderID;
            commissionDetail.SourceAmount = sourceAmount;
            commissionDetail.Percentage = percentage;
            commissionDetail.CommissionAmount = commissionAmount;
            commissionDetail.Level = level;
            commissionDetail.PaidLevel = paidLevel;
            return commissionDetail;
        }
        public int CommissionRunID { get; set; }
        public int CommissionDetailID { get; set; }
        public int CustomerID { get; set; }
        public int BonusID { get; set; }
        public int FromCustomerID { get; set; }
        public int OrderID { get; set; }
        public decimal SourceAmount { get; set; }
        public decimal Percentage { get; set; }
        public decimal CommissionAmount { get; set; }
        public int Level { get; set; }
        public int PaidLevel { get; set; }
        public Customer FromCustomer { get; set; }
        public Commission Commission { get; set; }
        public CommissionBonus Bonus { get; set; }
        public CommissionRun CommissionRun { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerExtendedGroup in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerExtendedGroupID
    /// </KeyProperties>
    public partial class CustomerExtendedGroup
    {
        /// <summary>
        /// Create a new CustomerExtendedGroup object.
        /// </summary>
        /// <param name="customerExtendedGroupID">Initial value of CustomerExtendedGroupID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static CustomerExtendedGroup CreateCustomerExtendedGroup(int customerExtendedGroupID)
        {
            CustomerExtendedGroup customerExtendedGroup = new CustomerExtendedGroup();
            customerExtendedGroup.CustomerExtendedGroupID = customerExtendedGroupID;
            return customerExtendedGroup;
        }
        /// <summary>
        /// There are no comments for Property CustomerExtendedGroupID in the schema.
        /// </summary>
        public int CustomerExtendedGroupID { get; set; }
        public string CustomerExtendedGroupDescription { get; set; }
        public string Field1Name { get; set; }
        public string Field2Name { get; set; }
        public string Field3Name { get; set; }
        public string Field4Name { get; set; }
        public string Field5Name { get; set; }
        public string Field6Name { get; set; }
        public string Field7Name { get; set; }
        public string Field8Name { get; set; }
        public string Field9Name { get; set; }
        public string Field10Name { get; set; }
        public string Field11Name { get; set; }
        public string Field12Name { get; set; }
        public string Field13Name { get; set; }
        public string Field14Name { get; set; }
        public string Field15Name { get; set; }
        public string Field16Name { get; set; }
        public string Field17Name { get; set; }
        public string Field18Name { get; set; }
        public string Field19Name { get; set; }
        public string Field20Name { get; set; }
        public string Field21Name { get; set; }
        public string Field22Name { get; set; }
        public string Field23Name { get; set; }
        public string Field24Name { get; set; }
        public string Field25Name { get; set; }
        public string Field26Name { get; set; }
        public string Field27Name { get; set; }
        public string Field28Name { get; set; }
        public string Field29Name { get; set; }
        public string Field30Name { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerExtendedDetail in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerExtendedDetailID
    /// </KeyProperties>
    public partial class CustomerExtendedDetail
    {
        /// <summary>
        /// Create a new CustomerExtendedDetail object.
        /// </summary>
        /// <param name="customerExtendedDetailID">Initial value of CustomerExtendedDetailID.</param>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="customerExtendedGroupID">Initial value of CustomerExtendedGroupID.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        public static CustomerExtendedDetail CreateCustomerExtendedDetail(int customerExtendedDetailID, int customerID, int customerExtendedGroupID, global::System.DateTime modifiedDate)
        {
            CustomerExtendedDetail customerExtendedDetail = new CustomerExtendedDetail();
            customerExtendedDetail.CustomerExtendedDetailID = customerExtendedDetailID;
            customerExtendedDetail.CustomerID = customerID;
            customerExtendedDetail.CustomerExtendedGroupID = customerExtendedGroupID;
            customerExtendedDetail.ModifiedDate = modifiedDate;
            return customerExtendedDetail;
        }
        /// <summary>
        /// There are no comments for Property CustomerExtendedDetailID in the schema.
        /// </summary>
        public int CustomerExtendedDetailID { get; set; }
        public int CustomerID { get; set; }
        public int CustomerExtendedGroupID { get; set; }
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
        public string Field16 { get; set; }
        public string Field17 { get; set; }
        public string Field18 { get; set; }
        public string Field19 { get; set; }
        public string Field20 { get; set; }
        public string Field21 { get; set; }
        public string Field22 { get; set; }
        public string Field23 { get; set; }
        public string Field24 { get; set; }
        public string Field25 { get; set; }
        public string Field26 { get; set; }
        public string Field27 { get; set; }
        public string Field28 { get; set; }
        public string Field29 { get; set; }
        public string Field30 { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public Customer Customer { get; set; }
        public CustomerExtendedGroup CustomerExtendedGroup { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.PayoutBill in the schema.
    /// </summary>
    /// <KeyProperties>
    /// TransactionID
    /// PayoutID
    /// </KeyProperties>
    public partial class PayoutBill
    {
        /// <summary>
        /// Create a new PayoutBill object.
        /// </summary>
        /// <param name="transactionID">Initial value of TransactionID.</param>
        /// <param name="payoutID">Initial value of PayoutID.</param>
        /// <param name="createdDate">Initial value of CreatedDate.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        public static PayoutBill CreatePayoutBill(int transactionID, int payoutID, global::System.DateTime createdDate, global::System.DateTime modifiedDate)
        {
            PayoutBill payoutBill = new PayoutBill();
            payoutBill.TransactionID = transactionID;
            payoutBill.PayoutID = payoutID;
            payoutBill.CreatedDate = createdDate;
            payoutBill.ModifiedDate = modifiedDate;
            return payoutBill;
        }
        /// <summary>
        /// There are no comments for Property TransactionID in the schema.
        /// </summary>
        public int TransactionID { get; set; }
        public int PayoutID { get; set; }
        public global::System.Nullable<int> BillID { get; set; }
        public global::System.DateTime CreatedDate { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public Payout Payout { get; set; }
        public Bill Bill { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.Payout in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PayoutID
    /// </KeyProperties>
    public partial class Payout
    {
        /// <summary>
        /// Create a new Payout object.
        /// </summary>
        /// <param name="payoutID">Initial value of PayoutID.</param>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="payoutDate">Initial value of PayoutDate.</param>
        /// <param name="payoutTypeID">Initial value of PayoutTypeID.</param>
        /// <param name="amount">Initial value of Amount.</param>
        /// <param name="isTaxable">Initial value of IsTaxable.</param>
        /// <param name="bankAccountID">Initial value of BankAccountID.</param>
        /// <param name="createdDate">Initial value of CreatedDate.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        public static Payout CreatePayout(int payoutID, int customerID, global::System.DateTime payoutDate, int payoutTypeID, decimal amount, bool isTaxable, int bankAccountID, global::System.DateTime createdDate, global::System.DateTime modifiedDate)
        {
            Payout payout = new Payout();
            payout.PayoutID = payoutID;
            payout.CustomerID = customerID;
            payout.PayoutDate = payoutDate;
            payout.PayoutTypeID = payoutTypeID;
            payout.Amount = amount;
            payout.IsTaxable = isTaxable;
            payout.BankAccountID = bankAccountID;
            payout.CreatedDate = createdDate;
            payout.ModifiedDate = modifiedDate;
            return payout;
        }
        /// <summary>
        /// There are no comments for Property PayoutID in the schema.
        /// </summary>
        public int PayoutID { get; set; }
        public int CustomerID { get; set; }
        public global::System.DateTime PayoutDate { get; set; }
        public int PayoutTypeID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public string PayeeName { get; set; }
        public string PayeeCompany { get; set; }
        public string PayeeAddress1 { get; set; }
        public string PayeeAddress2 { get; set; }
        public string PayeeCity { get; set; }
        public string PayeeState { get; set; }
        public string PayeeZip { get; set; }
        public string PayeeCountry { get; set; }
        public global::System.Nullable<int> CheckNumber { get; set; }
        public global::System.Nullable<global::System.DateTime> CheckDate { get; set; }
        public global::System.Nullable<global::System.DateTime> VoidedDate { get; set; }
        public global::System.Nullable<int> DepositNumber { get; set; }
        public bool IsTaxable { get; set; }
        public int BankAccountID { get; set; }
        public string Reference { get; set; }
        public global::System.DateTime CreatedDate { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Customer Customer { get; set; }
        public PayoutType PayoutType { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.PayoutType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PayoutTypeID
    /// </KeyProperties>
    public partial class PayoutType
    {
        /// <summary>
        /// Create a new PayoutType object.
        /// </summary>
        /// <param name="payoutTypeID">Initial value of PayoutTypeID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static PayoutType CreatePayoutType(int payoutTypeID)
        {
            PayoutType payoutType = new PayoutType();
            payoutType.PayoutTypeID = payoutTypeID;
            return payoutType;
        }
        /// <summary>
        /// There are no comments for Property PayoutTypeID in the schema.
        /// </summary>
        public int PayoutTypeID { get; set; }
        public string PayoutTypeDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.Bill in the schema.
    /// </summary>
    /// <KeyProperties>
    /// BillID
    /// </KeyProperties>
    public partial class Bill
    {
        /// <summary>
        /// Create a new Bill object.
        /// </summary>
        /// <param name="billID">Initial value of BillID.</param>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="billStatusID">Initial value of BillStatusID.</param>
        /// <param name="dueDate">Initial value of DueDate.</param>
        /// <param name="billTypeID">Initial value of BillTypeID.</param>
        /// <param name="amount">Initial value of Amount.</param>
        /// <param name="isOtherIncome">Initial value of IsOtherIncome.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        public static Bill CreateBill(int billID, int customerID, int billStatusID, global::System.DateTime dueDate, int billTypeID, decimal amount, bool isOtherIncome, global::System.DateTime modifiedDate)
        {
            Bill bill = new Bill();
            bill.BillID = billID;
            bill.CustomerID = customerID;
            bill.BillStatusID = billStatusID;
            bill.DueDate = dueDate;
            bill.BillTypeID = billTypeID;
            bill.Amount = amount;
            bill.IsOtherIncome = isOtherIncome;
            bill.ModifiedDate = modifiedDate;
            return bill;
        }
        /// <summary>
        /// There are no comments for Property BillID in the schema.
        /// </summary>
        public int BillID { get; set; }
        public int CustomerID { get; set; }
        public int BillStatusID { get; set; }
        public global::System.DateTime DueDate { get; set; }
        public int BillTypeID { get; set; }
        public string CurrencyCode { get; set; }
        public decimal Amount { get; set; }
        public global::System.Nullable<int> CommissionRunID { get; set; }
        public bool IsOtherIncome { get; set; }
        public string Reference { get; set; }
        public string Notes { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public Customer Customer { get; set; }
        public BillStatus BillStatus { get; set; }
        public BillType BillType { get; set; }
        public CommissionRun CommissionRun { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.BillStatus in the schema.
    /// </summary>
    /// <KeyProperties>
    /// BillStatusID
    /// </KeyProperties>
    public partial class BillStatus
    {
        /// <summary>
        /// Create a new BillStatus object.
        /// </summary>
        /// <param name="billStatusID">Initial value of BillStatusID.</param>
        public static BillStatus CreateBillStatus(int billStatusID)
        {
            BillStatus billStatus = new BillStatus();
            billStatus.BillStatusID = billStatusID;
            return billStatus;
        }
        /// <summary>
        /// There are no comments for Property BillStatusID in the schema.
        /// </summary>
        public int BillStatusID { get; set; }
        public string BillStatusDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.BillType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// BillTypeID
    /// </KeyProperties>
    public partial class BillType
    {
        /// <summary>
        /// Create a new BillType object.
        /// </summary>
        /// <param name="billTypeID">Initial value of BillTypeID.</param>
        public static BillType CreateBillType(int billTypeID)
        {
            BillType billType = new BillType();
            billType.BillTypeID = billTypeID;
            return billType;
        }
        /// <summary>
        /// There are no comments for Property BillTypeID in the schema.
        /// </summary>
        public int BillTypeID { get; set; }
        public string BillTypeDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.Country in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CountryCode
    /// </KeyProperties>
    public partial class Country
    {
        /// <summary>
        /// Create a new Country object.
        /// </summary>
        /// <param name="countryCode">Initial value of CountryCode.</param>
        /// <param name="sortOrder">Initial value of SortOrder.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Country CreateCountry(string countryCode, int sortOrder)
        {
            Country country = new Country();
            country.CountryCode = countryCode;
            country.SortOrder = sortOrder;
            return country;
        }
        /// <summary>
        /// There are no comments for Property CountryCode in the schema.
        /// </summary>
        public string CountryCode { get; set; }
        public string CountryDescription { get; set; }
        public int SortOrder { get; set; }
        public global::System.Data.Services.Client.DataServiceCollection<CountryRegion> Regions { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CountryRegion in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CountryCode
    /// RegionCode
    /// </KeyProperties>
    public partial class CountryRegion
    {
        /// <summary>
        /// Create a new CountryRegion object.
        /// </summary>
        /// <param name="countryCode">Initial value of CountryCode.</param>
        /// <param name="regionCode">Initial value of RegionCode.</param>
        /// <param name="sortOrder">Initial value of SortOrder.</param>
        public static CountryRegion CreateCountryRegion(string countryCode, string regionCode, int sortOrder)
        {
            CountryRegion countryRegion = new CountryRegion();
            countryRegion.CountryCode = countryCode;
            countryRegion.RegionCode = regionCode;
            countryRegion.SortOrder = sortOrder;
            return countryRegion;
        }
        /// <summary>
        /// There are no comments for Property CountryCode in the schema.
        /// </summary>
        public string CountryCode { get; set; }
        public string RegionCode { get; set; }
        public string RegionDescription { get; set; }
        public int SortOrder { get; set; }
        public Country Country { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerSite in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerID
    /// WebAlias
    /// </KeyProperties>
    public partial class CustomerSite
    {
        /// <summary>
        /// Create a new CustomerSite object.
        /// </summary>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="webAlias">Initial value of WebAlias.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        public static CustomerSite CreateCustomerSite(int customerID, string webAlias, global::System.DateTime modifiedDate)
        {
            CustomerSite customerSite = new CustomerSite();
            customerSite.CustomerID = customerID;
            customerSite.WebAlias = webAlias;
            customerSite.ModifiedDate = modifiedDate;
            return customerSite;
        }
        /// <summary>
        /// There are no comments for Property CustomerID in the schema.
        /// </summary>
        public int CustomerID { get; set; }
        public string WebAlias { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Phone2 { get; set; }
        public string Fax { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string Notes1 { get; set; }
        public string Notes2 { get; set; }
        public string Notes3 { get; set; }
        public string Notes4 { get; set; }
        public string Url1 { get; set; }
        public string Url2 { get; set; }
        public string Url3 { get; set; }
        public string Url4 { get; set; }
        public string Url5 { get; set; }
        public string Url6 { get; set; }
        public string Url7 { get; set; }
        public string Url8 { get; set; }
        public string Url9 { get; set; }
        public string Url10 { get; set; }
        public string Url1Description { get; set; }
        public string Url2Description { get; set; }
        public string Url3Description { get; set; }
        public string Url4Description { get; set; }
        public string Url5Description { get; set; }
        public string Url6Description { get; set; }
        public string Url7Description { get; set; }
        public string Url8Description { get; set; }
        public string Url9Description { get; set; }
        public string Url10Description { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public Customer Customer { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerPayoutSetting in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerID
    /// </KeyProperties>
    public partial class CustomerPayoutSetting
    {
        /// <summary>
        /// Create a new CustomerPayoutSetting object.
        /// </summary>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="produce1099">Initial value of Produce1099.</param>
        /// <param name="taxAddressTypeID">Initial value of TaxAddressTypeID.</param>
        /// <param name="taxNameTypeID">Initial value of TaxNameTypeID.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        public static CustomerPayoutSetting CreateCustomerPayoutSetting(int customerID, bool produce1099, int taxAddressTypeID, int taxNameTypeID, global::System.DateTime modifiedDate)
        {
            CustomerPayoutSetting customerPayoutSetting = new CustomerPayoutSetting();
            customerPayoutSetting.CustomerID = customerID;
            customerPayoutSetting.Produce1099 = produce1099;
            customerPayoutSetting.TaxAddressTypeID = taxAddressTypeID;
            customerPayoutSetting.TaxNameTypeID = taxNameTypeID;
            customerPayoutSetting.ModifiedDate = modifiedDate;
            return customerPayoutSetting;
        }
        /// <summary>
        /// There are no comments for Property CustomerID in the schema.
        /// </summary>
        public int CustomerID { get; set; }
        public bool Produce1099 { get; set; }
        public int TaxAddressTypeID { get; set; }
        public int TaxNameTypeID { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
        public string ModifiedBy { get; set; }
        public Customer Customer { get; set; }
        public TaxAddressType TaxAddressType { get; set; }
        public TaxNameType TaxNameType { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.TaxAddressType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// TaxAddressTypeID
    /// </KeyProperties>
    public partial class TaxAddressType
    {
        /// <summary>
        /// Create a new TaxAddressType object.
        /// </summary>
        /// <param name="taxAddressTypeID">Initial value of TaxAddressTypeID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static TaxAddressType CreateTaxAddressType(int taxAddressTypeID)
        {
            TaxAddressType taxAddressType = new TaxAddressType();
            taxAddressType.TaxAddressTypeID = taxAddressTypeID;
            return taxAddressType;
        }
        /// <summary>
        /// There are no comments for Property TaxAddressTypeID in the schema.
        /// </summary>
        public int TaxAddressTypeID { get; set; }
        public string TaxAddressTypeDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.TaxNameType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// TaxNameTypeID
    /// </KeyProperties>
    public partial class TaxNameType
    {
        /// <summary>
        /// Create a new TaxNameType object.
        /// </summary>
        /// <param name="taxNameTypeID">Initial value of TaxNameTypeID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static TaxNameType CreateTaxNameType(int taxNameTypeID)
        {
            TaxNameType taxNameType = new TaxNameType();
            taxNameType.TaxNameTypeID = taxNameTypeID;
            return taxNameType;
        }
        /// <summary>
        /// There are no comments for Property TaxNameTypeID in the schema.
        /// </summary>
        public int TaxNameTypeID { get; set; }
        public string TaxNameTypeDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.TaxCodeType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// TaxCodeTypeID
    /// </KeyProperties>
    public partial class TaxCodeType
    {
        /// <summary>
        /// Create a new TaxCodeType object.
        /// </summary>
        /// <param name="taxCodeTypeID">Initial value of TaxCodeTypeID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static TaxCodeType CreateTaxCodeType(int taxCodeTypeID)
        {
            TaxCodeType taxCodeType = new TaxCodeType();
            taxCodeType.TaxCodeTypeID = taxCodeTypeID;
            return taxCodeType;
        }
        /// <summary>
        /// There are no comments for Property TaxCodeTypeID in the schema.
        /// </summary>
        public int TaxCodeTypeID { get; set; }
        public string TaxCodeTypeDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerInquiry in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerInquiryID
    /// CustomerID
    /// CustomerInquiryCategoryID
    /// CustomerInquiryStatusID
    /// CustomerInquiryTypeID
    /// </KeyProperties>
    public partial class CustomerInquiry
    {
        /// <summary>
        /// Create a new CustomerInquiry object.
        /// </summary>
        /// <param name="customerInquiryID">Initial value of CustomerInquiryID.</param>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="customerInquiryCategoryID">Initial value of CustomerInquiryCategoryID.</param>
        /// <param name="customerInquiryStatusID">Initial value of CustomerInquiryStatusID.</param>
        /// <param name="customerInquiryTypeID">Initial value of CustomerInquiryTypeID.</param>
        /// <param name="createdDate">Initial value of CreatedDate.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static CustomerInquiry CreateCustomerInquiry(int customerInquiryID, int customerID, int customerInquiryCategoryID, int customerInquiryStatusID, int customerInquiryTypeID, global::System.DateTime createdDate)
        {
            CustomerInquiry customerInquiry = new CustomerInquiry();
            customerInquiry.CustomerInquiryID = customerInquiryID;
            customerInquiry.CustomerID = customerID;
            customerInquiry.CustomerInquiryCategoryID = customerInquiryCategoryID;
            customerInquiry.CustomerInquiryStatusID = customerInquiryStatusID;
            customerInquiry.CustomerInquiryTypeID = customerInquiryTypeID;
            customerInquiry.CreatedDate = createdDate;
            return customerInquiry;
        }
        /// <summary>
        /// There are no comments for Property CustomerInquiryID in the schema.
        /// </summary>
        public int CustomerInquiryID { get; set; }
        public int CustomerID { get; set; }
        public int CustomerInquiryCategoryID { get; set; }
        public int CustomerInquiryStatusID { get; set; }
        public int CustomerInquiryTypeID { get; set; }
        public global::System.Nullable<int> ParentID { get; set; }
        public global::System.DateTime CreatedDate { get; set; }
        public string Description { get; set; }
        public string Detail { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedTo { get; set; }
        public global::System.Nullable<global::System.DateTime> ClosedDate { get; set; }
        public string ClosedBy { get; set; }
        public Customer Customer { get; set; }
        public CustomerInquiryCategory CustomerInquiryCategory { get; set; }
        public CustomerInquiryType CustomerInquiryType { get; set; }
        public CustomerInquiryStatus CustomerInquiryStatus { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerInquiryCategory in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerInquiryCategoryID
    /// </KeyProperties>

    public partial class CustomerInquiryCategory
    {
        /// <summary>
        /// Create a new CustomerInquiryCategory object.
        /// </summary>
        /// <param name="customerInquiryCategoryID">Initial value of CustomerInquiryCategoryID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static CustomerInquiryCategory CreateCustomerInquiryCategory(int customerInquiryCategoryID)
        {
            CustomerInquiryCategory customerInquiryCategory = new CustomerInquiryCategory();
            customerInquiryCategory.CustomerInquiryCategoryID = customerInquiryCategoryID;
            return customerInquiryCategory;
        }
        /// <summary>
        /// There are no comments for Property CustomerInquiryCategoryID in the schema.
        /// </summary>
        public int CustomerInquiryCategoryID { get; set; }
        public string CustomerInquiryCategoryDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerInquiryType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerInquiryTypeID
    /// </KeyProperties>
    public partial class CustomerInquiryType
    {
        /// <summary>
        /// Create a new CustomerInquiryType object.
        /// </summary>
        /// <param name="customerInquiryTypeID">Initial value of CustomerInquiryTypeID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static CustomerInquiryType CreateCustomerInquiryType(int customerInquiryTypeID)
        {
            CustomerInquiryType customerInquiryType = new CustomerInquiryType();
            customerInquiryType.CustomerInquiryTypeID = customerInquiryTypeID;
            return customerInquiryType;
        }
        /// <summary>
        /// There are no comments for Property CustomerInquiryTypeID in the schema.
        /// </summary>
        public int CustomerInquiryTypeID { get; set; }
        public string CustomerInquiryTypeDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerInquiryStatus in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerInquiryStatusID
    /// </KeyProperties>
    public partial class CustomerInquiryStatus
    {
        /// <summary>
        /// Create a new CustomerInquiryStatus object.
        /// </summary>
        /// <param name="customerInquiryStatusID">Initial value of CustomerInquiryStatusID.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static CustomerInquiryStatus CreateCustomerInquiryStatus(int customerInquiryStatusID)
        {
            CustomerInquiryStatus customerInquiryStatus = new CustomerInquiryStatus();
            customerInquiryStatus.CustomerInquiryStatusID = customerInquiryStatusID;
            return customerInquiryStatus;
        }
        /// <summary>
        /// There are no comments for Property CustomerInquiryStatusID in the schema.
        /// </summary>
        public int CustomerInquiryStatusID { get; set; }
        public string CustomerInquiryStatusDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.Session in the schema.
    /// </summary>
    /// <KeyProperties>
    /// SessionID
    /// </KeyProperties>
    public partial class Session
    {
        /// <summary>
        /// Create a new Session object.
        /// </summary>
        /// <param name="sessionID">Initial value of SessionID.</param>
        /// <param name="createdDate">Initial value of CreatedDate.</param>
        /// <param name="modifiedDate">Initial value of ModifiedDate.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static Session CreateSession(global::System.Guid sessionID, global::System.DateTime createdDate, global::System.DateTime modifiedDate)
        {
            Session session = new Session();
            session.SessionID = sessionID;
            session.CreatedDate = createdDate;
            session.ModifiedDate = modifiedDate;
            return session;
        }
        /// <summary>
        /// There are no comments for Property SessionID in the schema.
        /// </summary>
        public global::System.Guid SessionID { get; set; }
        public string Data { get; set; }
        public global::System.DateTime CreatedDate { get; set; }
        public global::System.DateTime ModifiedDate { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.PeriodRankScore in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PeriodTypeID
    /// PeriodID
    /// CustomerID
    /// PaidRankID
    /// </KeyProperties>
    public partial class PeriodRankScore
    {
        /// <summary>
        /// Create a new PeriodRankScore object.
        /// </summary>
        /// <param name="periodTypeID">Initial value of PeriodTypeID.</param>
        /// <param name="periodID">Initial value of PeriodID.</param>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="paidRankID">Initial value of PaidRankID.</param>
        /// <param name="score">Initial value of Score.</param>
        public static PeriodRankScore CreatePeriodRankScore(int periodTypeID, int periodID, int customerID, int paidRankID, decimal score)
        {
            PeriodRankScore periodRankScore = new PeriodRankScore();
            periodRankScore.PeriodTypeID = periodTypeID;
            periodRankScore.PeriodID = periodID;
            periodRankScore.CustomerID = customerID;
            periodRankScore.PaidRankID = paidRankID;
            periodRankScore.Score = score;
            return periodRankScore;
        }
        /// <summary>
        /// There are no comments for Property PeriodTypeID in the schema.
        /// </summary>
        public int PeriodTypeID { get; set; }
        public int PeriodID { get; set; }
        public int CustomerID { get; set; }
        public int PaidRankID { get; set; }
        public decimal Score { get; set; }
        public Rank PaidRank { get; set; }
        public Customer Customer { get; set; }
        public Period Period { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.PaymentTotal in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CurrencyCode
    /// PaymentTypeID
    /// WarehouseID
    /// </KeyProperties>
    public partial class PaymentTotal
    {
        /// <summary>
        /// Create a new PaymentTotal object.
        /// </summary>
        /// <param name="currencyCode">Initial value of CurrencyCode.</param>
        /// <param name="paymentTypeID">Initial value of PaymentTypeID.</param>
        /// <param name="warehouseID">Initial value of WarehouseID.</param>
        /// <param name="paymentAmount">Initial value of PaymentAmount.</param>
        /// <param name="paymentCount">Initial value of PaymentCount.</param>
        public static PaymentTotal CreatePaymentTotal(string currencyCode, int paymentTypeID, int warehouseID, decimal paymentAmount, int paymentCount)
        {
            PaymentTotal paymentTotal = new PaymentTotal();
            paymentTotal.CurrencyCode = currencyCode;
            paymentTotal.PaymentTypeID = paymentTypeID;
            paymentTotal.WarehouseID = warehouseID;
            paymentTotal.PaymentAmount = paymentAmount;
            paymentTotal.PaymentCount = paymentCount;
            return paymentTotal;
        }
        /// <summary>
        /// There are no comments for Property CurrencyCode in the schema.
        /// </summary>
        public string CurrencyCode { get; set; }
        public int PaymentTypeID { get; set; }
        public int WarehouseID { get; set; }
        public string WarehouseDescription { get; set; }
        public string CurrencyCodeDescription { get; set; }
        public string PaymentTypeDescription { get; set; }
        public decimal PaymentAmount { get; set; }
        public int PaymentCount { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.NodeSearchResult in the schema.
    /// </summary>
    /// <KeyProperties>
    /// NodeID
    /// </KeyProperties>
    public partial class NodeSearchResult
    {
        /// <summary>
        /// Create a new NodeSearchResult object.
        /// </summary>
        /// <param name="nodeID">Initial value of NodeID.</param>
        /// <param name="parentID">Initial value of ParentID.</param>
        /// <param name="level">Initial value of Level.</param>
        public static NodeSearchResult CreateNodeSearchResult(int nodeID, int parentID, int level)
        {
            NodeSearchResult nodeSearchResult = new NodeSearchResult();
            nodeSearchResult.NodeID = nodeID;
            nodeSearchResult.ParentID = parentID;
            nodeSearchResult.Level = level;
            return nodeSearchResult;
        }
        /// <summary>
        /// There are no comments for Property NodeID in the schema.
        /// </summary>
        public int NodeID { get; set; }
        public int ParentID { get; set; }
        public int Level { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.WallItem in the schema.
    /// </summary>
    /// <KeyProperties>
    /// WallItemID
    /// </KeyProperties>
    public partial class WallItem
    {
        /// <summary>
        /// Create a new WallItem object.
        /// </summary>
        /// <param name="wallItemID">Initial value of WallItemID.</param>
        /// <param name="customerID">Initial value of CustomerID.</param>
        /// <param name="entryDate">Initial value of EntryDate.</param>
        public static WallItem CreateWallItem(int wallItemID, int customerID, global::System.DateTime entryDate)
        {
            WallItem wallItem = new WallItem();
            wallItem.WallItemID = wallItemID;
            wallItem.CustomerID = customerID;
            wallItem.EntryDate = entryDate;
            return wallItem;
        }
        /// <summary>
        /// There are no comments for Property WallItemID in the schema.
        /// </summary>
        public int WallItemID { get; set; }
        public int CustomerID { get; set; }
        public global::System.DateTime EntryDate { get; set; }
        public string Text { get; set; }
        public string Field1 { get; set; }
        public string Field2 { get; set; }
        public string Field3 { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.ImageFile in the schema.
    /// </summary>
    /// <KeyProperties>
    /// Path
    /// FileName
    /// </KeyProperties>
    public partial class ImageFile
    {
        /// <summary>
        /// Create a new ImageFile object.
        /// </summary>
        /// <param name="path">Initial value of Path.</param>
        /// <param name="fileName">Initial value of FileName.</param>
        /// <param name="createdDate">Initial value of CreatedDate.</param>
        /// <param name="size">Initial value of Size.</param>
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Services.Design", "1.0.0")]
        public static ImageFile CreateImageFile(string path, string fileName, global::System.DateTime createdDate, int size)
        {
            ImageFile imageFile = new ImageFile();
            imageFile.Path = path;
            imageFile.FileName = fileName;
            imageFile.CreatedDate = createdDate;
            imageFile.Size = size;
            return imageFile;
        }
        /// <summary>
        /// There are no comments for Property Path in the schema.
        /// </summary>
        public string Path { get; set; }
        public string FileName { get; set; }
        public string Url { get; set; }
        public global::System.DateTime CreatedDate { get; set; }
        public int Size { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CalendarItem in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CalendarID
    /// CalendarItemID
    /// </KeyProperties>
    public partial class CalendarItem
    {
        /// <summary>
        /// Create a new CalendarItem object.
        /// </summary>
        /// <param name="calendarID">Initial value of CalendarID.</param>
        /// <param name="calendarItemID">Initial value of CalendarItemID.</param>
        /// <param name="calendarItemTypeID">Initial value of CalendarItemTypeID.</param>
        /// <param name="calendarItemStatusTypeID">Initial value of CalendarItemStatusTypeID.</param>
        /// <param name="calendarItemPriorityTypeID">Initial value of CalendarItemPriorityTypeID.</param>
        /// <param name="startDate">Initial value of StartDate.</param>
        /// <param name="endDate">Initial value of EndDate.</param>
        /// <param name="timeZone">Initial value of TimeZone.</param>
        /// <param name="contactPhoneType">Initial value of ContactPhoneType.</param>
        /// <param name="hasReminder">Initial value of HasReminder.</param>
        /// <param name="isShared">Initial value of IsShared.</param>
        public static CalendarItem CreateCalendarItem(int calendarID, int calendarItemID, int calendarItemTypeID, int calendarItemStatusTypeID, int calendarItemPriorityTypeID, global::System.DateTime startDate, global::System.DateTime endDate, int timeZone, int contactPhoneType, bool hasReminder, bool isShared)
        {
            CalendarItem calendarItem = new CalendarItem();
            calendarItem.CalendarID = calendarID;
            calendarItem.CalendarItemID = calendarItemID;
            calendarItem.CalendarItemTypeID = calendarItemTypeID;
            calendarItem.CalendarItemStatusTypeID = calendarItemStatusTypeID;
            calendarItem.CalendarItemPriorityTypeID = calendarItemPriorityTypeID;
            calendarItem.StartDate = startDate;
            calendarItem.EndDate = endDate;
            calendarItem.TimeZone = timeZone;
            calendarItem.ContactPhoneType = contactPhoneType;
            calendarItem.HasReminder = hasReminder;
            calendarItem.IsShared = isShared;
            return calendarItem;
        }
        /// <summary>
        /// There are no comments for Property CalendarID in the schema.
        /// </summary>
        public int CalendarID { get; set; }
        public int CalendarItemID { get; set; }
        public global::System.Nullable<int> UserID { get; set; }
        public int CalendarItemTypeID { get; set; }
        public int CalendarItemStatusTypeID { get; set; }
        public int CalendarItemPriorityTypeID { get; set; }
        public string Subject { get; set; }
        public global::System.DateTime StartDate { get; set; }
        public global::System.DateTime EndDate { get; set; }
        public string Location { get; set; }
        public string Notes { get; set; }
        public int TimeZone { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }
        public string ContactInfo { get; set; }
        public string ContactPhone { get; set; }
        public int ContactPhoneType { get; set; }
        public string ContactEmail { get; set; }
        public string EventHost { get; set; }
        public string SpecialGuests { get; set; }
        public string EventFlyer { get; set; }
        public string EventCostInfo { get; set; }
        public string EventConferenceCallOrWebinar { get; set; }
        public string EventRegistrationInfo { get; set; }
        public string EventTags { get; set; }
        public bool HasReminder { get; set; }
        public bool IsShared { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CalendarType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CalendarTypeID
    /// </KeyProperties>
    public partial class CalendarType
    {
        public int CalendarTypeID { get; set; }
        public string CalendarTypeDescription { get; set; }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CalendarViewType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CalendarViewTypeID
    /// </KeyProperties>
    public partial class CalendarViewType
    {
        public int CalendarViewTypeID
        {
            get;
            set;
        }
        public string CalendarViewTypeDescription
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CalendarItemType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CalendarItemTypeID
    /// </KeyProperties>
    public partial class CalendarItemType
    {
        public int CalendarItemTypeID
        {
            get;
            set;
        }
        public string CalendarItemTypeDescription
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CalendarItemStatusType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CalendarItemStatusTypeID
    /// </KeyProperties>
    public partial class CalendarItemStatusType
    {
        public int CalendarItemStatusTypeID
        {
            get;
            set;
        }
        public string CalendarItemStatusTypeDescription
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CalendarItemPriorityType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CalendarItemPriorityTypeID
    /// </KeyProperties>
    public partial class CalendarItemPriorityType
    {
        public int CalendarItemPriorityTypeID
        {
            get;
            set;
        }
        public string CalendarItemPriorityTypeDescription
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.Calendar in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CalendarID
    /// UserID
    /// </KeyProperties>
    public partial class Calendar
    {
        public int CalendarID
        {
            get;
            set;
        }
        public int UserID
        {
            get;
            set;
        }
        public int CalendarTypeID
        {
            get;
            set;
        }
        public string CalendarTitle
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.EmailFromSetting in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerID
    /// MailFrom
    /// </KeyProperties>
    public partial class EmailFromSetting
    {
        public int CustomerID
        {
            get;
            set;
        }
        public string MailFrom
        {
            get;
            set;
        }
        public Customer Customer
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.EmailTemplate in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerID
    /// TemplateID
    /// </KeyProperties>
    public partial class EmailTemplate
    {
        public int CustomerID
        {
            get;
            set;
        }
        public int TemplateID
        {
            get;
            set;
        }
        public string Description
        {
            get;
            set;
        }
        public string Content
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.WebCategory in the schema.
    /// </summary>
    /// <KeyProperties>
    /// WebID
    /// WebCategoryID
    /// </KeyProperties>
    public partial class WebCategory
    {

        public int WebID
        {
            get;
            set;
        }
        public int WebCategoryID
        {
            get;
            set;
        }
        public global::System.Nullable<int> ParentID
        {
            get;
            set;
        }
        public string WebCategoryDescription
        {
            get;
            set;
        }
        public int NestedLevel
        {
            get;
            set;
        }
        public int SortOrder
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.WebCategoryItem in the schema.
    /// </summary>
    /// <KeyProperties>
    /// WebID
    /// WebCategoryID
    /// ItemID
    /// </KeyProperties>
    public partial class WebCategoryItem
    {
        public int WebID
        {
            get;
            set;
        }
        public int WebCategoryID
        {
            get;
            set;
        }
        public int ItemID
        {
            get;
            set;
        }
        public int SortOrder
        {
            get;
            set;
        }
        public WebCategory WebCategory
        {
            get;
            set;
        }
        public Item Item
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.PartyGuest in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PartyID
    /// GuestID
    /// </KeyProperties>
    public partial class PartyGuest
    {
        public int PartyID
        {
            get;
            set;
        }
        public int GuestID
        {
            get;
            set;
        }
        public Party Party
        {
            get;
            set;
        }
        public Guest Guest
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.PointTransactionType in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PointTransactionTypeID
    /// </KeyProperties>
    public partial class PointTransactionType
    {
        public int PointTransactionTypeID
        {
            get;
            set;
        }
        public string PointTransactionTypeDescription
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.PointTransaction in the schema.
    /// </summary>
    /// <KeyProperties>
    /// PointTransactionID
    /// </KeyProperties>
    public partial class PointTransaction
    {
        public int PointTransactionID
        {
            get;
            set;
        }
        public int CustomerID
        {
            get;
            set;
        }
        public int PointAccountID
        {
            get;
            set;
        }
        public decimal Amount
        {
            get;
            set;
        }
        public int PointTransactionTypeID
        {
            get;
            set;
        }
        public global::System.DateTime TransactionDate
        {
            get;
            set;
        }
        public global::System.Nullable<int> OrderID
        {
            get;
            set;
        }
        public string Reference
        {
            get;
            set;
        }
        public Customer Customer
        {
            get;
            set;
        }
        public PointAccount PointAccount
        {
            get;
            set;
        }
        public PointTransactionType PointTransactionType
        {
            get;
            set;
        }
    }
    /// <summary>
    /// There are no comments for Exigo.Api.Models.CustomerLead in the schema.
    /// </summary>
    /// <KeyProperties>
    /// CustomerLeadID
    /// </KeyProperties>
    public partial class CustomerLead
    {

        public int CustomerLeadID
        {
            get;
            set;
        }
        public int CustomerID
        {
            get;
            set;
        }
        public string FirstName
        {
            get;
            set;
        }
        public string LastName
        {
            get;
            set;
        }
        public string Company
        {
            get;
            set;
        }
        public string Email
        {
            get;
            set;
        }
        public string Phone
        {
            get;
            set;
        }
        public string Phone2
        {
            get;
            set;
        }
        public string MobilePhone
        {
            get;
            set;
        }
        public string Fax
        {
            get;
            set;
        }
        public string Address1
        {
            get;
            set;
        }
        public string Address2
        {
            get;
            set;
        }
        public string City
        {
            get;
            set;
        }
        public string State
        {
            get;
            set;
        }
        public string Zip
        {
            get;
            set;
        }
        public string Country
        {
            get;
            set;
        }
        public global::System.Nullable<global::System.DateTime> BirthDate
        {
            get;
            set;
        }
        public string Notes
        {
            get;
            set;
        }
        public global::System.DateTime ModifiedDate
        {
            get;
            set;
        }
    }
}
