using EShop.Core;

namespace EShop.Mocks
{
    public class MockProductRepository : IProductRepository
    {
        private readonly List<Product> _products;

        public MockProductRepository()
        {
            // Initialize with some mock data
            _products =
           [
               new() { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/mock1.jpg", Price = 10.0m, Description = "Mock Description 1" },
               new() { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/mock2.jpg", Price = 20.0m, Description = "Mock Description 2" },
               new() { Id = 3, Name = "Mock Product 3", ImgUri = "http://example.com/mock3.jpg", Price = 30.0m, Description = "Mock Description 3" }
           ];
        }

        public Task<Product?> GetProductById(int id)
        {
            return Task.FromResult(_products.FirstOrDefault(p => p.Id == id));
        }

        public Task<IEnumerable<Product>> GetAllProducts()
        {
            return Task.FromResult(_products.AsEnumerable());
        }

        public Task<IEnumerable<Product>> GetAllProducts(int page, int pageSize)
        {
            return Task.FromResult(_products.Skip((page - 1) * pageSize).Take(pageSize).AsEnumerable());
        }

        public Task UpdateProductDescription(int id, string description)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product != null)
            {
                product.Description = description;
            }

            return Task.CompletedTask;
        }
    }
}
