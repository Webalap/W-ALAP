using Common.ModelsEx.Reward;
using Common.ModelsEx.Shopping;
using Common.ModelsEx.Shopping.Discounts;
using Common.ServicesEx.Rewards;
using System;
using System.Collections.Generic;

namespace Common.ServicesEx
{
    public interface IRewardService
    {
        HostSpecialDiscount GetHostSpecialReward(DateTime eventDate);
        HostSpecialDiscount GetHostSpecialReward(int eventId);
        IList<IIndividualReward> GetActiveRewards(string siteType, int customerId);
        IList<Product> GetRewardProducts(IList<IIndividualReward> rewards, IList<Product> productsInShoppingCart);
		RewardPhase GetPhaseStatus(int customerId, int phaseNum);
        IList<EBRewardDiscount> GetCustomerEbRewardDiscounts(int customerId);
        int CreateRewardsAutoOrder(string itemId, int customerId, int ExtendedGroupID, int phase);

    }
}
