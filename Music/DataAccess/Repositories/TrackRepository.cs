using Application.Abstractions.Repositories;
using Application.Dtos;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories
{
	public class TrackRepository(AppDbContext dbContext) : ITrackRepository
	{
		public async Task<Track> AddTrackAsync(Track track)
		{
			var addedTrack = await dbContext.Tracks.AddAsync(track);
			return addedTrack.Entity;
		}

		public async Task<bool> CheckIfTracksPresentAndOwnedByArtist(long artistId, List<long> tracksIds)
		{
			var tracksOwnedByArtist = await dbContext.Tracks
				.Where(t => tracksIds.Contains(t.Id) && t.ArtistId == artistId)
				.Select(t => t.Id)
				.ToListAsync();

			return tracksOwnedByArtist.Count == tracksIds.Count;
		}

		public async Task<List<Track>> GetAllTracks()
		{
			return await dbContext.Tracks.Include(t => t.Artist).ToListAsync();
		}

		public async Task<List<TrackDto>> GetRecommendationsAsync(long userId, List<long> listenedTracks)
		{
			var favoriteAlbumsTracks = await dbContext.Users
				.Where(u => u.Id == userId)
				.Include(u => u.FavouriteAlbums)
					.ThenInclude(fa => fa.Album)
						.ThenInclude(a => a.Tracks)
				.SelectMany(u => u.FavouriteAlbums.SelectMany(a => a.Album.Tracks))
				.Where(t => !listenedTracks.Contains(t.Id))
				.OrderBy(t => EF.Functions.Random())
				.Take(3)
				.Select(t => new TrackDto
				{
					Id = t.Id,
					Title = t.Title,
					IsFavourite = dbContext.FavoriteTracks.Any(ft => ft.TrackId == t.Id && ft.UserId == userId),
					Artist = t.Artist.Nickname,
					ArtistId = t.ArtistId,
					Url = t.Url,
					TrackCoverUrl = t.TrackCoverUrl
				})
				.ToListAsync();

			var favouriteGenresTracks = await dbContext.Users
				.Where(u => u.Id == userId)
				.Include(u => u.FavouriteGenres)
				.SelectMany(u => u.FavouriteGenres
					.SelectMany(g => dbContext.Tracks
						.Where(t => t.GenreId == g.Id)))
				.Where(t => !listenedTracks.Contains(t.Id))
				.OrderBy(t => EF.Functions.Random())
				.Take(3)
				.Select(t => new TrackDto
				{
					Id = t.Id,
					Title = t.Title,
					IsFavourite = dbContext.FavoriteTracks.Any(ft => ft.TrackId == t.Id && ft.UserId == userId),
					Artist = t.Artist.Nickname,
					ArtistId = t.ArtistId,
					Url = t.Url,
					TrackCoverUrl = t.TrackCoverUrl
				})
				.ToListAsync();

			var userFavouriteTracks = dbContext.FavoriteTracks
				.Where(ft => ft.UserId == userId)
				.Select(ft => ft.TrackId);

			var usersWithSimilirarTastes = dbContext.FavoriteTracks
				.Where(ft => userFavouriteTracks.Contains(ft.TrackId) && ft.UserId != userId)
				.Select(ft => ft.UserId)
				.Distinct();

			var similarUsersTracks = await dbContext.FavoriteTracks
				.Where(ft => usersWithSimilirarTastes.Contains(ft.UserId))
				.Include(ft => ft.Track)
				.Where(ft => !listenedTracks.Contains(ft.TrackId))
				.OrderBy(t => EF.Functions.Random())
				.Select(ft => new TrackDto
				{
					Id = ft.Track.Id,
					Title = ft.Track.Title,
					IsFavourite = userFavouriteTracks.Contains(ft.TrackId),
					Artist = ft.Track.Artist.Nickname,
					ArtistId = ft.Track.ArtistId,
					Url = ft.Track.Url,
					TrackCoverUrl = ft.Track.TrackCoverUrl
				})
				.Distinct()
				.Take(3)
				.ToListAsync();

			var allRecommendedTracks = favoriteAlbumsTracks
				.Concat(favouriteGenresTracks)
				.Concat(similarUsersTracks)
				.Distinct()
				.ToList();

			if (allRecommendedTracks.Count == 0)
			{
				return await dbContext.Tracks
					.Where(t => !listenedTracks.Contains(t.Id))
					.Select(t => new TrackDto
					{
						Id = t.Id,
						Title = t.Title,
						IsFavourite = dbContext.FavoriteTracks.Any(ft => ft.TrackId == t.Id && ft.UserId == userId),
						Artist = t.Artist.Nickname,
						ArtistId = t.ArtistId,
						Url = t.Url,
						TrackCoverUrl = t.TrackCoverUrl
					})
					.Take(10)
					.ToListAsync();
			}

			return allRecommendedTracks;
		}

		public async Task<List<TrackDto>> GetAllTracksByArtistIdAsync(long artistId, long? userId)
		{
			List<TrackDto> tracks = new();

			if(userId.HasValue)
			{
				tracks = await dbContext.Tracks
					.Where(t => t.ArtistId == artistId)
					.Include(t => t.Artist)
					.Select(t => new TrackDto
					{
						Id = t.Id,
						Title = t.Title,
						IsFavourite = dbContext.FavoriteTracks.Any(ft => ft.TrackId == t.Id && ft.UserId == userId),
						Artist = t.Artist.Nickname,
						ArtistId = t.ArtistId,
						Url = t.Url,
						TrackCoverUrl = t.TrackCoverUrl
					})
					.ToListAsync();
			}
			else
			{
				tracks = tracks = await dbContext.Tracks
					.Where(t => t.ArtistId == artistId)
					.Include(t => t.Artist)
					.Select(t => new TrackDto
					{
						Id = t.Id,
						Title = t.Title,
						IsFavourite = false,
						Artist = t.Artist.Nickname,
						ArtistId = t.ArtistId,
						Url = t.Url,
						TrackCoverUrl = t.TrackCoverUrl
					})
					.ToListAsync();
			}

			return tracks;
		}


		public async Task<List<TrackDto>> GetTracksByAlbumIdAsync(long albumId, long userId)
		{
			var tracks = await dbContext.Tracks
					.AsNoTracking()
					.Include(t => t.Artist)
					.Where(t => t.AlbumId == albumId)
					.Select(t => new TrackDto
					{
						Id = t.Id,
						Title = t.Title,
						IsFavourite = dbContext.FavoriteTracks.Any(ft => ft.TrackId == t.Id && ft.UserId == userId),
						Artist = t.Artist.Nickname,
						ArtistId = t.ArtistId,
						Url = t.Url,
						TrackCoverUrl = t.TrackCoverUrl
					})
					.ToListAsync();

			return tracks;
		}

		public async Task<List<TrackDto>> GetFavouriteTracksByUserIdAsync(long userId)
		{
			var tracks = await dbContext.Tracks
				.Include(t => t.Artist)
				.Join(dbContext.FavoriteTracks.Where(ft => ft.UserId == userId),
					track => track.Id,
					favTrack => favTrack.TrackId,
					(track, favTrack) => new TrackDto()
					{
						Id = track.Id,
						Title = track.Title,
						IsFavourite = true,
						Artist = track.Artist.Nickname,
						ArtistId = track.ArtistId,
						Url = track.Url,
						TrackCoverUrl = track.TrackCoverUrl
					})
				.ToListAsync();

			return tracks;
		}

		public async Task<Track?> GetTrackByIdAsync(long id)
		{
			return await dbContext.Tracks.AsNoTracking()
				.Include(t => t.Artist)
				.SingleOrDefaultAsync(t => t.Id == id);
		}

		public async Task<List<TrackDto>> GetTracksByNameWithinRange(string text, int from, int to, long? userId)
		{
			List<TrackDto> tracks = new();
			if (userId.HasValue)
			{
				tracks = await dbContext.Tracks
					.AsNoTracking()
					.Include(t => t.Artist)
					.Where(t => EF.Functions.ILike(t.Title, $"%{text}%") || EF.Functions.ILike(t.Artist.Nickname, $"%{text}%"))
					.OrderBy(t => t.Id)
					.Skip(from)
					.Take(to-from)
					.Select(t => new TrackDto
					{
						Id = t.Id,
						Title = t.Title,
						IsFavourite = dbContext.FavoriteTracks.Any(ft => ft.TrackId == t.Id && ft.UserId == userId),
						Artist = t.Artist.Nickname,
						ArtistId = t.ArtistId,
						Url = t.Url,
						TrackCoverUrl = t.TrackCoverUrl
					})
					.ToListAsync();
			}
			else
			{
				tracks = await dbContext.Tracks
					.AsNoTracking()
					.Include(t => t.Artist)
					.Where(t => EF.Functions.ILike(t.Title, $"%{text}%") || EF.Functions.ILike(t.Artist.Nickname, $"%{text}%"))
					.OrderBy(t => t.Id)
					.Skip(from)
					.Take(to-from)
					.Select(t => new TrackDto
					{
						Id = t.Id,
						Title = t.Title,
						IsFavourite = false,
						Artist = t.Artist.Nickname,
						ArtistId = t.ArtistId,
						Url = t.Url,
						TrackCoverUrl = t.TrackCoverUrl
					})
					.ToListAsync();
			}

			return tracks;
		}
	}
}
