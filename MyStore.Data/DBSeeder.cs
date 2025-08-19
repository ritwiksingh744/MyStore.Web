using MyStore.Data.Entity;

namespace MyStore.Data
{
    public class DBSeeder
    {
        public static async Task Seed(StoreDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();

            //Seed Category
            if (!context.Category.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Id = Guid.NewGuid(), CategoryName = "Electronics", IsDeleted = false, CreatedOn = DateTime.Now },
                    new Category { Id = Guid.NewGuid(), CategoryName = "Books", IsDeleted = false, CreatedOn = DateTime.Now },
                    new Category { Id = Guid.NewGuid(), CategoryName = "Clothing", IsDeleted = false, CreatedOn = DateTime.Now },
                    new Category { Id = Guid.NewGuid(), CategoryName = "Home & Kitchen", IsDeleted = false, CreatedOn = DateTime.Now },
                    new Category { Id = Guid.NewGuid(), CategoryName = "Sports & Outdoors", IsDeleted = false, CreatedOn = DateTime.Now }
                };

                await context.Category.AddRangeAsync(categories);
                await context.SaveChangesAsync();
            }

            //Seed Items
            if (!context.Items.Any()) 
            {
                var electronicsId = context.Category.First(x => x.CategoryName == "Electronics").Id;
                var booksId = context.Category.First(x => x.CategoryName == "Books").Id;
                var clothingId = context.Category.First(x => x.CategoryName == "Clothing").Id;
                var homeKitchenId = context.Category.First(x => x.CategoryName == "Home & Kitchen").Id;
                var sportsId = context.Category.First(x => x.CategoryName == "Sports & Outdoors").Id;

                var items = new List<Items>
                {
                    new Items { Id = Guid.NewGuid(), ItemName = "Laptop", Price = 800.00, CategoryId = electronicsId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Smartphone", Price = 500.00, CategoryId = electronicsId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Wireless Earbuds", Price = 120.00, CategoryId = electronicsId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Smartwatch", Price = 200.00, CategoryId = electronicsId, IsDeleted = false, CreatedOn = DateTime.UtcNow },

                    new Items { Id = Guid.NewGuid(), ItemName = "Programming in C#", Price = 35.00, CategoryId = booksId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Clean Code", Price = 40.00, CategoryId = booksId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "The Pragmatic Programmer", Price = 38.00, CategoryId = booksId, IsDeleted = false, CreatedOn = DateTime.Now },

                    new Items { Id = Guid.NewGuid(), ItemName = "Men's T-Shirt", Price = 15.00, CategoryId = clothingId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Women's Jeans", Price = 45.00, CategoryId = clothingId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Hoodie", Price = 30.00, CategoryId = clothingId, IsDeleted = false, CreatedOn = DateTime.Now },

                    new Items { Id = Guid.NewGuid(), ItemName = "Blender", Price = 60.00, CategoryId = homeKitchenId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Coffee Maker", Price = 90.00, CategoryId = homeKitchenId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Non-stick Pan", Price = 25.00, CategoryId = homeKitchenId, IsDeleted = false, CreatedOn = DateTime.Now },

                    new Items { Id = Guid.NewGuid(), ItemName = "Football", Price = 22.00, CategoryId = sportsId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Yoga Mat", Price = 18.00, CategoryId = sportsId, IsDeleted = false, CreatedOn = DateTime.Now },
                    new Items { Id = Guid.NewGuid(), ItemName = "Tennis Racket", Price = 75.00, CategoryId = sportsId, IsDeleted = false, CreatedOn = DateTime.Now }

                };

                await context.Items.AddRangeAsync(items);
                await context.SaveChangesAsync();
            }
        }
    }
}
