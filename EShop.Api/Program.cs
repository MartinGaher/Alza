using EShop.Api.Swagger;
using EShop.Core.Factories;
using EShop.Infrastructure.Factories;
using EShop.Mocks;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Entity Framework Core with SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
   options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
       b => b.MigrationsAssembly("EShop.Infrastructure"))); // Specify the migrations assembly

// Check if mock data should be used
bool useMockData = builder.Configuration.GetValue<bool>("UseMockData");

// Register the repository factory as scoped
builder.Services.AddScoped<IRepositoryFactory>(provider => new RepositoryFactory(useMockData));

// Register the repository based on the useMockData flag
if (useMockData)
{
    // Use the mock repository
    builder.Services.AddScoped<IProductRepository, MockProductRepository>();
}
else
{
    // Use the real repository
    builder.Services.AddScoped<IProductRepository, ProductRepository>();
}

// Use the factory to resolve the repository in your services
builder.Services.AddScoped<IProductRepository>(provider =>
{
    var factory = provider.GetRequiredService<IRepositoryFactory>();
    return factory.CreateProductRepository(provider);
});

// Dependency Injection
builder.Services.AddScoped<IProductService, ProductService>();

// Configure Swagger and API versioning
builder.Services.AddSwaggerServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwaggerMiddleware(app.Environment);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();