using Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate;
using Basket.Management.Basket.Domain.SeedWork;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate
{
    public class Basket : DomainEntity, IAggregateRoot
    {
        private readonly List<BasketItem> _basketItems = new List<BasketItem>();
        public IReadOnlyCollection<BasketItem> BasketItems => _basketItems;

        public decimal TotalValue { get; set; }
        public string Name { get; }

        public Basket(string name, List<BasketItem> basketItems)
        {
            Name = name;
            _basketItems.AddRange(basketItems);
        }

        public void AddBasketItem(Item item, int quantity)
        {
            var existingBasketItem = _basketItems.Where(bi => bi.Item.ItemId == item.ItemId).SingleOrDefault();

            if (existingBasketItem != null)
            {
                existingBasketItem.Quantity = quantity;
                this.RecalculateTotalValue();
            }
            else
            {
                _basketItems.Add(new BasketItem(item, quantity));
                this.RecalculateTotalValue();
            }
        }

        public void RemoveBasketItem(string itemId)
        {
            var existingBasketItem = _basketItems.Where(bi => bi.Item.ItemId == itemId).SingleOrDefault();

            if (existingBasketItem != null)
            {
                _basketItems.Remove(existingBasketItem);
                this.RecalculateTotalValue();
            }
        }

        public void RecalculateTotalValue()
        {
            TotalValue = _basketItems.Sum(bi => bi.Quantity * bi.Price);
        }
    }
}
