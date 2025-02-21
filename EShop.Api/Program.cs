using Asp.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
            b => b.MigrationsAssembly("EShop.Infrastructure"))); // Specify the migrations assembly
builder.Services.AddScoped<IProductRepository, ProductRepository>();

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

// Dependency Injection
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

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
