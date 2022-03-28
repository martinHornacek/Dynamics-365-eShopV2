using Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate;
using Basket.Management.Basket.Domain.Exceptions;
using Basket.Management.Basket.Domain.SeedWork;

namespace Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate
{
    public class BasketItem : DomainEntity
    {
        private readonly Item _item;
        private int _quantity;

        public BasketItem(Item item, int quantity)
        {
            if (quantity <= 0) throw new BasketDomainException("Invalid quantity");

            _item = item;
            _quantity = quantity;
        }

        public decimal Price => Item.Price;

        public Item Item => _item;

        public int Quantity
        {
            get
            {
                return _quantity;
            }
            set
            {
                if (value < 0) throw new BasketDomainException("Invalid quantity");
                _quantity = value;
            }
        }
    }
}
