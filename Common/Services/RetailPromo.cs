using Common.ModelsEx.Shopping;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Common.Services
{
    public class RetailPromo
    {
        public string ItemCode { get; set; }
        public int DiscountType { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal BV { get; set; }
        public decimal CV { get; set; }
    }
    public class RetailPromoService
    {
        public static RetailPromo GetRetailPromo(Product product)
        {
                        try
            {
            
            using (SqlConnection connection = Exigo.Sql())
            {

                string SqlProcedure = string.Format("[dbo].[RetailDiscount] '{0}'", product.ItemCode);
                var retailDiscount = connection.Query<RetailPromo>(SqlProcedure).FirstOrDefault();
                if (retailDiscount == null) return null;

                var discount = new RetailPromo ();

                discount.ItemCode = retailDiscount.ItemCode;
                discount.DiscountType = retailDiscount.DiscountType;
                discount.DiscountAmount = retailDiscount.DiscountAmount;
                discount.BV = retailDiscount.BV;
                discount.CV = retailDiscount.CV;

            
            return discount;
            }
                        }
             catch (Exception ex )
            {              
                return null;
            }
        }

        public static List<RetailPromo> GetRetailPromos()
        {
            try
            {

                using (SqlConnection connection = Exigo.Sql())
                {

                    string SqlProcedure = string.Format("[dbo].[RetailDiscounts]");
                    var retailDiscounts = connection.Query<RetailPromo>(SqlProcedure).ToList();
                    if (retailDiscounts == null) return null;

                    var discounts = new List<RetailPromo>();
                    foreach (var disc in retailDiscounts)
                    {
                        discounts.Add(disc);
                    }

                    return discounts;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}