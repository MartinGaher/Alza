using EShop.Core;

namespace EShop.Application
{
    public class ProductService(IProductRepository productRepository) : IProductService
    {
        private readonly IProductRepository _productRepository = productRepository;

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        public async Task<IEnumerable<Product>> GetAllProducts(int page = 1, int pageSize = 10)
        {
            return await _productRepository.GetAllProducts(page, pageSize);
        }

        public async Task<Product?> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        public async Task UpdateProductDescription(int id, string description)
        {
            var product = await _productRepository.GetProductById(id) ?? throw new KeyNotFoundException($"Product with ID {id} not found.");
            product.Description = description;
            await _productRepository.UpdateProductDescription(id, description);
        }
    }
}