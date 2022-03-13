using System;
using Basket.Management.Basket.Domain.SeedWork;

namespace Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate
{
    public interface IBasketRepository : IRepository<Basket>
    {
        void Add(Basket basket);

        void Update(Basket basket);

        void Delete(Basket basket);

        Basket GetById(Guid basketId);
    }
}
