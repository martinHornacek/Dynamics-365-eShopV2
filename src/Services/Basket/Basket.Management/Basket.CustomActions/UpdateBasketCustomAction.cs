using Basket.Management.Basket.CustomActions.Model;
using Basket.Management.Basket.Domain.AggregatesModel.BasketAggregate;
using Basket.Management.Basket.Helpers;
using Basket.Management.Basket.Infrastructure.Contexts;
using Basket.Management.Basket.Infrastructure.Mappers;
using Basket.Management.Basket.Infrastructure.Repositories;
using Microsoft.Xrm.Sdk;
using System;

namespace Basket.Management.Basket.CustomActions
{
    public class UpdateBasketCustomAction : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (!context.InputParameters.Contains("payload") || !(context.InputParameters["payload"] is string)) return;

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            var basketContext = new BasketContext(service);

            var itemMapper = new ItemMapper();
            var basketItemMapper = new BasketItemMapper(itemMapper);
            var basketMapper = new BasketMapper(basketItemMapper);

            var basketRepository = new BasketRepository(basketContext, basketMapper, basketItemMapper, itemMapper);
            var itemRepository = new ItemRepository(basketContext, itemMapper);

            (BasketEvent @event, BasketItemDto basketItemDto) = ParsePayload(context);
            if (basketItemDto.new_itemid == null || basketItemDto.new_basketid == null) return;

            var basket = basketRepository.GetByBasketId(basketItemDto.new_basketid);
            var item = itemRepository.GetByItemId(basketItemDto.new_itemid);

            BasketItem basketItem; bool exists;
            switch (@event.EventName)
            {
                case "UpsertBasketItem":
                    (exists, basketItem) = basket.AddBasketItem(item, basketItemDto.new_quantity ?? 0);
                    if (exists) basketRepository.UpdateBasketItem(basketItem);
                    if (!exists) basketRepository.CreateBasketItem(basket, basketItem);
                    break;
                case "DeleteBasketItem":
                    basketItem = basket.RemoveBasketItem(basketItemDto.new_itemid);
                    basketRepository.DeleteBasketItem(basketItem);
                    break;
            }

            basketRepository.Update(basket);
            basketRepository.SaveEntities();
        }

        private (BasketEvent @event, BasketItemDto basketItemDto) ParsePayload(IPluginExecutionContext context)
        {
            string payload = (string)context.InputParameters["payload"];
            var @event = JsonSerializer<BasketEvent>.Deserialize(payload);
            var basketItemDto = @event.EventArgument;

            return (@event, basketItemDto);
        }
    }
}
