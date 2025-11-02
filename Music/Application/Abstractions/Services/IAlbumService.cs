using Application.Dtos;
using Domain.Entities;

namespace Application.Abstractions.Services
{
	public interface IAlbumService
	{
		Task<List<AlbumDto>> GetAlbumsBySearchAsync(string? text, int from, int to);

		Task<List<AlbumDto>> GetArtistAlbumsAsync(long artistId);

		Task<AlbumDto> GetAlbumByIdAsync(long albumId, long? userId = null);

		Task<List<AlbumDto>> GetFavouriteAlbumsAsync(long userId);

		Task AddAlbumToFavourites(long userId, long albumId);

		Task RemoveAlbumFromFavourites(long userId, long albumId);

		Task<Album> UploadAlbumAsync(Stream pictureStream, string pictureContentType,
			AlbumUploadDto dto, long artistId);
	}
}
