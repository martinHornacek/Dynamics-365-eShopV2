using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using System.Linq;

namespace Basket.Management.Basket.Infrastructure.Contexts
{
    public class BasketContext : BasketMangementServiceContext
    {
        public IQueryable<new_basket> Baskets { get; private set; }
        public IQueryable<new_basketitem> BasketItems { get; private set; }
        public IQueryable<new_item> Items { get; private set; }

        public BasketContext(IOrganizationService service) : base(service)
        {
            Baskets = base.new_basketSet;
            BasketItems = base.new_basketitemSet;
            Items = base.new_itemSet;
        }
    }
}
