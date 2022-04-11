using Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate;
using Basket.Management.Basket.Infrastructure.Contexts;
using Basket.Management.Basket.Infrastructure.Mappers;
using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using System;
using System.Linq;

namespace Basket.Management.Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketContext _context;
        private readonly BasketMapper _basketMapper;
        private readonly BasketItemMapper _basketItemMapper;
        private readonly ItemMapper _itemMapper;

        public BasketRepository(BasketContext context, BasketMapper basketMapper, BasketItemMapper basketItemMapper, ItemMapper itemMapper)
        {
            _context = context;
            _basketMapper = basketMapper;
            _basketItemMapper = basketItemMapper;
            _itemMapper = itemMapper;
        }

        public void Update(Domain.AggregatesModel.BasketAggregate.Basket basket)
        {
            var new_basket = _basketMapper.Tonew_basket(basket);
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

            return _basketMapper.ToBasket(new_basket, new_basketitems, new_items);
        }

        public void DeleteBasketItem(BasketItem basketItem)
        {
            var new_basketitem = _context.BasketItems
                           .Where(bi => bi.new_itemid == basketItem.Item.ItemId)
                           .Select(bi => new new_basketitem
                           {
                               new_basketitemId = bi.new_basketitemId,
                               new_basketid = bi.new_basketid,
                               new_itemid = bi.new_itemid,
                               new_quantity = bi.new_quantity,
                               new_item = bi.new_item
                           }).FirstOrDefault();

            _context.DeleteObject(new_basketitem);
        }

        public void UpdateBasketItem(BasketItem basketItem)
        {
            var new_basketitem = _context.BasketItems
                          .Where(bi => bi.new_itemid == basketItem.Item.ItemId)
                          .Select(bi => new new_basketitem
                          {
                              new_basketitemId = bi.new_basketitemId,
                              new_basketid = bi.new_basketid,
                              new_itemid = bi.new_itemid,
                              new_quantity = bi.new_quantity,
                              new_item = bi.new_item
                          }).FirstOrDefault();

            new_basketitem.new_Id = basketItem.Item.ItemId;
            new_basketitem.new_itemid = basketItem.Item.ItemId;
            new_basketitem.new_name = basketItem.Item.Name;
            new_basketitem.new_quantity = basketItem.Quantity;

            _context.UpdateObject(new_basketitem);
        }

        public void CreateBasketItem(Domain.AggregatesModel.BasketAggregate.Basket basket, BasketItem basketItem)
        {
            var new_basketitem = _basketItemMapper.Tonew_basketitem(basketItem);
            var new_basket = _basketMapper.Tonew_basket(basket);
            var new_item = _itemMapper.Tonew_item(basketItem.Item);

            new_basketitem.new_basketid = new_basket.new_id;
            new_basketitem.new_Id = basketItem.Item.ItemId;
            new_basketitem.new_itemid = basketItem.Item.ItemId;
            new_basketitem.new_name = basketItem.Item.Name;

            _context.AddObject(new_basketitem);
            _context.AddLink(new_basketitem, new Relationship("new_new_basket_new_basketitem_basketid"), new_basket);
            _context.AddLink(new_item, new Relationship("new_new_item_new_basketitem_itemid"), new_basketitem);
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

            return _basketMapper.ToBasket(new_basket, new_basketitems, new_items);
        }

        public bool SaveEntities()
        {
            var result = _context.SaveChanges();
            return true;
        }
    }
}
