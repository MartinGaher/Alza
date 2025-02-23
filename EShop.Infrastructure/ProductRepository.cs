using EShop.Core;
using Microsoft.EntityFrameworkCore;

namespace EShop.Infrastructure
{
    public class ProductRepository(AppDbContext context) : IProductRepository
    {
        private readonly AppDbContext _context = context;

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetAllProducts(int page, int pageSize)
        {
            return await _context.Products
               .Skip((page - 1) * pageSize)
               .Take(pageSize)
               .ToListAsync();
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task UpdateProductDescription(int id, string description)
        {
            var product = await _context.Products.FindAsync(id);

            if (product != null)
            {
                product.Description = description;
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException($"Product with ID {id} not found.");
            }
        }
    }
}