using Microsoft.OpenApi.Models;

namespace Loan.Api.Extensions;

public static class SwaggerExtension
{
    private static readonly string[] value = ["Bearer"];

    public static void AddSwagger(this IServiceCollection services)
    {
        // This method gets called by the runtime from the startup "ConfigureServices()" to add swagger.
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Loan API",
                Version = "v1",
                Description = "Educational platform API with JWT authentication supporting both Bearer tokens and cookies."
            });

            var securitySchema = new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. You can also authenticate using cookies by logging in through the /api/auth/login endpoint.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };
            c.AddSecurityDefinition("Bearer", securitySchema);
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
              {
                      { securitySchema, value }
              });
        });
    }
}