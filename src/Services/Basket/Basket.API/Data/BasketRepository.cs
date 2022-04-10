using Basket.API.DTOs;
using Basket.API.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Basket.API.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly string baseUrl = "http://kmdev16-ver-9/eshop/api/data/v9.0/";
        private readonly BasketContext _context;

        public BasketRepository(BasketContext context)
        {
            _context = context;
        }

        public async Task AddBasketItem(BasketItemDto basketItem)
        {
            if (basketItem == null)
            {
                throw new ArgumentNullException(nameof(basketItem));
            }

            var @event = new BasketEvent
            {
                EventName = "UpsertBasketItem",
                EventArgument = basketItem
            };

            await PostAsync("new_UpdateBasket", $"{{ payload: \"{HttpUtility.JavaScriptStringEncode(JsonConvert.SerializeObject(@event))}\"}}");
        }

        public async Task CreateBasket(Model.Basket basket)
        {
            if (basket == null)
            {
                throw new ArgumentNullException(nameof(basket));
            }

            var payload = JsonConvert.SerializeObject(basket);
            await PostAsync("new_baskets", payload);
        }

        public async Task EmptyBasket(string basketId)
        {
            var basketItems = await this.GetAllBasketItemsForBasket(basketId);
            foreach (var basketItem in basketItems)
            {
                await RemoveBasketItem(basketItem.new_basketid, basketItem.new_itemid);
            }
        }

        public async Task<IEnumerable<BasketItem>> GetAllBasketItemsForBasket(string basketId)
        {
            var responseJson = await GetStringAsync($"new_basketitems?$select=new_name,new_id,new_itemid,new_basketid,new_quantity&$filter=new_basketid eq '{basketId}'");
            ODataResponse<BasketItem> oDataResponse = JsonConvert.DeserializeObject<ODataResponse<BasketItem>>(responseJson);
            var basketItems = oDataResponse.Value;
            return basketItems;
        }

        public async Task<BasketItem> GetBasketItemForBasket(string basketId, string itemId)
        {
            var responseJson = await GetStringAsync($"new_basketitems?$select=new_name,new_id,new_itemid,new_basketid,new_quantity&$filter=new_basketid eq '{basketId}' and new_itemid eq '{itemId}'");
            ODataResponse<BasketItem> oDataResponse = JsonConvert.DeserializeObject<ODataResponse<BasketItem>>(responseJson);
            var basketItem = oDataResponse.Value.FirstOrDefault();
            return basketItem;
        }

        public async Task<IEnumerable<Model.Basket>> GetAllBaskets()
        {
            var responseJson = await GetStringAsync("new_baskets?$select=new_name,new_id,new_description,new_totalvalue");
            ODataResponse<Model.Basket> oDataResponse = JsonConvert.DeserializeObject<ODataResponse<Model.Basket>>(responseJson);
            var baskets = oDataResponse.Value;
            return baskets;
        }

        public async Task<Model.Basket> GetBasketById(string basketId)
        {
            var responseJson = await GetStringAsync($"new_baskets?$select=new_name,new_id,new_description,new_totalvalue&$filter=new_id eq '{basketId}'");
            ODataResponse<Model.Basket> oDataResponse = JsonConvert.DeserializeObject<ODataResponse<Model.Basket>>(responseJson);
            var basket = oDataResponse.Value.FirstOrDefault();
            return basket;
        }

        public async Task RemoveBasketItem(string basketid, string itemid)
        {
            var @event = new BasketEvent
            {
                EventName = "DeleteBasketItem",
                EventArgument = new BasketItemDto
                {
                    new_basketid = basketid,
                    new_itemid = itemid
                }
            };

            await PostAsync("new_UpdateBasket", $"{{ payload: \"{HttpUtility.JavaScriptStringEncode(JsonConvert.SerializeObject(@event))}\"}}");
        }

        private Task<string> GetStringAsync(string url)
        {
            var uri = new Uri(baseUrl);
            var credentialsCache = new CredentialCache { { uri, "NTLM", CredentialCache.DefaultNetworkCredentials } };
            var handler = new HttpClientHandler { Credentials = credentialsCache };
            var httpClient = new HttpClient(handler) { BaseAddress = uri, Timeout = new TimeSpan(0, 0, 10) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient.GetStringAsync(url);
        }

        private Task<HttpResponseMessage> PostAsync(string url, string payload)
        {
            var uri = new Uri(baseUrl);
            var credentialsCache = new CredentialCache { { uri, "NTLM", CredentialCache.DefaultNetworkCredentials } };
            var handler = new HttpClientHandler { Credentials = credentialsCache };
            var httpClient = new HttpClient(handler) { BaseAddress = uri, Timeout = new TimeSpan(0, 0, 10) };
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var content = new StringContent(payload, Encoding.UTF8, "application/json");
            return httpClient.PostAsync(url, content);
        }
    }
}
