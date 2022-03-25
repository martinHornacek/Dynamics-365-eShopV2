using Basket.Management.Basket.Infrastructure;
using Basket.Management.Basket.Infrastructure.Repositories;
using Microsoft.Xrm.Sdk;
using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Basket.Management.Basket.CustomActions
{
    public class UpdateBasketCustomAction : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            var context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));

            if (context.InputParameters.Contains("payload") && context.InputParameters["payload"] is string)
            {
                var serviceFactory = (IOrganizationServiceFactory)serviceProvider.GetService(typeof(IOrganizationServiceFactory));
                var service = serviceFactory.CreateOrganizationService(context.UserId);

                var basketContext = new BasketContext(service);
                var basketRepository = new BasketRepository(basketContext);

                var itemContext = new ItemContext(service);
                var itemRepository = new ItemRepository(itemContext);

                string payload = (string)context.InputParameters["payload"];
                var @event = JSONSerializer<BasketEvent>.DeSerialize(payload);
                var basketItem = @event.EventArgument;

                if (basketItem.new_itemid == null || basketItem.new_basketid == null)
                {
                    return;
                }

                var item = itemRepository.GetByItemId(basketItem.new_itemid);
                var basket = basketRepository.GetByBasketId(basketItem.new_basketid);

                switch (@event.EventName)
                {
                    case "CreateBasketItem":
                    case "UpdateBasketItem":
                        basket.AddBasketItem(item.ItemId, item.Price, basketItem.new_quantity ?? 0);
                        break;
                    case "DeleteBasketItem":
                        basket.RemoveBasketItem(basketItem.new_itemid);
                        break;
                }

                basketRepository.Update(basket);
                basketContext.SaveChanges();
            }
        }

        public class BasketEvent
        {
            public string EventName { get; set; }
            public BasketItemDto EventArgument { get; set; }
        }

        public class BasketItemDto
        {
            public string new_itemid { get; set; }
            public string new_basketid { get; set; }
            public int? new_quantity { get; set; }
        }

        public static class JSONSerializer<TType> where TType : class
        {
            /// <summary>
            /// Serializes an object to JSON
            /// </summary>
            public static string Serialize(TType instance)
            {
                var serializer = CreateSerializer();
                using (var stream = new MemoryStream())
                {
                    serializer.WriteObject(stream, instance);
                    return Encoding.Default.GetString(stream.ToArray());
                }
            }

            private static DataContractJsonSerializer CreateSerializer()
            {
                var serializerSettings = new DataContractJsonSerializerSettings();
                serializerSettings.DateTimeFormat = new DateTimeFormat("yyyy-MM-dd'T'HH:mm:ssZ");
                return new DataContractJsonSerializer(typeof(TType), serializerSettings);
            }

            /// <summary>
            /// DeSerializes an object from JSON
            /// </summary>
            public static TType DeSerialize(string json)
            {
                using (var stream = new MemoryStream(Encoding.Default.GetBytes(json)))
                {
                    var serializer = CreateSerializer();
                    return serializer.ReadObject(stream) as TType;
                }
            }
        }
    }
}
