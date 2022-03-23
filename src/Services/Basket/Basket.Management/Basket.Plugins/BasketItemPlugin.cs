using Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate;
using Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate;
using Basket.Management.Basket.Infrastructure;
using Basket.Management.Basket.Infrastructure.Repositories;
using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace Basket.Management.Basket.Plugins
{
    public class BasketItemPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.PrimaryEntityName != new_basketitem.EntityLogicalName)
            {
                return;
            }

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            var basketContext = new BasketContext(service);
            var basketRepository = new BasketRepository(basketContext);

            var itemContext = new ItemContext(service);
            var itemRepository = new ItemRepository(itemContext);

            switch (context.MessageName)
            {
                case "Create":
                    PreOperationBasketItemCreateHandler(context, basketRepository, itemRepository);
                    break;
                case "Update":
                    PreOperationBasketItemUpdateHandler(context, basketRepository, itemRepository);
                    break;
                case "Delete":
                    PreOperationBasketItemDeleteHandler(context, basketRepository, itemRepository);
                    break;
            }

            basketContext.SaveChanges();
        }

        private void PreOperationBasketItemCreateHandler(IPluginExecutionContext context, IBasketRepository basketRepository, IItemRepository itemRepository)
        {
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
            {
                return;
            }

            var target = context.InputParameters["Target"] as Entity;
            var basketItem = target.ToEntity<new_basketitem>();

            if (basketItem.new_item == null || basketItem.new_basket == null)
            {
                return;
            }

            var item = itemRepository.GetById(basketItem.new_item.Id);
            var basket = basketRepository.GetById(basketItem.new_basket.Id);

            basket.AddBasketItem(item.ItemId, item.Price, basketItem.new_quantity ?? 0);
            basketRepository.Update(basket);
        }

        private void PreOperationBasketItemUpdateHandler(IPluginExecutionContext context, IBasketRepository basketRepository, IItemRepository itemRepository)
        {
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity))
            {
                return;
            }

            var preImage = context.PreEntityImages["PreImage"] as Entity;
            var target = context.InputParameters["Target"] as Entity;

            var basketItem = preImage.ToEntity<new_basketitem>();
            if (basketItem.new_item == null || basketItem.new_basket == null)
            {
                return;
            }

            var updatedQuantity = target.GetAttributeValue<int?>("new_quantity");

            var item = itemRepository.GetById(basketItem.new_item.Id);
            var basket = basketRepository.GetById(basketItem.new_basket.Id);

            basket.AddBasketItem(item.ItemId, item.Price, updatedQuantity ?? 0);
            basketRepository.Update(basket);
        }

        private void PreOperationBasketItemDeleteHandler(IPluginExecutionContext context, IBasketRepository basketRepository, IItemRepository itemRepository)
        {
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is EntityReference))
            {
                return;
            }

            var preImage = context.PreEntityImages["PreImage"] as Entity;
            var basketItem = preImage.ToEntity<new_basketitem>();

            if (basketItem.new_item == null || basketItem.new_basket == null)
            {
                return;
            }

            var basket = basketRepository.GetById(basketItem.new_basket.Id);

            basket.RemoveBasketItem(basketItem.new_itemid);
            basketRepository.Update(basket);
        }
    }
}
