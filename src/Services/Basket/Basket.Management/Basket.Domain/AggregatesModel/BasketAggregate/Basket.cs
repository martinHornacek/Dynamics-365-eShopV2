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

        public (bool, BasketItem) AddBasketItem(Item item, int quantity)
        {
            var existingBasketItem = _basketItems.Where(bi => bi.Item.ItemId == item.ItemId).SingleOrDefault();

            if (existingBasketItem != null)
            {
                existingBasketItem.Quantity = quantity;
                this.RecalculateTotalValue();
                return (true, existingBasketItem);
            }
            else
            {
                var basketItem = new BasketItem(item, quantity);
                _basketItems.Add(basketItem);
                this.RecalculateTotalValue();
                return (false, basketItem);
            }
        }

        public BasketItem RemoveBasketItem(string itemId)
        {
            var existingBasketItem = _basketItems.Where(bi => bi.Item.ItemId == itemId).SingleOrDefault();

            if (existingBasketItem != null)
            {
                _basketItems.Remove(existingBasketItem);
                this.RecalculateTotalValue();
            }

            return existingBasketItem;
        }

        public void RecalculateTotalValue()
        {
            TotalValue = _basketItems.Sum(bi => bi.Quantity * bi.Price);
        }

        public override bool Equals(object obj)
        {
            // If the passed object is null
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Basket))
            {
                return false;
            }
            return (this.Name == ((Basket)obj).Name);
        }
        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }
    }
}
