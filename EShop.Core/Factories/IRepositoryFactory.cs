namespace EShop.Core.Factories
{
    public interface IRepositoryFactory
    {
        IProductRepository CreateProductRepository(IServiceProvider serviceProvider);
    }
}
