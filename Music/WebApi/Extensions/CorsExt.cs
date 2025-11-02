namespace WebApi.Extensions
{
	public static class CorsExt
	{
		public static IServiceCollection AddCorsWithFrontendPolicy(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddCors(options =>
			{
				options.AddPolicy(name: "Frontend",
					policy =>
					{
						policy.WithOrigins("http://localhost:5173")
							.AllowAnyHeader()
							.AllowAnyMethod()
							.AllowCredentials();
					});
			});

			return serviceCollection;
		}
	}
}
