using System.Linq;
using Basket.Management.Basket.Domain.Messaging;

namespace Basket.Management.Basket.Infrastructure
{
    public static class MediatorExtension
    {
        public static void DispatchDomainEvents(this IMediator mediator, BasketContext context)
        {
            var domainEntities = context
                .GetAttachedEntities()
                .Select(e => e.ToEntity<Domain.AggregatesModel.BasketAggregate.Basket>())
                .Where(x => x.DomainEvents != null && x.DomainEvents.Any());

            var domainEvents = domainEntities
                .SelectMany(x => x.DomainEvents)
                .ToList();

            domainEntities.ToList()
                .ForEach(entity => entity.ClearDomainEvents());

            foreach (var domainEvent in domainEvents)
            {
                mediator.Publish(domainEvent);
            }
        }
    }
}
