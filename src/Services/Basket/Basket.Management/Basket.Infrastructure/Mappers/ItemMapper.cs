using Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate;
using CrmEarlyBound;

namespace Basket.Management.Basket.Infrastructure.Mappers
{
    public static class ItemMapper
    {
        public static Item ToItem(new_item new_item)
        {
            return new Item(new_item.new_id, new_item.new_name, new_item.new_price ?? 0m);
        }

        public static new_item Tonew_item(Item item)
        {
            return new new_item 
            {
                new_id = item.ItemId,
                new_name = item.Name,
                new_price = item.Price
            };
        }
    }
}
