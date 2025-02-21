public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<IEnumerable<Product>> GetAllProducts()
    {
        return await _productRepository.GetAllProducts();
    }

    public async Task<Product?> GetProductById(int id)
    {
        return await _productRepository.GetProductById(id);
    }

    public async Task UpdateProductDescription(int id, string description)
    {
        var product = await _productRepository.GetProductById(id);
        if (product == null)
        {
            throw new KeyNotFoundException($"Product with ID {id} not found.");
        }

        product.Description = description;
        await _productRepository.UpdateProductDescription(id, description);
    }
}
