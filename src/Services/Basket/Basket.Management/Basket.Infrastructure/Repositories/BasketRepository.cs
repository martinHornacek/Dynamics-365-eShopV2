using System;
using System.Linq;
using Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate;
using Basket.Management.Basket.Infrastructure.Contexts;
using Basket.Management.Basket.Infrastructure.Mappers;
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
            var new_basket = BasketMapper.Tonew_basket(basket);
            // TODO handle basket item changes as well
            _context.UpdateObject(new_basket);
        }

        public Domain.AggregatesModel.BasketAggregate.Basket GetById(Guid id)
        {
            var new_basket = _context.Baskets
                               .Where(b => b.new_basketId == id)
                               .Select(b => new new_basket { new_basketId = b.Id })
                               .First();


            var new_basketitems = _context.BasketItems
                                      .Where(bi => bi.new_basket.Id == id)
                                      .Select(bi => new new_basketitem
                                      {
                                          new_basketitemId = bi.new_basketitemId,
                                          new_basketid = bi.new_basketid,
                                          new_itemid = bi.new_itemid,
                                          new_quantity = bi.new_quantity,
                                          new_item = bi.new_item
                                      }).ToList();

            var new_items = _context.Items
                                .Select(i => new new_item
                                {
                                    new_itemId = i.new_itemId,
                                    new_id = i.new_id,
                                    new_name = i.new_name,
                                    new_price = i.new_price
                                }).ToList();



            return BasketMapper.ToBasket(new_basket, new_basketitems, new_items);
        }

        public Domain.AggregatesModel.BasketAggregate.Basket GetByBasketId(string new_id)
        {
            var new_basket = _context.Baskets
                                .Where(b => b.new_id == new_id)
                                .Select(b => new new_basket { new_basketId = b.Id })
                                .First();

            var new_basketitems = _context.BasketItems
                                      .Where(bi => bi.new_basket.Id == new_basket.Id)
                                      .Select(bi => new new_basketitem
                                      {
                                          new_basketitemId = bi.new_basketitemId,
                                          new_basketid = bi.new_basketid,
                                          new_itemid = bi.new_itemid,
                                          new_quantity = bi.new_quantity,
                                          new_item = bi.new_item
                                      }).ToList();

            var new_items = _context.Items
                                .Select(i => new new_item
                                {
                                    new_itemId = i.new_itemId,
                                    new_id = i.new_id,
                                    new_name = i.new_name,
                                    new_price = i.new_price
                                })
                                .ToList();

            return BasketMapper.ToBasket(new_basket, new_basketitems, new_items);
        }

        public bool SaveEntities()
        {
            var result = _context.SaveChanges();
            return true;
        }
    }
}
