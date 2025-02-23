using Asp.Versioning;
using EShop.Application;
using Microsoft.AspNetCore.Mvc;

namespace EShop.Api.Controllers.v2
{
    /// <summary>
    /// Products Controller
    /// </summary>
    /// <remarks>
    /// Products Controller constructor
    /// </remarks>
    [ApiController]
    [Route("api/v2/[controller]")]
    [ApiVersion("2.0")]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        private readonly IProductService _productService = productService;

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
    }
}