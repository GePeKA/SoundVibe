using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class FavouriteAlbumRepository(AppDbContext dbContext) : IFavouriteAlbumRepository
	{
		public async Task<FavouriteAlbum> AddToFavouriteAsync(FavouriteAlbum favouriteAlbum)
		{
			var added = await dbContext.FavouriteAlbums.AddAsync(favouriteAlbum);

			return added.Entity;
		}

		public async Task<FavouriteAlbum?> GetFavouriteAlbumEntityByUserIdAndAlbumId(long userId, long albumId)
		{
			return await dbContext.FavouriteAlbums.AsNoTracking()
				.SingleOrDefaultAsync(fa => fa.UserId == userId && fa.AlbumId == albumId);
		}

		public FavouriteAlbum RemoveFromFavourite(FavouriteAlbum favouriteAlbum)
		{
			var deleted = dbContext.FavouriteAlbums.Remove(favouriteAlbum);

			return deleted.Entity;
		}
	}
}
