public interface IProductService
{
    Task<IEnumerable<Product>> GetAllProducts(int page = 1, int pageSize = 10);
    Task<Product?> GetProductById(int id);
    Task UpdateProductDescription(int id, string description);
}