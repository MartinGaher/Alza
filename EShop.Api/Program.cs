using Asp.Versioning;
using EShop.Core.Factories;
using EShop.Infrastructure.Factories;
using EShop.Mocks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EShop API", Version = "v1" });
    c.SwaggerDoc("v2", new OpenApiInfo { Title = "EShop API", Version = "v2" });

    // Include XML comments if applicable
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"; // Adjust if necessary
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Add a versioned API endpoint
    c.DocInclusionPredicate((version, apiDescription) =>
    {
        var actionApiVersionModel = apiDescription.ActionDescriptor
            .EndpointMetadata
            .OfType<ApiVersionAttribute>()
            .FirstOrDefault();

        if (actionApiVersionModel != null)
        {
            return actionApiVersionModel.Versions.Any(v => $"v{v.MajorVersion}" == version);
        }

        return false;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
        c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
