using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers.v2
{
    /// <summary>
    /// Products Controller
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion("2.0")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        /// <summary>
        /// Products Controller constructor
        /// </summary>
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Gets all products v2
        /// </summary>
        /// <returns>The list of products.</returns>
        [HttpGet]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetAllProductsV2(int page = 1, int pageSize = 10)
        {
            var products = await _productService.GetAllProducts(page, pageSize);
            return Ok(products);
        }
        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product details.</returns>
        [HttpGet("{id}")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null) return NotFound();
            return Ok(product);
        }

        /// <summary>
        /// Update produst description
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <param name="description">The new description.</param>
        /// <returns>Success message.</returns>
        [HttpPatch("{id}/description")]
        [MapToApiVersion("2.0")]
        public async Task<IActionResult> UpdateProductDescription(int id, [FromBody] string description)
        {
            try
            {
                await _productService.UpdateProductDescription(id, description);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }
}