using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using System;

namespace BasketManagement.Basket.Plugins
{
    public class BasketItemPlugin : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            if (context.PrimaryEntityName != "new_basketitem") return;

            var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
            var service = serviceFactory.CreateOrganizationService(context.UserId);

            switch (context.MessageName)
            {
                case "Create":
                    PostOperationBasketItemCreateHandler(context, service);
                    break;
                case "Update":
                    PostOperationBasketItemUpdateHandler(context, service);
                    break;
                case "Delete":
                    PostOperationBasketItemDeleteHandler(context, service);
                    break;
            }
        }

        private void PostOperationBasketItemCreateHandler(IPluginExecutionContext context, IOrganizationService service)
        {
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity basketItem)
            {
                var itemReference = basketItem.GetAttributeValue<EntityReference>("new_item");
                if (itemReference == null) return;

                var item = service.Retrieve(itemReference.LogicalName, itemReference.Id, new ColumnSet("new_price"));

                var quantity = basketItem.GetAttributeValue<int>("new_quantity");
                var itemPrice = item.GetAttributeValue<decimal>("new_price");

                decimal basketItemTotalValue = quantity * itemPrice;

                var basketReference = basketItem.GetAttributeValue<EntityReference>("new_basket");
                if (basketReference == null) return;
                var basket = service.Retrieve(basketReference.LogicalName, basketReference.Id, new ColumnSet("new_totalvalue"));
                
                var basketTotalValue = basket.GetAttributeValue<decimal>("new_totalvalue");

                service.Update(new Entity
                {
                    LogicalName = basketReference.LogicalName,
                    Id = basketReference.Id,
                    Attributes =
                    {
                        ["new_totalvalue"] = basketTotalValue + basketItemTotalValue
                    }
                });
            }
        }

        private void PostOperationBasketItemUpdateHandler(IPluginExecutionContext context, IOrganizationService service)
        {
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is Entity basketItem)
            {
                var basketItemPreImage = context.PreEntityImages["PreImage"] as Entity;
                var basketItemPostImage = context.PostEntityImages["PostImage"] as Entity;

                var itemReference = basketItemPostImage.GetAttributeValue<EntityReference>("new_item");
                if (itemReference == null) return;
                var item = service.Retrieve(itemReference.LogicalName, itemReference.Id, new ColumnSet("new_price"));

                var preQuantity = basketItemPreImage.GetAttributeValue<int>("new_quantity");
                var postQuantity = basketItemPostImage.GetAttributeValue<int>("new_quantity");
                var itemPrice = item.GetAttributeValue<decimal>("new_price");

                decimal preBasketItemTotalValue = preQuantity * itemPrice;
                decimal postBasketItemTotalValue = postQuantity * itemPrice;

                var basketReference = basketItemPostImage.GetAttributeValue<EntityReference>("new_basket");
                if (basketReference == null) return;
                var basket = service.Retrieve(basketReference.LogicalName, basketReference.Id, new ColumnSet("new_totalvalue"));

                var preBasketTotalValue = basket.GetAttributeValue<decimal>("new_totalvalue");

                service.Update(new Entity
                {
                    LogicalName = basketReference.LogicalName,
                    Id = basketReference.Id,
                    Attributes =
                    {
                        ["new_totalvalue"] = preBasketTotalValue - preBasketItemTotalValue + postBasketItemTotalValue
                    }
                });
            }
        }

        private void PostOperationBasketItemDeleteHandler(IPluginExecutionContext context, IOrganizationService service)
        {
            if (context.InputParameters.Contains("Target") && context.InputParameters["Target"] is EntityReference basketItemReference)
            {
                var basketItemPreImage = context.PreEntityImages["PreImage"] as Entity;

                var itemReference = basketItemPreImage.GetAttributeValue<EntityReference>("new_item");
                if (itemReference == null) return;
                var item = service.Retrieve(itemReference.LogicalName, itemReference.Id, new ColumnSet("new_price"));

                var quantity = basketItemPreImage.GetAttributeValue<int>("new_quantity");
                var itemPrice = item.GetAttributeValue<decimal>("new_price");

                decimal basketItemTotalValue = quantity * itemPrice;

                var basketReference = basketItemPreImage.GetAttributeValue<EntityReference>("new_basket");
                if (basketReference == null) return;
                var basket = service.Retrieve(basketReference.LogicalName, basketReference.Id, new ColumnSet("new_totalvalue"));

                var oldBasketTotalValue = basket.GetAttributeValue<decimal>("new_totalvalue");

                service.Update(new Entity
                {
                    LogicalName = basketReference.LogicalName,
                    Id = basketReference.Id,
                    Attributes =
                    {
                        ["new_totalvalue"] = oldBasketTotalValue - basketItemTotalValue
                    }
                });
            }
        }
    }
}
