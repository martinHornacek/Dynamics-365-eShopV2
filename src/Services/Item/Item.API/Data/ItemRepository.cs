using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Item.API.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly string baseUrl = "http://kmdev16-ver-9/eshop/api/data/v9.0/";
        private readonly ItemContext _context;

        public ItemRepository(ItemContext context)
        {
            _context = context;
        }

        public async Task CreateItem(Model.Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            var payload = JsonConvert.SerializeObject(item);
            await PostAsync("new_items", payload);
        }

        public async Task<IEnumerable<Model.Item>> GetAllItems()
        {
            var responseJson = await GetStringAsync("new_items?$select=new_name,new_id,new_price,new_description,new_category");
            ODataResponse<Model.Item> oDataResponse = JsonConvert.DeserializeObject<ODataResponse<Model.Item>>(responseJson);
            var items = oDataResponse.Value;
            return items;
        }

        public async Task<Model.Item> GetItemById(string id)
        {
            var responseJson = await GetStringAsync($"new_items?$select=new_name,new_id,new_price,new_description,new_category&$filter=new_id eq '{id}'");
            ODataResponse<Model.Item> oDataResponse = JsonConvert.DeserializeObject<ODataResponse<Model.Item>>(responseJson);
            var item = oDataResponse.Value.FirstOrDefault();
            return item;
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
