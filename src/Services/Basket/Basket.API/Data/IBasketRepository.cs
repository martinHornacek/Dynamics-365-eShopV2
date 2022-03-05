using System.Collections.Generic;

namespace Basket.API.Data
{
    public interface IBasketRepository
    {
        bool SaveChanges();

        IEnumerable<Model.BasketItem> GetAllBasketItemsForBasket(int basketId);
        Model.BasketItem GetBasketItemForBasket(int basketId, int basketItemId);
        void AddBasketItem(int basketId, Model.BasketItem item);
        void RemoveBasketItem(int basketId, int basketItemId);

        void EmptyBasket(int basketId);
        IEnumerable<Model.Basket> GetAllBaskets();
        Model.Basket GetBasketById(int basketId);
        void CreateBasket(Model.Basket basket);
    }
}
