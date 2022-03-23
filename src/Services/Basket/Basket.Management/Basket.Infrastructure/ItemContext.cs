using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using System.Linq;

namespace Basket.Management.Basket.Infrastructure
{
    public class ItemContext : BasketMangementServiceContext
    {
        //private readonly IMediator _mediator;

        public IQueryable<new_item> Items { get; private set; }

        public ItemContext(IOrganizationService service/*, IMediator mediator*/) : base(service)
        {
            //_mediator = mediator;
            Items = base.new_itemSet;
        }

        public bool SaveEntities()
        {
            // Dispatch Domain Events collection.
            //_mediator.DispatchDomainEvents(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the OrganizationServiceContext will be committed
            var result = base.SaveChanges();

            return true;
        }
    }
}
