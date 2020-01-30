using Common.Api.ExigoOData.Rewards;
using ExigoService;
using System;
using System.Collections.Generic;
using System.Linq;
//using Common.Api.ExigoOData.Rewards;

namespace Common.Services
{

    public class EventsPromotion
    {
        public static List<GiftPromo> GetPromotionGifts(int discountType)
        {
            var giftPromo = new List<GiftPromo>();
            try
            {
                using (var context = Exigo.Sql())
                {

                    var SqlProcedure = string.Format("GetPromotionGifts {0}", discountType);
                    giftPromo = context.Query<GiftPromo>(SqlProcedure).ToList();
                }
            }
            catch (Exception)
            {
                // ignored
            }
            return giftPromo;
        }

        public static bool GiftPromosProductInsert(GiftPromo product)
        {
            return Exigo.AddToGiftPromos(product);
        }


    }
}