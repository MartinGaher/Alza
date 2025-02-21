using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
       .Property(p => p.Price)
       .HasColumnType("decimal(18,2)"); // Specify precision and scale

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Product 1", ImgUri = "http://example.com/img1.jpg", Price = 10.0m, Description = "Description for Product 1" },
            new Product { Id = 2, Name = "Product 2", ImgUri = "http://example.com/img2.jpg", Price = 20.0m, Description = "Description for Product 2" },
            new Product { Id = 3, Name = "Product 3", ImgUri = "http://example.com/img3.jpg", Price = 30.0m, Description = "Description for Product 3" }
        );
    }
}