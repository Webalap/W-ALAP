using System;
using System.Collections.Generic;
using Common.Api.ExigoWebService;

namespace Common.ModelsEx.Shopping.Discounts
{
    class TestDiscounts
    {
        public void EnsureDiscountTypesAreCreatedProperly()
        {
            var product = new Product
            {
                //Item = new ItemResponse
                //{
                    Price = 10m,
                //},
                Quantity = 1
            };

            var factory = new DiscountFactory();

            var rewardDiscount = factory.CreateDiscount(DiscountType.Fixed, 2m, product);
            var halfOffDiscount = factory.CreateDiscount(DiscountType.Percent, 0.5m, product);
            var twentyFivePercentDiscount = factory.CreateDiscount(DiscountType.Percent, 0.25m, product);

            var total1 = rewardDiscount.Total;
            var total2 = halfOffDiscount.Total;

            product.Discounts = new List<Discount> { rewardDiscount };
            var subtotal = product.SubTotal;
            var total = product.Total;
        }

        public void EnsureAddingProductAppliesDiscount()
        {
            // Arrange
            var party = GenerateEvent();
            var product = new Product
            {
                //Item = new ItemResponse
                //{
                    Price = 99m,
                //},
                Quantity = 1
            };
            var discount = party.HostessCredits[0];

            // Act
            product.AddToOrder(party.Order);
            discount.Apply(
                product,
                () => party.HostessCredits.Remove(discount)
            );

            // Assert
            if (party.HostessCredits.Count != 1)
                throw new Exception("party.HostessCredits should only have one item after applying discount.");
            if (product.Discounts[0] != discount)
                throw new Exception("Where is the discount on the product?");
            if (discount.Product != product)
                throw new Exception("Discount not linked to the product.");
        }

        public void EnsureRemovingProductUnappliesDiscount()
        {
            // Arrange
            var party = GenerateEvent();
            var product = new Product
            {
                //Item = new ItemResponse
                //{
                    Price = 99m,
                //},
                Quantity = 1
            };
            var discount = party.HostessCredits[0];
            product.AddToOrder(party.Order);
            discount.Apply(
                product,
                () => party.HostessCredits.Remove(discount)
            );
            var discountToUnapply = product.Discounts[0];

            // Act
            discountToUnapply.Unapply(
                () => party.HostessCredits.Add(discountToUnapply)
            );

            // Assert
            if (discountToUnapply.Product != null)
                throw new Exception("The unapplied discount should not link to a product!");
            if (product.Discounts.Count != 0)
                throw new Exception("The product should not have any discounts applied to it!");
            if (party.HostessCredits.Count != 2)
                throw new Exception("There should be 2 Hostess Credits!");
        }

        public void EnsureCashRewardsDiscountDoesNotExceedProductPriceWhenApplied()
        {
            // Arrange
            var party = GenerateEvent();
            var product = new Product
            {
                //Item = new ItemResponse
                //{
                //    Price = 2m
                //},
                Price = 2m,
                Quantity = 1
            };
            var discount = party.HostessCredits[0];

            // Act
            product.AddToOrder(party.Order);

            // TODO: if (DiscountValidator.IsValidFor(discount, product))

            var discountToAdd = discount.Subtract(product.Price);

            discountToAdd.Apply(
                product,
                null // The discount was already subtracted
            );

            // end if

            // Assert
            if (product.Discounts.Count != 1 || product.Discounts[0].DiscountAmount != 2m)
                throw new Exception("The product should have a $2 discount applied to it.");
            if (party.HostessCredits.Count != 2 || party.HostessCredits[0].DiscountAmount != 3m)
                throw new Exception("The first cash reward discount should be $3.");
        }

        public void EnsureDiscountRemainsAppliedIfProductQuantityDecreases()
        {
            // Arrange
            var party = GenerateEvent();
            var product = new Product
            {
                //Item = new ItemResponse
                //{
                //    Price = 99m
                //},
                Price = 99m,
                Quantity = 2
            };
            var discount = party.HostessRewards[0];
            product.AddToOrder(party.Order);
            discount.Apply(
                product,
                () => party.HostessRewards.Remove(discount)
            );

            // Act
            // TODO: George, I think it will make life easier in the future
            // if you refactor Product.RemoveFromOrder to perform
            // the logic behind unapplying discounts when Quantity == 0.
            // e.g.
            //product.RemoveFromOrder(
            //    party.Order,
            //    removeAllQty: false,
            //    followUpAction: () => party.HostessCredits.Add(discountToUnapply)
            //);
            product.RemoveFromOrder(party.Order);

            // Assert
            if (product.Discounts.Count != 1)
                throw new Exception("The product should still have the discount applied to it.");
        }

        private MockEvent GenerateEvent()
        {
            return new MockEvent
            {
                EventName = "My Party",
                HostessCredits = new List<Discount>
                {
                    _discountFactory.CreateDiscount(DiscountType.Fixed, 5m, null),
                    _discountFactory.CreateDiscount(DiscountType.Fixed, 5m, null),
                },
                HostessRewards = new List<Discount>
                {
                    _discountFactory.CreateDiscount(DiscountType.Percent, 0.5m, null),
                },
                Order = new ShoppingCart()
            };
        }

        private readonly DiscountFactory _discountFactory = new DiscountFactory();

        class MockEvent
        {
            public string EventName { get; set; }
            public List<Discount> HostessCredits { get; set; }
            public List<Discount> HostessRewards { get; set; }
            public IOrder Order { get; set; }
        }
    }
}