using Application.Abstractions.Services;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
	public static class ServicesRegisterExt
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IAlbumService, AlbumService>();
			serviceCollection.AddScoped<ITrackService, TrackService>();
			serviceCollection.AddScoped<IArtistService, ArtistService>();

			return serviceCollection;
		}
	}
}
