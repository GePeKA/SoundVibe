using Application.Abstractions.Repositories;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccess.Extensions
{
	public static class DbRegisterExtension
	{
		public static IServiceCollection AddDbContext(this IServiceCollection serviceCollection,
		IConfiguration configuration)
		{
			serviceCollection.AddScoped<IUserRepository, UserRepository>();
			serviceCollection.AddScoped<IArtistRepository, ArtistRepository>();
			serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
			serviceCollection.AddScoped<ITrackRepository, TrackRepository>();
			serviceCollection.AddScoped<IFavouriteTrackRepository, FavouriteTrackRepository>();
			serviceCollection.AddScoped<IFavouriteAlbumRepository, FavouriteAlbumRepository>();
			serviceCollection.AddScoped<IAlbumRepository, AlbumRepository>();

			return serviceCollection.AddDbContext<AppDbContext>(builder =>
			{
				builder.UseNpgsql(configuration["Database:ConnectionString"]);
				builder.UseSnakeCaseNamingConvention();
			});
		}
	}
}
