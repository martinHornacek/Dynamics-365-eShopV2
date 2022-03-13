using System.Collections.Generic;
using System.Linq;
using Basket.Management.Basket.Domain.SeedWork;

namespace Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate
{
    public class Basket : DomainEntity, IAggregateRoot
    {
        private readonly string _name;
        private readonly string _description;

        private readonly List<BasketItem> _basketItems;
        public IReadOnlyCollection<BasketItem> BasketItems => _basketItems;

        public static Basket New()
        {
            var basket = new Basket();
            return basket;
        }

        protected Basket()
        {
            _basketItems = new List<BasketItem>();
        }

        public Basket(string name, string description) : this()
        {
            _name = name;
            _description = description;
        }

        // This Basket AggregateRoot's method "AddBasketItem()" should be the only way to add Items to the Basket,
        // so any behavior (discounts, etc.) and validations are controlled by the AggregateRoot 
        // in order to maintain consistency between the whole Aggregate. 
        public void AddBasketItem(string itemId, string itemName, decimal price, int quantity = 1)
        {
            var existingBasketItem = _basketItems.Where(o => o.ItemId == itemId).SingleOrDefault();

            if (existingBasketItem != null)
            {
                //if previous item exist modify it with quanity..
                existingBasketItem.IncreaseQuantity(quantity);
            }
            else
            {
                //add validated new basket item
                var basketItem = new BasketItem(itemId, itemName, price, quantity);
                _basketItems.Add(basketItem);
            }
        }

        public decimal GetTotalValue()
        {
            return _basketItems.Sum(bi => bi.GetQuantity() * bi.GetItemPrice());
        }
    }
}
