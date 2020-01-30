using Common.Api.ExigoOData.Rewards;
using Common.ModelsEx.Reward;
using Common.ModelsEx.SavedCart;
using Common.ModelsEx.Shopping.Discounts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace ExigoService
{
    public static partial class Exigo
    {
        public static string GetSavedCartByCartID(int cartID)
        {
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetSpecificSavedCart {0}", cartID);
                return contextsql.Query<SavedCart>(sql).FirstOrDefault().PropertyBag;
            }
        }

        public static BasePropertyBag GetSavedCartByCartID(int cartID, string shoppingCartName)
        {
            var Datalist = new List<SavedCart>();
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetSpecificSavedCart {0}", cartID);
                Datalist = contextsql.Query<SavedCart>(sql).ToList();
            }
            IList<BasePropertyBag> lstPropertyBag = new List<BasePropertyBag>();
            foreach (var item in Datalist)
            {
                if (shoppingCartName.Contains("BackofficeShopping"))
                    lstPropertyBag.Add(
                        JsonConvert.DeserializeObject<PersonalShopppingCart>(
                        item.PropertyBag
                        )
                    );
                if (shoppingCartName.Contains("RetailCustomerOrderPropertyBag"))
                    lstPropertyBag.Add(
                        JsonConvert.DeserializeObject<RetailCustomerOrderPropertyBag>(
                        item.PropertyBag
                        )
                    );
                if (shoppingCartName.Contains("EventCustomerOrderPropertyBag"))
                    lstPropertyBag.Add(
                        JsonConvert.DeserializeObject<EventCustomerOrderPropertyBag>(
                        item.PropertyBag
                        )
                    );
            }
            return lstPropertyBag.FirstOrDefault();
        }

        public static DisplaySaveCart GetSpecificSavedCart(int cartID)
        {
            DisplaySaveCart Datalist = new DisplaySaveCart();
            using (var contextsql = Exigo.Sql())
            {
                var sql = string.Format(@"Exec GetSpecificSavedCart {0}", cartID);
                Datalist = contextsql.Query<DisplaySaveCart>(sql).FirstOrDefault();
            }
            return Datalist;
        }

        public static string GenratePromoCode(string email)
        {
            string promoCode = RandomString(6);
            try
            {
                while (GetPromoCodes().Where(i => i.Code == (promoCode)).FirstOrDefault() != null)
                {
                    promoCode = RandomString(6);
                }
                InsertPromoCode(new PromoCode { Code = promoCode, DiscountAmount = 30, Email =email, DiscountType = (int)DiscountType.PromoCode, EligibleCount = int.Parse(ConfigurationManager.AppSettings["SpecificPromoCodeCount"].ToString()), EndDate = DateTime.Now.AddYears(5),
                     StartDate = DateTime.Now
                });
            }
            catch (Exception e) { }
            return promoCode;
        }
        public static List<PromoCode> GetPromoCodes()
        {
            List<PromoCode> lst = new List<PromoCode>();
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("GetPromoCodes");
                lst = context.Query<PromoCode>(SqlProcedure).ToList();
            }
            return lst;
        }

        public static bool InsertPromoCode(PromoCode promoCode)
        {
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("InsertPromoCodes '{0}','{1}','{2}',{3},{4},'{5}',{6},{7}",
                   promoCode.Code, promoCode.StartDate, promoCode.EndDate, promoCode.DiscountType, 30, promoCode.Email,promoCode.UsedCount ,promoCode.EligibleCount);
                var styleAmbassadorRewardSettings = context.Query<PromoCode>(SqlProcedure).FirstOrDefault();
                return true;
            }
        }


        public static bool AddToGiftPromos(GiftPromo product)
        {
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("InsertGiftPromo '{0}',{1},{2},'{3}','{4}','{5}',{6},'{7}'",
                   product.ItemCode, product.DiscountPrecent, product.SalesThreshold, product.StartDate, product.EndDate, product.Description.Replace("'", "''"), product.DiscountType, product.CreatedDate);
                var styleAmbassadorRewardSettings = context.Query<PromoCode>(SqlProcedure).FirstOrDefault();
                return true;
            }
        }

        public static List<GiftPromo> GetGiftPromos()
        {
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("GetGiftPromos");
                return context.Query<GiftPromo>(SqlProcedure).ToList();
            }
        }
        public static string RandomString(int length)
        {
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static List<AnnouncementBanner> GetAnnouncementBanners()
        {
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("GetAnnouncementBanners");
                return context.Query<AnnouncementBanner>(SqlProcedure).ToList();
            }
        }

        public static List<Charity> GetCharities()
        {
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("GetCharities");
                return context.Query<Charity>(SqlProcedure).ToList();
            }
        }

        public static bool AddToCharity(Charity Charity)
        {
            using (var context = Exigo.Sql())
            {
                var SqlProcedure = string.Format("InsertCharity '{0}','{1}','{2}','{3}','{4}','{5}'",
                   Charity.EIN, Charity.Name.Replace("'", "''"), Charity.City.Replace("'", "''"), Charity.State, Charity.Country, Charity.Deductibility);
                var styleAmbassadorRewardSettings = context.Query<PromoCode>(SqlProcedure).FirstOrDefault();
                return true;
            }
        }
    }
}