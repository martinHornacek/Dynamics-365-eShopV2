using Basket.Management.Basket.Domain.SeedWork;
using System;

namespace Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate
{
    public interface IBasketRepository : IRepository<Basket>
    {
        void Update(Basket basket);

        Basket GetById(Guid id);

        Basket GetByBasketId(string new_id);

        bool SaveEntities();
    }
}
