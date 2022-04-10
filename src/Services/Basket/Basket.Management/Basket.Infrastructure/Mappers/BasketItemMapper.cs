using CrmEarlyBound;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Management.Basket.Infrastructure.Mappers
{
    public class BasketItemMapper
    {
        private readonly Dictionary<Domain.AggregatesModel.BasketAggregate.BasketItem, new_basketitem> _mappingDictionary;
        private readonly ItemMapper _itemMapper;

        public BasketItemMapper(ItemMapper itemMapper)
        {
            _mappingDictionary = new Dictionary<Domain.AggregatesModel.BasketAggregate.BasketItem, new_basketitem>();
            _itemMapper = itemMapper;
        }

        public Domain.AggregatesModel.BasketAggregate.BasketItem ToBasketItem(new_basketitem new_basketitem, List<new_item> new_items)
        {
            var new_item = new_items.FirstOrDefault(i => i.new_id == new_basketitem.new_itemid);
            var item = _itemMapper.ToItem(new_item);

            var basketItem = new Domain.AggregatesModel.BasketAggregate.BasketItem(item, new_basketitem.new_quantity ?? 0);
            _mappingDictionary.Add(basketItem, new_basketitem);

            return basketItem;
        }

        public new_basketitem Tonew_basketitem(Domain.AggregatesModel.BasketAggregate.BasketItem basketItem)
        {
            var found = _mappingDictionary.TryGetValue(basketItem, out new_basketitem new_basketitem);
            if (!found)
            {
                new_basketitem = new new_basketitem();
            }

            new_basketitem.new_quantity = basketItem.Quantity;
            new_basketitem.new_itemid = basketItem.Item.ItemId;

            return new_basketitem;
        }
    }
}
