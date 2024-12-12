using Mango.Services.ProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>options):base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 1,
                Name = "Bananas",
                Price = 10,
                Description = "description",
                CategoryName = "Fruit",
                ImageUrl="haidgsfghdfj"
            });

            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 2,
                Name = "Peras",
                Price = 12,
                Description = "description",
                CategoryName = "Fruit",
                ImageUrl = "haidgsfghdfj"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 3,
                Name = "Apples",
                Price = 8,
                Description = "description",
                CategoryName = "Fruit",
                ImageUrl = "haidgsfghdfj"
            });
            modelBuilder.Entity<Product>().HasData(new Product
            {
                ProductId = 4,
                Name = "Bread",
                Price = 5,
                Description = "description",
                CategoryName = "Bakery",
                ImageUrl = "haidgsfghdfj"
            });
        }
    }
}
