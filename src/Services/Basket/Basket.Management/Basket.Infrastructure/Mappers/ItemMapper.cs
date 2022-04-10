using Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate;
using CrmEarlyBound;
using System;
using System.Collections.Generic;

namespace Basket.Management.Basket.Infrastructure.Mappers
{
    public class ItemMapper
    {
        private readonly Dictionary<Item, new_item> _mappingDictionary;

        public ItemMapper()
        {
            _mappingDictionary = new Dictionary<Item, new_item>();
        }

        public Item ToItem(new_item new_item)
        {
            var item = new Item(new_item.new_id, new_item.new_name, new_item.new_price ?? 0m);
            _mappingDictionary.Add(item, new_item);
            return item;
        }

        public new_item Tonew_item(Item item)
        {
            var found = _mappingDictionary.TryGetValue(item, out new_item new_item);
            if (!found) throw new InvalidOperationException(nameof(_mappingDictionary));

            new_item.new_id = item.ItemId;
            new_item.new_name = item.Name;
            new_item.new_price = item.Price;
            return new_item;
        }
    }
}
