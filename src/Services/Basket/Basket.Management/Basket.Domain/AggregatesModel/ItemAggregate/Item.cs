using Basket.Management.Basket.Domain.SeedWork;

namespace Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate
{
    public class Item : DomainEntity, IAggregateRoot
    {
        private string _name;
        private decimal _price;
        private string _itemId;

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public decimal Price
        {
            get
            {
                return _price;
            }
        }

        public string ItemId
        {
            get
            {
                return _itemId;
            }
        }

        public Item(string name, string id, decimal price)
        {
            _name = name;
            _price = price;
            _itemId = id;
        }
    }
}
