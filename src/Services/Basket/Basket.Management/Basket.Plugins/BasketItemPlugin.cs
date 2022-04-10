using Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate;
using Basket.Management.Basket.Domain.AggregatesModel.ItemAggregate;
using Basket.Management.Basket.Infrastructure.Contexts;
using Basket.Management.Basket.Infrastructure.Mappers;
using Basket.Management.Basket.Infrastructure.Repositories;
using CrmEarlyBound;
using Microsoft.Xrm.Sdk;
using System;

namespace Basket.Management.Basket.Plugins
{
    public class BasketItemPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (context.PrimaryEntityName != new_basketitem.EntityLogicalName) return;

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            var basketContext = new BasketContext(service);

            var itemMapper = new ItemMapper();
            var basketItemMapper = new BasketItemMapper(itemMapper);
            var basketMapper = new BasketMapper(basketItemMapper);

            var basketRepository = new BasketRepository(basketContext, basketMapper, basketItemMapper, itemMapper);
            var itemRepository = new ItemRepository(basketContext, itemMapper);

            switch (context.MessageName)
            {
                case "Create":
                    PreOperationBasketItemCreateHandler(context, basketRepository, itemRepository);
                    break;
                case "Update":
                    PostOperationBasketItemUpdateHandler(context, basketRepository, itemRepository);
                    break;
                case "Delete":
                    PreOperationBasketItemDeleteHandler(context, basketRepository, itemRepository);
                    break;
            }

            basketRepository.SaveEntities();
        }

        private void PreOperationBasketItemCreateHandler(IPluginExecutionContext context, IBasketRepository basketRepository, IItemRepository itemRepository)
        {
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity)) return;

            var target = context.InputParameters["Target"] as Entity;
            var new_basketItem = target.ToEntity<new_basketitem>();

            if (new_basketItem.new_item == null || new_basketItem.new_basket == null) return;

            var item = itemRepository.GetById(new_basketItem.new_item.Id);
            var basket = basketRepository.GetById(new_basketItem.new_basket.Id);

            basket.AddBasketItem(item, new_basketItem.new_quantity ?? 0);
            basketRepository.Update(basket);
        }

        private void PostOperationBasketItemUpdateHandler(IPluginExecutionContext context, IBasketRepository basketRepository, IItemRepository itemRepository)
        {
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is Entity)) return;

            var postImage = context.PostEntityImages["PostImage"] as Entity;
            var basketItem = postImage.ToEntity<new_basketitem>();

            if (basketItem.new_item == null || basketItem.new_basket == null) return;

            var item = itemRepository.GetById(basketItem.new_item.Id);
            var basket = basketRepository.GetById(basketItem.new_basket.Id);

            basket.AddBasketItem(item, basketItem.new_quantity ?? 0);
            basketRepository.Update(basket);
        }

        private void PreOperationBasketItemDeleteHandler(IPluginExecutionContext context, IBasketRepository basketRepository, IItemRepository itemRepository)
        {
            if (!context.InputParameters.Contains("Target") || !(context.InputParameters["Target"] is EntityReference)) return;

            var preImage = context.PreEntityImages["PreImage"] as Entity;
            var basketItem = preImage.ToEntity<new_basketitem>();

            if (basketItem.new_basket == null) return;

            var basket = basketRepository.GetById(basketItem.new_basket.Id);

            basket.RemoveBasketItem(basketItem.new_itemid);
            basketRepository.Update(basket);
        }
    }
}
