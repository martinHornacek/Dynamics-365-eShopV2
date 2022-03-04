using System;
using System.Collections.Generic;
using System.Linq;

namespace Item.API.Data
{
    public class ItemRepository : IItemRepository
    {
        private readonly ItemContext _context;

        public ItemRepository(ItemContext context)
        {
            _context = context;
        }

        public void CreateItem(Model.Item item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            _context.Items.Add(item);
        }

        public IEnumerable<Model.Item> GetAllItems()
        {
            return _context.Items.ToList();
        }

        public Model.Item GetItemById(int id)
        {
            return _context.Items.FirstOrDefault(p => p.Id == id);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
