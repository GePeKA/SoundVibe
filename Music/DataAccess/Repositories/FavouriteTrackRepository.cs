using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class FavouriteTrackRepository(AppDbContext dbContext) : IFavouriteTrackRepository
	{
		public async Task<FavouriteTrack> AddToFavouriteAsync(FavouriteTrack favouriteTrack)
		{
			var added = await dbContext.FavoriteTracks.AddAsync(favouriteTrack);

			return added.Entity;
		}

		public async Task<FavouriteTrack?> GetFavouriteTrackEntityByUserIdAndTrackId(long userId, long trackId)
		{
			return await dbContext.FavoriteTracks.AsNoTracking()
				.SingleOrDefaultAsync(ft => ft.UserId == userId && ft.TrackId == trackId);
		}

		public FavouriteTrack RemoveFromFavourite(FavouriteTrack favouriteTrack)
		{
			var deleted = dbContext.FavoriteTracks.Remove(favouriteTrack);

			return deleted.Entity;
		}
	}
}
