using System.Collections.Generic;
using System.Threading.Tasks;

namespace Item.API.Data
{
    public interface IItemRepository
    {
        Task<IEnumerable<Model.Item>> GetAllItems();
        Task<Model.Item> GetItemById(string id);
        Task CreateItem(Model.Item item);
    }
}
