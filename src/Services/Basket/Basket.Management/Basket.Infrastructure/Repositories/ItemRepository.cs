using Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate;
using CrmEarlyBound;
using System;
using System.Linq;

namespace Basket.Management.Basket.Infrastructure.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly ItemContext _context;

        public ItemRepository(ItemContext context)
        {
            _context = context;
        }

        public Item GetById(Guid itemId)
        {
            var itemQuery = from i in _context.Items
                            where i.Id == itemId
                            select new new_item
                            {
                                new_itemId = i.new_itemId,
                                new_name = i.new_name,
                                new_price = i.new_price,
                                new_id = i.new_id
                            };
                       
            var item = itemQuery.FirstOrDefault();

            return new Item(item.new_name, item.new_id, item.new_price ?? 0m);
        }
    }
}
