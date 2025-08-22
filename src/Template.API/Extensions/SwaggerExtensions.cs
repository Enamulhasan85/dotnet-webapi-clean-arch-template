using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Template.API.Extensions
{
    /// <summary>
    /// Extensions for configuring Swagger/OpenAPI
    /// </summary>
    public static class SwaggerExtensions
    {
        /// <summary>
        /// Adds Swagger services with enhanced configuration
        /// </summary>
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Template API",
                    Version = "v1",
                    Description = "A Clean Architecture API Template with comprehensive features",
                    Contact = new OpenApiContact
                    {
                        Name = "API Support",
                        Email = "support@template.com"
                    },
                    License = new OpenApiLicense
                    {
                        Name = "MIT License",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });

                // Include XML comments
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Add JWT authentication
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

                // Group by tags
                c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
                c.DocInclusionPredicate((name, api) => true);

                // Custom operation filters
                c.OperationFilter<SwaggerDefaultValues>();
            });

            return services;
        }

        /// <summary>
        /// Configures Swagger UI with enhanced settings
        /// </summary>
        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Template API v1");
                c.RoutePrefix = "swagger";
                c.DocumentTitle = "Template API Documentation";
                c.DefaultModelsExpandDepth(-1);
                c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.None);
                c.EnableDeepLinking();
                c.EnableFilter();
                c.ShowExtensions();
                c.EnableValidator();
            });

            return app;
        }
    }

    /// <summary>
    /// Custom operation filter for Swagger
    /// </summary>
    public class SwaggerDefaultValues : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Add default responses
            if (!operation.Responses.ContainsKey("400"))
            {
                operation.Responses.Add("400", new OpenApiResponse
                {
                    Description = "Bad Request - Invalid input parameters"
                });
            }

            if (!operation.Responses.ContainsKey("500"))
            {
                operation.Responses.Add("500", new OpenApiResponse
                {
                    Description = "Internal Server Error"
                });
            }

            // Add authorization responses for secured endpoints
            var hasAuth = context.MethodInfo.DeclaringType?.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<Microsoft.AspNetCore.Authorization.AuthorizeAttribute>()
                .Any() ?? false;

            if (hasAuth)
            {
                if (!operation.Responses.ContainsKey("401"))
                {
                    operation.Responses.Add("401", new OpenApiResponse
                    {
                        Description = "Unauthorized - Authentication required"
                    });
                }

                if (!operation.Responses.ContainsKey("403"))
                {
                    operation.Responses.Add("403", new OpenApiResponse
                    {
                        Description = "Forbidden - Insufficient permissions"
                    });
                }
            }
        }
    }
}
