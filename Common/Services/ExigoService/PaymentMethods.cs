﻿using Common.Api.ExigoOData;
using Common.Api.ExigoWebService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static IEnumerable<IPaymentMethod> GetCustomerPaymentMethods(int customerID)
        {
            return GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID               = customerID,
                ExcludeIncompleteMethods = false,
                ExcludeInvalidMethods    = false
            });
        }
        public static IEnumerable<IPaymentMethod> GetCustomerPaymentMethods(GetCustomerPaymentMethodsRequest request)
        {
            var methods = new List<IPaymentMethod>();
            if (!HttpContext.Current.Request.IsAuthenticated) return methods.AsEnumerable();

            try
            {
                #region OData Query
                // Get the customer's billing info
                //var billing = Exigo.OData().CustomerAccounts
                //    .Where(c => c.CustomerID == request.CustomerID)
                //    .FirstOrDefault();
                #endregion
                #region SQL Procedure
                var billing = new CustomerAccount();
                using (var context = ExigoService.Exigo.Sql())
                {
                    var sqlProcedure = string.Format("exec GetCustomerBillingInfo {0}", request.CustomerID);
                    billing = context.Query<CustomerAccount>(sqlProcedure).FirstOrDefault();
                }
                #endregion
                if (billing == null) return methods.AsEnumerable();

                // Get the customer's auto orders
                var autoOrders = GetCustomerAutoOrders(new GetCustomerAutoOrdersRequest
                {
                    CustomerID = request.CustomerID
                });


                methods.Add(new BankAccount(ExigoService.BankAccountType.Primary)
                {
                    BankName = string.Empty,
                    NameOnAccount = billing.BankNameOnAccount,
                    AccountNumber = billing.BankAccountNumber,
                    RoutingNumber = billing.BankRoutingNumber,
                    AutoOrderIDs = autoOrders.Where(c => c.AutoOrderProcessTypeID == 3).Select(c => c.AutoOrderID).ToArray(),

                    BillingAddress = new Address()
                    {
                        Address1 = billing.BankAccountAddress,
                        City = billing.BankAccountCity,
                        State = billing.BankAccountState,
                        Zip = billing.BankAccountZip,
                        Country = billing.BankAccountCountry
                    }
                });


                methods.Add(new CreditCard(CreditCardType.Primary)
                {
                    NameOnCard = billing.PrimaryBillingName,
                    CardNumber = billing.PrimaryCreditCardDisplay,
                    ExpirationMonth = billing.PrimaryExpirationMonth,
                    ExpirationYear = billing.PrimaryExpirationYear,
                    TypeOfCard = (TypeOfCreditCard)billing.PrimaryCreditCardTypeID,
                    AutoOrderIDs = autoOrders.Where(c => c.AutoOrderProcessTypeID == 1).Select(c => c.AutoOrderID).ToArray(),

                    BillingAddress = new Address()
                    {
                        Address1 = billing.PrimaryBillingAddress,
                        City = billing.PrimaryBillingCity,
                        State = billing.PrimaryBillingState,
                        Zip = billing.PrimaryBillingZip,
                        Country = billing.PrimaryBillingCountry
                    }
                });


                methods.Add(new CreditCard(CreditCardType.Secondary)
                {
                    NameOnCard = billing.SecondaryBillingName,
                    CardNumber = billing.SecondaryCreditCardDisplay,
                    ExpirationMonth = billing.SecondaryExpirationMonth,
                    ExpirationYear = billing.SecondaryExpirationYear,
                    TypeOfCard = (TypeOfCreditCard)billing.SecondaryCreditCardTypeID,
                    AutoOrderIDs = autoOrders.Where(c => c.AutoOrderProcessTypeID == 2).Select(c => c.AutoOrderID).ToArray(),

                    BillingAddress = new Address()
                    {
                        Address1 = billing.SecondaryBillingAddress,
                        City = billing.SecondaryBillingCity,
                        State = billing.SecondaryBillingState,
                        Zip = billing.SecondaryBillingZip,
                        Country = billing.SecondaryBillingCountry
                    }
                });


                // Filter out invalid or incomplete methods if applicable
                if (request.ExcludeInvalidMethods)
                {
                    methods = methods.Where(c => c.IsValid).ToList();
                }
                if (request.ExcludeIncompleteMethods)
                {
                    methods = methods.Where(c => c.IsComplete).ToList();
                }
                if (request.ExcludeNonAutoshipPaymentMethods)
                {
                    methods = methods.Where(c => c is IAutoOrderPaymentMethod).ToList();
                }

            }
            catch { }
            return methods.AsEnumerable();
        }

        public static CreditCard SaveNewCustomerCreditCard(int customerID, CreditCard card)
        {
            // Get the credit cards on file
            var creditCardsOnFile = GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID            = customerID,
                ExcludeInvalidMethods = true
            }).Where(c => c is CreditCard).Select(c => (CreditCard)c);


            // Do we have any empty slots? If so, save this card to the next available slot
            if (!creditCardsOnFile.Any(c => c.Type == CreditCardType.Primary))
            {
                card.Type = CreditCardType.Primary;
                return SetCustomerCreditCard(customerID, card);
            }
            if (!creditCardsOnFile.Any(c => c.Type == CreditCardType.Secondary))
            {
                card.Type = CreditCardType.Secondary;
                return SetCustomerCreditCard(customerID, card);
            }


            // If not, try to save it to a card slot that does not have any autoships bound to it.
            if (!creditCardsOnFile.Where(c => c.Type == CreditCardType.Primary).Single().IsUsedInAutoOrders)
            {
                card.Type = CreditCardType.Primary;
                return SetCustomerCreditCard(customerID, card);
            }
            if (!creditCardsOnFile.Where(c => c.Type == CreditCardType.Secondary).Single().IsUsedInAutoOrders)
            {
                card.Type = CreditCardType.Secondary;
                return SetCustomerCreditCard(customerID, card);
            }


            // If no autoship-free slots exist, don't save it.
            return card;
        }
        public static CreditCard SetCustomerCreditCard(int customerID, CreditCard card)
        {
            return SetCustomerCreditCard(customerID, card, card.Type);
        }
        public static CreditCard SetCustomerCreditCard(int customerID, CreditCard card, CreditCardType type)
        {
            // New credit cards
            if (type == CreditCardType.New)
            {
                return SaveNewCustomerCreditCard(customerID, card);
            }

            // Validate that we have a token
            var token = card.GetToken();
            if (token.IsNullOrEmpty()) return card;


            // Save the credit card
            var request = new SetAccountCreditCardTokenRequest
            {
                CustomerID            = customerID,

                CreditCardAccountType = (card.Type == CreditCardType.Primary) ? AccountCreditCardType.Primary : AccountCreditCardType.Secondary,
                CreditCardToken       = token,
                ExpirationMonth       = card.ExpirationMonth,
                ExpirationYear        = card.ExpirationYear,

                BillingName           = card.NameOnCard,
                BillingAddress        = card.BillingAddress.AddressDisplay,
                BillingCity           = card.BillingAddress.City,
                BillingState          = card.BillingAddress.State,
                BillingZip            = card.BillingAddress.Zip,
                BillingCountry        = card.BillingAddress.Country
            };
            var response = Exigo.WebService().SetAccountCreditCardToken(request);


            return card;
        }
        public static void DeleteCustomerCreditCard(int customerID, CreditCardType type)
        {
            // If this is a new credit card, don't delete it - we have nothing to delete
            if (type == CreditCardType.New) return;


            // Save the a blank copy of the credit card
            // Passing a blank token will do the trick
            var request = new SetAccountCreditCardTokenRequest
            {
                CustomerID            = customerID,

                CreditCardAccountType = (type == CreditCardType.Primary) ? AccountCreditCardType.Primary : AccountCreditCardType.Secondary,
                CreditCardToken       = string.Empty,
                ExpirationMonth       = 1,
                ExpirationYear        = DateTime.Now.Year + 1,

                BillingName           = string.Empty,
                BillingAddress        = string.Empty,
                BillingCity           = string.Empty,
                BillingState          = string.Empty,
                BillingZip            = string.Empty,
                BillingCountry        = string.Empty
            };
            var response = Exigo.WebService().SetAccountCreditCardToken(request);
        }

        public static BankAccount SaveNewCustomerBankAccount(int customerID, BankAccount account)
        {
            // Get the bank accounts on file
            var bankAccountsOnFile = GetCustomerPaymentMethods(new GetCustomerPaymentMethodsRequest
            {
                CustomerID            = customerID,
                ExcludeInvalidMethods = true
            }).Where(c => c is BankAccount).Select(c => (BankAccount)c);


            // Do we have any empty slots? If so, save this bank account to the next available slot
            if (!bankAccountsOnFile.Any(c => c.Type == ExigoService.BankAccountType.Primary))
            {
                account.Type = ExigoService.BankAccountType.Primary;
                return SetCustomerBankAccount(customerID, account);
            }

            // If not, try to save it to a bank account slot that does not have any autoships bound to it.
            if (!bankAccountsOnFile.Where(c => c.Type == ExigoService.BankAccountType.Primary).Single().IsUsedInAutoOrders)
            {
                account.Type = ExigoService.BankAccountType.Primary;
                return SetCustomerBankAccount(customerID, account);
            }


            // If no autoship-free slots exist, don't save it.
            return account;
        }
        public static BankAccount SetCustomerBankAccount(int customerID, BankAccount account)
        {
            return SetCustomerBankAccount(customerID, account, account.Type);
        }
        public static BankAccount SetCustomerBankAccount(int customerID, BankAccount account, ExigoService.BankAccountType type)
        {
            // New bank accounts
            if (type == ExigoService.BankAccountType.New)
            {
                return SaveNewCustomerBankAccount(customerID, account);
            }


            // Save the bank account
            var request = new SetAccountCheckingRequest
            {
                CustomerID        = customerID,

                BankName          = account.BankName,
                BankAccountNumber = account.AccountNumber,
                BankRoutingNumber = account.RoutingNumber,
                BankAccountType   = Common.Api.ExigoWebService.BankAccountType.CheckingPersonal,

                NameOnAccount     = account.NameOnAccount,
                BillingAddress    = account.BillingAddress.AddressDisplay,
                BillingCity       = account.BillingAddress.City,
                BillingState      = account.BillingAddress.State,
                BillingZip        = account.BillingAddress.Zip,
                BillingCountry    = account.BillingAddress.Country
            };
            var response = Exigo.WebService().SetAccountChecking(request);


            return account;
        }
        public static void DeleteCustomerBankAccount(int customerID, ExigoService.BankAccountType type)
        {
            // If this is a new credit card, don't delete it - we have nothing to delete
            if (type == ExigoService.BankAccountType.New) return;


            // Save the a blank copy of the bank account
            // Save the bank account
            var request = new SetAccountCheckingRequest
            {
                CustomerID        = customerID,

                BankName          = string.Empty,
                BankAccountNumber = string.Empty,
                BankRoutingNumber = string.Empty,
                BankAccountType   = Common.Api.ExigoWebService.BankAccountType.CheckingPersonal,

                NameOnAccount     = string.Empty,
                BillingAddress    = string.Empty, 
                BillingCity       = string.Empty, 
                BillingState      = string.Empty, 
                BillingZip        = string.Empty, 
                BillingCountry    = string.Empty 
            };
            var response = Exigo.WebService().SetAccountChecking(request);
        }
    }
}