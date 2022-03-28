using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using System.Linq;

namespace Basket.Management.Basket.Infrastructure.Contexts
{
    public class ItemContext : BasketMangementServiceContext
    {
        public IQueryable<new_item> Items { get; private set; }

        public ItemContext(IOrganizationService service) : base(service)
        {
            Items = base.new_itemSet;
        }
    }
}
