using Microsoft.OpenApi.Models;

namespace WebApi.Extensions
{
	public static class SwaggerExt
	{
		public static IServiceCollection AddSwaggerGenWithBearer(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					Type = SecuritySchemeType.ApiKey,
					Description = """
                              Authorization using JWT by adding header
                              Authorization: Bearer [token]
                              """,
					Name = "Authorization",
					In = ParameterLocation.Header,
					Scheme = "Bearer"
				});
				options.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Name = "Bearer",
							In = ParameterLocation.Header,
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							},
						},
						Array.Empty<string>()
					}
				});
			});

			return serviceCollection;
		}
	}
}
