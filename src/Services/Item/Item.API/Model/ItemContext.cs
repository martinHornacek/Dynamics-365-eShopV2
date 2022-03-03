using Microsoft.EntityFrameworkCore;

namespace Item.API.Model
{
    public class ItemContext : DbContext
    {
        public ItemContext(DbContextOptions<ItemContext> options)
            : base(options)
        {
        }

        public DbSet<Item> Items { get; set; }
    }
}
