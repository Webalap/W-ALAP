using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Services
{

    public class AdminCustomerServices
    {
            
             public static List<CustomerTypeList> CustomerType()
             {
                 List<CustomerTypeList> CustomerTypeList = new List<CustomerTypeList>();
                 try
                 {
                     using (var context = Exigo.Sql())
                     {
                         var sql = @"  SELECT CustomerTypeID as customertype,CustomerTypeDescription FROM CustomerTypes";
                         CustomerTypeList = context.Query<CustomerTypeList>(sql).ToList();
                     }
                 }
                 catch (Exception)
                 {
                     return CustomerTypeList;
                 }
                 return CustomerTypeList;
             }
             public static List<CustomerStatusList> CustomerStatus()
             {
                 List<CustomerStatusList> CustomerStatusList = new List<CustomerStatusList>();
                 try
                 {
                     using (var context = Exigo.Sql())
                     {
                         var sql2 = @"SELECT CustomerStatusID as CustomerStatus,CustomerStatusDescription FROM CustomerStatuses";
                         CustomerStatusList = context.Query<CustomerStatusList>(sql2).ToList();
                     }
                 }
                 catch (Exception)
                 {
                     return CustomerStatusList;
                 }
                 return CustomerStatusList;
             }
             public static List<WarehouseList> Warehouse()
             {
                 List<WarehouseList> WarehouseList = new List<WarehouseList>();
                 try
                 {
                     using (var context = Exigo.Sql())
                     {
                         var sql3 = @"SELECT  WarehouseID ,WarehouseDescription  FROM Warehouses";
                         WarehouseList = context.Query<WarehouseList>(sql3).ToList();
                     }
                 }
                 catch (Exception)
                 {
                     return WarehouseList;
                 }
                 return WarehouseList;
             }
             public static List<LanguageList> Languages()
             {
                 List<LanguageList> Languages = new List<LanguageList>();
                 try
                 {
                     using (var context = Exigo.Sql())
                     {
                         var sql = @"SELECT  LanguageID,LanguageDescription  FROM Languages";
                         Languages = context.Query<LanguageList>(sql).ToList();
                     }
                 }
                 catch (Exception)
                 {
                     return Languages;
                 }
                 return Languages;
             }
             public static List<TaxCodeTypesList> TaxCodeTypes()
             {
                 List<TaxCodeTypesList> TaxCodeTypes = new List<TaxCodeTypesList>();
                 try
                 {
                     using (var context = Exigo.Sql())
                     {
                         var sql = @"SELECT TaxCodeTypeID as TaxIDType,TaxCodeDescription  FROM TaxCodeTypes";
                         TaxCodeTypes = context.Query<TaxCodeTypesList>(sql).ToList();
                     }
                 }
                 catch (Exception)
                 {
                     return TaxCodeTypes;
                 }
                 return TaxCodeTypes;
             }
             public static List<PayableTypesList> PayableTypes()
             {
                 List<PayableTypesList> PayableTypes = new List<PayableTypesList>();
                 try
                 {
                     using (var context = Exigo.Sql())
                     {
                         var sql = @"SELECT PayableTypeID as PayableType,PayableTypeDescription  FROM PayableTypes";
                         PayableTypes = context.Query<PayableTypesList>(sql).ToList();
                     }
                 }
                 catch (Exception)
                 {
                     return PayableTypes;
                 }
                 return PayableTypes;
             }

             public static List<WalletAccountTypesList> WalletAccountTypes()
             {
                 List<WalletAccountTypesList> WalletAccountTypesList = new List<WalletAccountTypesList>();
                 try
                 {
                     WalletAccountTypesList node1 = new WalletAccountTypesList();
                     node1.WalletType = 6;
                     node1.WalletTypeDescription = "Payoneer";
                     WalletAccountTypesList.Add(node1);
                     WalletAccountTypesList node2 = new WalletAccountTypesList();
                     node2.WalletType = 8;
                     node2.WalletTypeDescription = "IPayout";
                     WalletAccountTypesList.Add(node2);
                     WalletAccountTypesList node3 = new WalletAccountTypesList();
                     node3.WalletType = 11;
                     node3.WalletTypeDescription = "HyperWallet";
                     WalletAccountTypesList.Add(node3);
                 }
                 catch (Exception)
                 {
                     return WalletAccountTypesList;
                 }
                 return WalletAccountTypesList;
             }
             public static List<DepositAccountTypesList> DepositAccountTypes()
             {
                 List<DepositAccountTypesList> DepositAccountTypesList = new List<DepositAccountTypesList>();
                 try
                 {
                     DepositAccountTypesList node1 = new DepositAccountTypesList();
                     node1.DepositAccountType = 0;
                     node1.DepositAccountTypeDescription = "Checking";
                     DepositAccountTypesList.Add(node1);
                     DepositAccountTypesList node2 = new DepositAccountTypesList();
                     node2.DepositAccountType = 1;
                     node2.DepositAccountTypeDescription = "Saving";
                     DepositAccountTypesList.Add(node2);
                     DepositAccountTypesList node3 = new DepositAccountTypesList();
                     node3.DepositAccountType = 2;
                     node3.DepositAccountTypeDescription = "Personal";
                     DepositAccountTypesList.Add(node3);
                     DepositAccountTypesList node4 = new DepositAccountTypesList();
                     node4.DepositAccountType = 3;
                     node4.DepositAccountTypeDescription = "Business";
                     DepositAccountTypesList.Add(node4);
                 }
                 catch (Exception)
                 {
                     return DepositAccountTypesList;
                 }
                 return DepositAccountTypesList;
             }
             public static List<CheckingAccountTypesList> CheckingAccountTypes()
             {
                 List<CheckingAccountTypesList> CheckingAccountTypesList = new List<CheckingAccountTypesList>();
                 try
                 {
                     CheckingAccountTypesList node1 = new CheckingAccountTypesList();
                     node1.BankAccountType = 0;
                     node1.BankAccountTypeDescription = "CheckingPersonal";
                     CheckingAccountTypesList.Add(node1);
                     CheckingAccountTypesList node2 = new CheckingAccountTypesList();
                     node2.BankAccountType = 1;
                     node2.BankAccountTypeDescription = "CheckingBusiness";
                     CheckingAccountTypesList.Add(node2);
                     CheckingAccountTypesList node3 = new CheckingAccountTypesList();
                     node3.BankAccountType = 2;
                     node3.BankAccountTypeDescription = "SavingsPersonal";
                     CheckingAccountTypesList.Add(node3);
                     CheckingAccountTypesList node4 = new CheckingAccountTypesList();
                     node4.BankAccountType = 3;
                     node4.BankAccountTypeDescription = "SavingsBusiness";
                     CheckingAccountTypesList.Add(node4);
                 }
                 catch (Exception)
                 {
                     return CheckingAccountTypesList;
                 }
                 return CheckingAccountTypesList;
             }


             public static List<CurrenciesList> CurrenciesList()
             {
                 List<CurrenciesList> CurrenciesList = new List<CurrenciesList>();
                 try
                 {
                     using (var context = Exigo.Sql())
                     {
                         var sql = @"SELECT CurrencyCode,CurrencyDescription,CurrencySymbol FROM Currencies";
                         CurrenciesList = context.Query<CurrenciesList>(sql).ToList();
                     }
                 }
                 catch (Exception)
                 {
                     return CurrenciesList;
                 }
                 return CurrenciesList;
             }


             public static List<PriceTypesList> PriceTypesList()
             {
                 List<PriceTypesList> PriceTypesList = new List<PriceTypesList>();
                 try
                 {
                      using (var context = Exigo.Sql())
                     {
                         var sql = @"SELECT PriceTypeID,PriceTypeDescription  FROM PriceTypes";
                         PriceTypesList = context.Query<PriceTypesList>(sql).ToList();
                     }
                    
                 }
                 catch (Exception)
                 {
                     return PriceTypesList;
                 }
                 return PriceTypesList;
             }

    }
    public class CustomerTypeList
    {
        public int customertype { get; set; }
        public string customertypeDescription { get; set; }
    }
    public class CustomerStatusList
    {
        public int CustomerStatus { get; set; }
        public string CustomerStatusDescription { get; set; }
    }
    public class WarehouseList
    {
        public int WarehouseID { get; set; }
        public string WarehouseDescription { get; set; }
    }
    public class LanguageList
    {
        public int LanguageID { get; set; }
        public string LanguageDescription { get; set; }
    }
    public class TaxCodeTypesList
    {
        public int TaxIDType { get; set; }
        public string TaxCodeDescription { get; set; }
    }
    public class PayableTypesList
    {
        public int PayableType { get; set; }
        public string PayableTypeDescription { get; set; }
    }
    public class WalletAccountTypesList
    {
        public int WalletType { get; set; }
        public string WalletTypeDescription { get; set; }
    }

    public class DepositAccountTypesList
    {
        public int DepositAccountType { get; set; }
        public string DepositAccountTypeDescription { get; set; }
    }

    public class CheckingAccountTypesList
    {
        public int BankAccountType { get; set; }
        public string BankAccountTypeDescription { get; set; }
    }

    public class CurrenciesList
    {
        public string CurrencyCode { get; set; }
        public string CurrencyDescription { get; set; }
        public string CurrencySymbol { get; set; }

    }
    public class PriceTypesList
    {
        public int PriceTypeID { get; set; }
        public string PriceTypeDescription { get; set; }
    }
}