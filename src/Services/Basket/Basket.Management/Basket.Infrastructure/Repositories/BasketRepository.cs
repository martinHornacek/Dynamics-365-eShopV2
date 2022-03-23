using System;
using System.Linq;
using Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate;
using CrmEarlyBound;

namespace Basket.Management.Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketContext _context;

        public BasketRepository(BasketContext context)
        {
            _context = context;
        }

        public void Update(Domain.AggregatesModel.BasketAggregate.Basket basket)
        {
            _context.UpdateObject(basket.new_basket);
        }

        public Domain.AggregatesModel.BasketAggregate.Basket GetById(Guid basketId)
        {
              var basket = _context.Baskets
                                 .Where(b => b.new_basketId == basketId)
                                 .Select(b => new new_basket { new_basketId = b.Id })
                                 .First();

            
            var basketItems = _context.BasketItems
                                      .Where(bi => bi.new_basket.Id == basketId)
                                      .Select(bi => new new_basketitem
                                       {
                                           new_basketitemId = bi.new_basketitemId,
                                           new_basketid = bi.new_basketid,
                                           new_itemid = bi.new_itemid,
                                           new_quantity = bi.new_quantity,
                                           new_item = bi.new_item
                                       }).ToList();

            var items = _context.Items
                                .Select(i => new new_item { new_itemId = i.new_itemId, new_price = i.new_price })
                                .ToList();

            return new Domain.AggregatesModel.BasketAggregate.Basket(basket, basketItems, items);
        }
    }
}
