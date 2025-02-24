using EShop.Api.Controllers.v1;
using EShop.Application;
using EShop.Core;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EShop.Tests
{
    public class ProductsControllerTests
    {
        private readonly Mock<IProductService> _mockProductService;
        private readonly ProductsController _controller;
        private readonly Api.Controllers.v2.ProductsController _controllerV2;

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
            _controllerV2 = new Api.Controllers.v2.ProductsController(_mockProductService.Object);
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOkResult_WithListOfProducts()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new() { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/mock1.jpg", Price = 10.0m, Description = "Mock Description 1" },
                new() { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/mock2.jpg", Price = 20.0m, Description = "Mock Description 2" }
            };

            // Setup the mock service to return the mock products
            _mockProductService.Setup(service => service.GetAllProducts()).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count()); // Ensure the count matches the mock data
        }

        [Fact]
        public async Task GetAllProducts_ReturnsOkResult_WithEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var mockProducts = new List<Product>(); // No products

            // Setup the mock service to return an empty list
            _mockProductService.Setup(service => service.GetAllProducts()).ReturnsAsync(mockProducts);

            // Act
            var result = await _controller.GetAllProducts();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Empty(returnedProducts); // Ensure the returned list is empty
        }

        [Fact]
        public async Task GetAllProductsV2_ReturnsOkResult_WithPaginatedProducts()
        {
            // Arrange
            var mockProducts = new List<Product>
            {
                new() { Id = 1, Name = "Mock Product 1", ImgUri = "http://example.com/mock1.jpg", Price = 10.0m, Description = "Mock Description 1" },
                new() { Id = 2, Name = "Mock Product 2", ImgUri = "http://example.com/mock2.jpg", Price = 20.0m, Description = "Mock Description 2" }
            };

            // Setup the mock service to return the mock products for pagination
            _mockProductService.Setup(service => service.GetAllProducts(1, 2)).ReturnsAsync(mockProducts);

            // Act
            var result = await _controllerV2.GetAllProductsV2(1, 2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Equal(2, returnedProducts.Count()); // Ensure the count matches the mock data
        }

        [Fact]
        public async Task GetAllProductsV2_ReturnsOkResult_WithEmptyList_WhenNoProductsExist()
        {
            // Arrange
            var mockProducts = new List<Product>(); // No products

            // Setup the mock service to return an empty list
            _mockProductService.Setup(service => service.GetAllProducts(1, 2)).ReturnsAsync(mockProducts);

            // Act
            var result = await _controllerV2.GetAllProductsV2(1, 2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Empty(returnedProducts); // Ensure the returned list is empty
        }

        [Fact]
        public async Task GetAllProductsV2_ReturnsOkResult_WithEmptyList_WhenPageIsOutOfRange()
        {
            // Arrange
            var mockProducts = new List<Product>(); // No products

            // Setup the mock service to return an empty list for an out-of-range page
            _mockProductService.Setup(service => service.GetAllProducts(10, 2)).ReturnsAsync(mockProducts);

            // Act
            var result = await _controllerV2.GetAllProductsV2(10, 2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProducts = Assert.IsAssignableFrom<IEnumerable<Product>>(okResult.Value);
            Assert.Empty(returnedProducts); // Ensure the returned list is empty
        }

        [Fact]
        public async Task GetProductById_ReturnsOkResult_WhenProductExists()
        {
            // Arrange
            var productId = 1;
            var product = new Product { Id = productId, Name = "Test Product", ImgUri = "Test Uri" };
            _mockProductService.Setup(service => service.GetProductById(productId)).ReturnsAsync(product);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedProduct = Assert.IsType<Product>(okResult.Value);
            Assert.Equal(productId, returnedProduct.Id);
        }

        [Fact]
        public async Task GetProductById_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            _mockProductService.Setup(service => service.GetProductById(productId)).ReturnsAsync((Product?)null);

            // Act
            var result = await _controller.GetProductById(productId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateProductDescription_ReturnsNoContent_WhenUpdateIsSuccessful()
        {
            // Arrange
            var productId = 1;
            var description = "Updated Description";
            _mockProductService.Setup(service => service.UpdateProductDescription(productId, description)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateProductDescription(productId, description);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task UpdateProductDescription_ReturnsNotFound_WhenProductDoesNotExist()
        {
            // Arrange
            var productId = 1;
            var description = "Updated Description";
            _mockProductService.Setup(service => service.UpdateProductDescription(productId, description)).ThrowsAsync(new KeyNotFoundException());

            // Act
            var result = await _controller.UpdateProductDescription(productId, description);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}