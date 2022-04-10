using Basket.Management.Basket.Domain.SeedWork;

namespace Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate
{
    public class Item : DomainEntity, IAggregateRoot
    {
        private readonly string _name;
        private readonly decimal _price;
        private readonly string _itemId;

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

        public Item(string id, string name, decimal price)
        {
            _itemId = id;
            _name = name;
            _price = price;
        }

        public override bool Equals(object obj)
        {
            // If the passed object is null
            if (obj == null)
            {
                return false;
            }
            if (!(obj is Item))
            {
                return false;
            }
            return (this.ItemId == ((Item)obj).ItemId);
        }
        public override int GetHashCode()
        {
            return ItemId.GetHashCode();
        }
    }
}
