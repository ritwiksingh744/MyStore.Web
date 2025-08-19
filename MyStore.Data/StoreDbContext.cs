using Microsoft.EntityFrameworkCore;
using MyStore.Data.Entity;

namespace MyStore.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions options) : base(options)
        {
            
        }

        public DbSet<Category> Category { get; set; }
        public DbSet<Items> Items { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderedItem> OrderedItem { get; set; }
    }
}
