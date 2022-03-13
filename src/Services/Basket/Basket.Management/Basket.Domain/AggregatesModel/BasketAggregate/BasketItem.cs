using Basket.Management.Basket.Domain.Exceptions;
using Basket.Management.Basket.Domain.SeedWork;

namespace Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate
{
    public class BasketItem : DomainEntity
    {
        private string _itemName;
        private decimal _itemPrice;
        private int _quantity;

        public string ItemId { get; private set; }

        protected BasketItem() { }

        public BasketItem(string itemId, string itemName, decimal itemPrice, int quantity = 1)
        {
            if (quantity <= 0)
            {
                throw new BasketDomainException("Invalid quantity");
            }

            ItemId = itemId;

            _itemName = itemName;
            _itemPrice = itemPrice;
            _quantity = quantity;
        }


        public int GetQuantity()
        {
            return _quantity;
        }

        public decimal GetItemPrice()
        {
            return _itemPrice;
        }

        public string GetBasketItemItemName() => _itemName;

        public void IncreaseQuantity(int quantity)
        {
            if (quantity < 0)
            {
                throw new BasketDomainException("Invalid quantity");
            }

            _quantity += quantity;
        }
    }
}
