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

        public ProductsControllerTests()
        {
            _mockProductService = new Mock<IProductService>();
            _controller = new ProductsController(_mockProductService.Object);
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