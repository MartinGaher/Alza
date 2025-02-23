using Asp.Versioning;
using EShop.Application;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers.v1
{
    /// <summary>
    /// Products Controller
    /// </summary>
    /// <remarks>
    /// Products Controller constructor
    /// </remarks>
    [ApiController]
    [Route("api/v1/[controller]")]
    [ApiVersion("1.0")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>The list of products.</returns>
        [HttpGet]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllProducts() => Ok(await _productService.GetAllProducts());

        /// <summary>
        /// Gets a product by its ID.
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <returns>The product details.</returns>
        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var product = await _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            return Ok(product);
        }

        /// <summary>
        /// Update produst description
        /// </summary>
        /// <param name="id">The ID of the product.</param>
        /// <param name="description">The new description.</param>
        /// <returns>Success message.</returns>
        [HttpPatch("{id}/description")]
        [MapToApiVersion("1.0")]
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