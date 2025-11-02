using Application.Abstractions.Repositories;
using Domain.Entities;

namespace DataAccess.Repositories
{
	public class ArtistRepository(AppDbContext dbContext) : IArtistRepository
	{
		public async Task<Artist> AddArtistAsync(Artist artist)
		{
			var entry = await dbContext.Artists.AddAsync(artist);

			return entry.Entity;
		}

		public async Task<Artist?> GetArtistByIdAsync(long artistId)
		{
			return await dbContext.Artists.FindAsync(artistId);
		}
	}
}
