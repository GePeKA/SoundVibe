using Application.Abstractions.Services;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
	public static class InfrastructureRegisterExt
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services)
		{
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IPasswordHasher, PasswordHasher>();
			services.AddSingleton<IStorageService, S3StorageService>();

			return services;
		}
	}
}
