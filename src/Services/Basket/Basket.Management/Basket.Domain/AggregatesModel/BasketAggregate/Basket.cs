using Basket.Management.Basket.Domain.SeedWork;
using CrmEarlyBound;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate
{
    public class Basket : DomainEntity, IAggregateRoot
    {
        public new_basket new_basket { get; }
        private List<new_basketitem> _new_basketitems { get; }
        private List<new_item> _new_items { get; }

        private readonly List<BasketItem> _basketItems = new List<BasketItem>();
        public IReadOnlyCollection<BasketItem> BasketItems => _basketItems;

        private decimal _totalValue;
        public decimal TotalValue
        {
            get
            {
                return _totalValue;
            }
            set
            {
                _totalValue = value;
                this.new_basket.new_totalvalue = value;
            }
        }

        public Basket(new_basket new_basket, List<new_basketitem> basketItems, List<new_item> items)
        {
            this.new_basket = new_basket;
            this._new_basketitems = basketItems;
            this._new_items = items;

            foreach (var bi in basketItems)
            {
                var item = items.FirstOrDefault(i => bi.new_item.Id == i.Id);
                _basketItems.Add(new BasketItem(bi.new_itemid, item.new_price ?? 0m, bi.new_quantity ?? 0));
            }
        }

        // This Basket AggregateRoot's method "AddBasketItem()" should be the only way to add Items to the Basket,
        // so any behavior (discounts, etc.) and validations are controlled by the AggregateRoot 
        // in order to maintain consistency between the whole Aggregate. 
        public void AddBasketItem(string itemId, decimal price, int quantity)
        {
            var existingBasketItem = _basketItems.Where(o => o.GetItemId() == itemId).SingleOrDefault();

            if (existingBasketItem != null)
            {
                existingBasketItem.UpdateQuantity(quantity); //if previous item exist modify it with quanity..
                this.RecalculateTotalValue();
            }
            else
            {
                //add validated new basket item
                _basketItems.Add(new BasketItem(itemId, price, quantity));
                this.RecalculateTotalValue();
            }
        }

        public void RemoveBasketItem(string itemId)
        {
            var existingBasketItem = _basketItems.Where(o => o.GetItemId() == itemId).SingleOrDefault();

            if (existingBasketItem != null)
            {
                _basketItems.Remove(existingBasketItem);
                this.RecalculateTotalValue();
            }
        }

        private void RecalculateTotalValue()
        {
            TotalValue = _basketItems.Sum(bi => bi.GetQuantity() * bi.GetItemPrice());
        }
    }
}
