using Microsoft.EntityFrameworkCore;

namespace Item.API.Data
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options) : base(options)
        {
        }

        public DbSet<Model.Item> Items { get; set; }
    }
}
