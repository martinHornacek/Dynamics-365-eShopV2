using CrmEarlyBound;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Basket.Management.Basket.Infrastructure.Mappers
{
    public class BasketMapper
    {
        private readonly Dictionary<Domain.AggregatesModel.BasketAggregate.Basket, new_basket> _mappingDictionary;
        private readonly BasketItemMapper _basketItemMapper;

        public BasketMapper(BasketItemMapper basketItemMapper)
        {
            _mappingDictionary = new Dictionary<Domain.AggregatesModel.BasketAggregate.Basket, new_basket>();
            _basketItemMapper = basketItemMapper;
        }

        public Domain.AggregatesModel.BasketAggregate.Basket ToBasket(new_basket new_basket, List<new_basketitem> new_basketitems, List<new_item> new_items)
        {
            var basketItems = new_basketitems.Select(bi => _basketItemMapper.ToBasketItem(bi, new_items)).ToList();
            var basket =  new Domain.AggregatesModel.BasketAggregate.Basket(new_basket.new_name, basketItems);

            _mappingDictionary.Add(basket, new_basket);

            return basket;
        }

        public new_basket Tonew_basket(Domain.AggregatesModel.BasketAggregate.Basket basket)
        {
            var found = _mappingDictionary.TryGetValue(basket, out new_basket new_basket);
            if (!found) throw new InvalidOperationException(nameof(_mappingDictionary));

            new_basket.new_totalvalue = basket.TotalValue;

            return new_basket;
        }
    }
}
