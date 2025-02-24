using EShop.Core;
using EShop.Core.Factories;
using EShop.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EShop.Infrastructure.Factories
{
    public class RepositoryFactory(bool useMockData) : IRepositoryFactory
    {
        private readonly bool _useMockData = useMockData;

        public IProductRepository CreateProductRepository(IServiceProvider serviceProvider)
        {
            if (_useMockData)
            {
                return new MockProductRepository();
            }
            else
            {
                // Resolve the DbContextOptions from the service provider
                var dbContextOptions = serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>();
                return new ProductRepository(new AppDbContext(dbContextOptions));
            }
        }
    }
}