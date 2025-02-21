using Asp.Versioning;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace EShop.Api.Swagger
{
    /// <summary>
    /// Swagger config
    /// </summary>
    public static class SwaggerConfig
    {
        /// <summary>
        /// Configuring Swagger
        /// </summary>
        public static void AddSwaggerServices(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ReportApiVersions = true;
            });

            services.AddSwaggerGen(c =>
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
        }

        /// <summary>
        /// Configure the HTTP request pipeline.
        /// </summary>
        public static void UseSwaggerMiddleware(this IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    c.SwaggerEndpoint("/swagger/v2/swagger.json", "v2");
                });
            }
        }

    }
}