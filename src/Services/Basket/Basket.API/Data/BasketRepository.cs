using System;
using System.Collections.Generic;
using System.Linq;
using Basket.API.Model;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketContext _context;

        public BasketRepository(BasketContext context)
        {
            _context = context;
        }

        public void AddBasketItem(int basketId, BasketItem basketItem)
        {
            if (basketItem == null)
            {
                throw new ArgumentNullException(nameof(basketItem));
            }

            _context.BasketItems.Add(basketItem);
        }

        public void CreateBasket(Model.Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }

            _context.Baskets.Add(basket);
        }

        public void EmptyBasket(int basketId)
        {
            foreach (var id in _context.BasketItems.Select(bi => bi.Id))
            {
                var basketItem = new BasketItem { Id = id };
                _context.BasketItems.Attach(basketItem);
                _context.BasketItems.Remove(basketItem);
            }
        }

        public IEnumerable<BasketItem> GetAllBasketItemsForBasket(int basketId)
        {
            return _context.BasketItems.Where(bi => bi.BasketId == basketId).ToList();
        }

        public BasketItem GetBasketItemForBasket(int basketId, int basketItemId)
        {
            return _context.BasketItems.Where(bi => bi.BasketId == basketId && bi.Id == basketItemId).FirstOrDefault();
        }

        public IEnumerable<Model.Basket> GetAllBaskets()
        {
            return _context.Baskets.ToList();
        }

        public Model.Basket GetBasketById(int basketId)
        {
            return _context.Baskets.FirstOrDefault(b => b.Id == basketId);
        }

        public void RemoveBasketItem(int basketId, int basketItemId)
        {
            var basketItem = _context.BasketItems.SingleOrDefault(bi => bi.BasketId == basketId && bi.Id == basketItemId);
            if (basketItem != null)
            {
                _context.BasketItems.Attach(basketItem);
                _context.BasketItems.Remove(basketItem);
            }
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}
