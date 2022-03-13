using Basket.Management.Basket.Domain.Messaging;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Client;

namespace Basket.Management.Basket.Infrastructure
{
    public class BasketContext : OrganizationServiceContext
    {
        private readonly IMediator _mediator;

        public BasketContext(IOrganizationService service, IMediator mediator) : base(service)
        {
            _mediator = mediator;
        }

        public bool SaveEntities()
        {
            // Dispatch Domain Events collection.
            _mediator.DispatchDomainEvents(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the OrganizationServiceContext will be committed
            var result = base.SaveChanges();

            return true;
        }
    }
}
