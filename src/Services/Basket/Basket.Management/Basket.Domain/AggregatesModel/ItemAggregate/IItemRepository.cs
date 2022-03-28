using Basket.Management.Basket.Domain.SeedWork;
using System;

namespace Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate
{
    public interface IItemRepository : IRepository<Item>
    {
        Item GetById(Guid id);
        Item GetByItemId(string new_id);
        bool SaveEntities();
    }
}
