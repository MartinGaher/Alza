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