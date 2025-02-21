public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProducts();
    Task<Product?> GetProductById(int id);
    Task UpdateProductDescription(int id, string description);
}