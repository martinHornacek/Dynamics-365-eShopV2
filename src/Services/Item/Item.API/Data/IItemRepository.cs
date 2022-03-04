using System.Collections.Generic;

namespace Item.API.Data
{
    public interface IItemRepository
    {
        bool SaveChanges();

        IEnumerable<Model.Item> GetAllItems();
        Model.Item GetItemById(int id);
        void CreateItem(Model.Item item);
    }
}
