using Basket.Management.Basket.Domain.Exceptions;
using Basket.Management.Basket.Domain.SeedWork;

namespace Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate
{
    public class BasketItem : DomainEntity
    {
        private readonly decimal _price;
        private readonly string _itemId;
        private int _quantity;

        public BasketItem(string itemId, decimal price, int quantity)
        {
            if (quantity <= 0)
            {
                throw new BasketDomainException("Invalid quantity");
            }

            _price = price;
            _itemId = itemId;
            _quantity = quantity;
        }

        public decimal GetItemPrice() => _price;

        public string GetItemId() => _itemId;
        
        public int GetQuantity() => _quantity;

        public void UpdateQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new BasketDomainException("Invalid quantity");
            }

            _quantity = quantity;
        }
    }
}
