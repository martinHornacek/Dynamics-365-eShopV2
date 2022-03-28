using Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate;
using Basket.Management.Basket.Infrastructure.Contexts;
using Basket.Management.Basket.Infrastructure.Mappers;
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

        public Item GetById(Guid id)
        {
            var itemQuery = from i in _context.Items
                            where i.Id == id
                            select new new_item
                            {
                                new_itemId = i.new_itemId,
                                new_id = i.new_id,
                                new_name = i.new_name,
                                new_price = i.new_price,
                            };
                       
            var new_item = itemQuery.FirstOrDefault();
            return ItemMapper.ToItem(new_item);
        }

        public Item GetByItemId(string new_id)
        {
            var itemQuery = from i in _context.Items
                            where i.new_id == new_id
                            select new new_item
                            {
                                new_itemId = i.new_itemId,
                                new_id = i.new_id,
                                new_name = i.new_name,
                                new_price = i.new_price,
                            };

            var new_item = itemQuery.FirstOrDefault();
            return ItemMapper.ToItem(new_item);
        }

        public bool SaveEntities()
        {
            var result = _context.SaveChanges();
            return true;
        }
    }
}
