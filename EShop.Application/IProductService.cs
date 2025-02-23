using EShop.Core;

namespace EShop.Application
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProducts();
        Task<IEnumerable<Product>> GetAllProducts(int page = 1, int pageSize = 10);
        Task<Product?> GetProductById(int id);
        Task UpdateProductDescription(int id, string description);
    }
}