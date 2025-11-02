using Domain.Entities;

namespace Application.Abstractions.Repositories
{
	public interface IFavouriteTrackRepository
	{
		Task<FavouriteTrack?> GetFavouriteTrackEntityByUserIdAndTrackId(long userId, long trackId);

		Task<FavouriteTrack> AddToFavouriteAsync(FavouriteTrack favouriteTrack);

		FavouriteTrack RemoveFromFavourite(FavouriteTrack favouriteTrack);
	}
}
