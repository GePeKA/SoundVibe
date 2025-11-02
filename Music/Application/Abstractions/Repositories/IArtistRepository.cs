using Domain.Entities;

namespace Application.Abstractions.Repositories
{
	public interface IArtistRepository
	{
		Task<Artist> AddArtistAsync(Artist artist);

		Task<Artist?> GetArtistByIdAsync(long artistId);
	}
}
