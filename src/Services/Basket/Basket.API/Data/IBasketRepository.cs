using Basket.API.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Basket.API.Data
{
    public interface IBasketRepository
    {
        Task<IEnumerable<Model.BasketItem>> GetAllBasketItemsForBasket(string basketId);
        Task<Model.BasketItem> GetBasketItemForBasket(string basketId, string itemId);
        Task AddBasketItem(BasketItemDto item);
        Task RemoveBasketItem(string basketid, string itemid);

        Task EmptyBasket(string basketId);
        Task<IEnumerable<Model.Basket>> GetAllBaskets();
        Task<Model.Basket> GetBasketById(string basketId);
        Task CreateBasket(Model.Basket basket);
    }
}
