using Microsoft.EntityFrameworkCore;

namespace RedisExampleApp.API.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>().HasData(
                new Product() { Id = 1, Name = "Kalem 1", Price = 100 },
                new Product() { Id = 2, Name = "Kalem 2", Price = 200 },
                new Product() { Id = 3, Name = "Kalem 3", Price = 300 },
                new Product() { Id = 4, Name = "Kalem 4", Price = 400 },
                new Product() { Id = 5, Name = "Kalem 5", Price = 500 },
                new Product() { Id = 6, Name = "Kalem 6", Price = 600 }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}
