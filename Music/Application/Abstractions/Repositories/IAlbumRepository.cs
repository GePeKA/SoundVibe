using Application.Dtos;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
	public interface IAlbumRepository
	{
		Task<Album> AddAlbumWithTracksAsync(Album album, List<long> tracksIds);

		Task<Album?> GetAlbumByIdAsync(long albumId);

		Task<List<AlbumDto>> GetAllAlbumsByArtistId(long artistId);

		Task<List<AlbumDto>> GetFavouriteAlbumsByUserIdAsync(long userId);

		Task<List<AlbumDto>> GetAlbumsByNameWithinRange(string text, int from, int to);
	}
}
