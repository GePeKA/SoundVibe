using Application.Abstractions.Repositories;
using Application.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class AlbumRepository(AppDbContext dbContext) : IAlbumRepository
	{
		public async Task<Album> AddAlbumWithTracksAsync(Album album, List<long> tracksIds)
		{
			var tracks = await dbContext.Tracks.Where(t => tracksIds.Contains(t.Id)).ToListAsync();
			album.Tracks = tracks;

			var addedAlbum = await dbContext.Albums.AddAsync(album);

			return addedAlbum.Entity;
		}

		public async Task<Album?> GetAlbumByIdAsync(long albumId)
		{
			return await dbContext.Albums.AsNoTracking()
				.Include(a => a.Artist)
				.SingleOrDefaultAsync(a => a.Id == albumId);
		}

		public async Task<List<AlbumDto>> GetAlbumsByNameWithinRange(string text, int from, int to)
		{
			var albums = await dbContext.Albums
				.AsNoTracking()
				.Include(a => a.Artist)
				.Where(a => EF.Functions.ILike(a.Title, $"%{text}%") || EF.Functions.ILike(a.Artist.Nickname, $"%{text}%"))
				.OrderBy(a => a.Id)
				.Skip(from)
				.Take(to-from)
				.Select(a => new AlbumDto
				{
					Id = a.Id,
					Title = a.Title,
					ReleaseDate = a.ReleaseDate,
					IsFavourite = false,
					AlbumCoverUrl = a.AlbumCoverUrl,
					ArtistId = a.ArtistId,
					Artist = a.Artist.Nickname
				})
				.ToListAsync();

			return albums;
		}

		public async Task<List<AlbumDto>> GetAllAlbumsByArtistId(long artistId)
		{
			var albums = await dbContext.Albums
				.Where(a => a.ArtistId == artistId)
				.Include(a => a.Artist)
				.Select(a => new AlbumDto
				{
					Id = a.Id,
					Title = a.Title,
					ReleaseDate = a.ReleaseDate,
					IsFavourite = false,
					AlbumCoverUrl = a.AlbumCoverUrl,
					ArtistId = a.ArtistId,
					Artist = a.Artist.Nickname
				})
				.ToListAsync();

			return albums;
		}

		public async Task<List<AlbumDto>> GetFavouriteAlbumsByUserIdAsync(long userId)
		{
			var albums = await dbContext.Albums
				.Include(t => t.Artist)
				.Join(dbContext.FavouriteAlbums.Where(fa => fa.UserId == userId),
					album => album.Id,
					favAlbum => favAlbum.AlbumId,
					(a, favAlbum) => new AlbumDto()
					{
						Id = a.Id,
						Title = a.Title,
						ReleaseDate = a.ReleaseDate,
						IsFavourite = true,
						AlbumCoverUrl = a.AlbumCoverUrl,
						ArtistId = a.ArtistId,
						Artist = a.Artist.Nickname
					})
				.ToListAsync();

			return albums;
		}
	}
}
