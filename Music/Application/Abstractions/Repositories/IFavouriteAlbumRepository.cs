using Domain.Entities;

namespace Application.Abstractions.Repositories
{
	public interface IFavouriteAlbumRepository
	{
		Task<FavouriteAlbum?> GetFavouriteAlbumEntityByUserIdAndAlbumId(long userId, long albumId);

		Task<FavouriteAlbum> AddToFavouriteAsync(FavouriteAlbum favouriteAlbum);

		FavouriteAlbum RemoveFromFavourite(FavouriteAlbum favouriteAlbum);
	}
}
