using EShop.Application;
using EShop.Core;
using Moq;

namespace EShop.Tests
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _mockProductRepository;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _mockProductRepository = new Mock<IProductRepository>();
            _productService = new ProductService(_mockProductRepository.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsAllProducts()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new() { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/mock1.jpg", Price = 10.0m, Description = "Mock Description 1" },
                new() { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/mock2.jpg", Price = 20.0m, Description = "Mock Description 2" },
                new() { Id = 3, Name = "Mock Product 3", ImgUri = "http://example.com/mock3.jpg", Price = 30.0m, Description = "Mock Description 3" }
            };

            // Setup the mock repository to return the mock products
            _mockProductRepository.Setup(repo => repo.GetAllProducts())
                .ReturnsAsync(mockProducts);

            // Act
            var result = await _productService.GetAllProducts();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count()); // Assuming there are 3 mock products
        }

        [Fact]
        public async Task GetAllProducts_WithPagination_ReturnsPaginatedProducts()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new() { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/mock1.jpg", Price = 10.0m, Description = "Mock Description 1" },
                new() { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/mock2.jpg", Price = 20.0m, Description = "Mock Description 2" },
                new() { Id = 3, Name = "Mock Product 3", ImgUri = "http://example.com/mock3.jpg", Price = 30.0m, Description = "Mock Description 3" }
            };

            // Setup the mock repository to return the first two products for pagination
            _mockProductRepository.Setup(repo => repo.GetAllProducts(1, 2))
                .ReturnsAsync(mockProducts.Take(2));

            // Act
            var result = await _productService.GetAllProducts(1, 2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count()); // Should return 2 products for page 1
        }

        [Fact]
        public async Task GetAllProducts_WithPagination_ReturnsEmpty_WhenPageIsOutOfRange()
        {
            // Arrange
            int page = 10;
            int pageSize = 2; // Set page size to 2
            var mockProducts = new List<Product>
            {
                new() { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/mock1.jpg", Price = 10.0m, Description = "Mock Description 1" },
                new() { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/mock2.jpg", Price = 20.0m, Description = "Mock Description 2" },
                new() { Id = 3, Name = "Mock Product 3", ImgUri = "http://example.com/mock3.jpg", Price = 30.0m, Description = "Mock Description 3" }
            };

            // Setup the mock repository to return an empty list for an out-of-range page
            _mockProductRepository.Setup(repo => repo.GetAllProducts(10, 2))
                .ReturnsAsync([]);

            // Act
            var result = await _productService.GetAllProducts(page, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result); // Should return an empty list
        }

        [Fact]
        public async Task GetProductById_ReturnsProduct_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", ImgUri = "Test Uri" };
            _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(product);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productId, result.Id);
        }

        [Fact]
        public async Task GetProductById_ReturnsNull_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync((Product?)null);

            // Act
            var result = await _productService.GetProductById(productId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProductDescription_UpdatesDescription_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var description = "Updated Description";
            var product = new Product { Id = productId, Name = "Test Product", ImgUri = "Test Uri" };
            _mockProductRepository.Setup(repo => repo.GetProductById(productId)).ReturnsAsync(product);

            // Act
            await _productService.UpdateProductDescription(productId, description);

            // Assert
            _mockProductRepository.Verify(repo => repo.UpdateProductDescription(productId, description), Times.Once);
        }
    }
}