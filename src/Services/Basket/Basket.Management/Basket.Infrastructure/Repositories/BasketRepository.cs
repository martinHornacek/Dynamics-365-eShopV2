using System;
using Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate;

namespace Basket.Management.Basket.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {
        private readonly BasketContext _context;

        public BasketRepository(BasketContext context)
        {
            _context = context;
        }

        public void Add(Domain.AggregatesModel.BasketAggregate.Basket basket)
        {
            _context.AddObject(basket);
        }

        public void Update(Domain.AggregatesModel.BasketAggregate.Basket basket)
        {
            _context.UpdateObject(basket);
        }

        public void Delete(Domain.AggregatesModel.BasketAggregate.Basket basket)
        {
            _context.DeleteObject(basket);
        }

        public Domain.AggregatesModel.BasketAggregate.Basket GetById(Guid basketId)
        {
            // TODO
            //var basket = from b in _context.BasketSet
            //             where b.Id == basketId
            //             select b;
            //return basket;
            return Domain.AggregatesModel.BasketAggregate.Basket.New();
        }

    }
}
