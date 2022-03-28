using Basket.Management.Basket.CustomActions.Model;
using Basket.Management.Basket.Helpers;
using Basket.Management.Basket.Infrastructure.Contexts;
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

            var basketRepository = new BasketRepository(new BasketContext(service));
            var itemRepository = new ItemRepository(new ItemContext(service));
                        
            (BasketEvent @event, BasketItemDto basketItemDto) = ParsePayload(context);
            if (basketItemDto.new_itemid == null || basketItemDto.new_basketid == null) return;

            var item = itemRepository.GetByItemId(basketItemDto.new_itemid);
            var basket = basketRepository.GetByBasketId(basketItemDto.new_basketid);

            switch (@event.EventName)
            {
                case "CreateBasketItem":
                case "UpdateBasketItem":
                    basket.AddBasketItem(item, basketItemDto.new_quantity ?? 0);
                    break;
                case "DeleteBasketItem":
                    basket.RemoveBasketItem(basketItemDto.new_itemid);
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
