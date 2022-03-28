using CrmEarlyBound;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Management.Basket.Infrastructure.Mappers
{
    public static class BasketMapper
    {
        private static readonly Dictionary<Domain.AggregatesModel.BasketAggregate.Basket, new_basket> _mappingDictionary;

        static BasketMapper()
        {
            _mappingDictionary = new Dictionary<Domain.AggregatesModel.BasketAggregate.Basket, new_basket>();
        }

        public static Domain.AggregatesModel.BasketAggregate.Basket ToBasket(new_basket new_basket, List<new_basketitem> new_basketitems, List<new_item> new_items)
        {
            var basketItems = new_basketitems.Select(bi => BasketItemMapper.ToBasketItem(bi, new_items)).ToList();
            var basket =  new Domain.AggregatesModel.BasketAggregate.Basket(new_basket.new_name, basketItems);

            _mappingDictionary.Add(basket, new_basket);

            return basket;
        }

        public static new_basket Tonew_basket(Domain.AggregatesModel.BasketAggregate.Basket basket)
        {
            var found = _mappingDictionary.TryGetValue(basket, out new_basket new_basket);
            if (!found) throw new InvalidOperationException(nameof(_mappingDictionary));

            new_basket.new_totalvalue = basket.TotalValue;
            return new_basket;
        }
    }

    public static class BasketItemMapper
    {
        public static Domain.AggregatesModel.BasketAggregate.BasketItem ToBasketItem(new_basketitem new_basketitem, List<new_item> new_items)
        {
            var new_item = new_items.FirstOrDefault(i => i.new_id == new_basketitem.new_itemid);
            var item = ItemMapper.ToItem(new_item);

            return new Domain.AggregatesModel.BasketAggregate.BasketItem(item, new_basketitem.new_quantity ?? 0);
        }

        public static new_basketitem Tonew_basketitem(Domain.AggregatesModel.BasketAggregate.BasketItem basketItem)
        {
            return new new_basketitem
            {
                new_quantity = basketItem.Quantity,
                new_itemid = basketItem.Item.ItemId
            };
        }
    }
}
