using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using System.Linq;

namespace Basket.Management.Basket.Infrastructure
{
    public class BasketContext : BasketMangementServiceContext
    {
        //private readonly IMediator _mediator;

        public IQueryable<new_basket> Baskets { get; private set; }
        public IQueryable<new_basketitem> BasketItems { get; private set; }
        public IQueryable<new_item> Items { get; private set; }

        public BasketContext(IOrganizationService service/*, IMediator mediator*/) : base(service)
        {
            //_mediator = mediator;
            Baskets = base.new_basketSet;
            BasketItems = base.new_basketitemSet;
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
