public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllProducts(int page, int pageSize);
    Task<Product?> GetProductById(int id);
    Task UpdateProductDescription(int id, string description);
}
