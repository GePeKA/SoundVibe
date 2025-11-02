using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
	public class TrackService(
		ITrackRepository trackRepository,
		IFavouriteTrackRepository favouriteTrackRepository,
		IUnitOfWork unitOfWork,
		IStorageService storageService,
		IMapper mapper): ITrackService
	{
		public async Task<List<TrackDto>> GetRecommendationsAsync(long userId, List<long> listenedTracksIds)
		{
			var random = new Random();

			var tracks = await trackRepository.GetRecommendationsAsync(userId, listenedTracksIds);
			var randomOrderedTracks = tracks.OrderBy(x => random.Next());
			
			var dtoConfigureTasks = randomOrderedTracks.Select(async t =>
			{
				t.TrackCoverUrl = await storageService.GetUrlAsync(t.TrackCoverUrl);
				t.Url = await storageService.GetUrlAsync(t.Url);

				return t;
			});

			return (await Task.WhenAll(dtoConfigureTasks)).ToList();
		}

		public async Task<TrackDto> GetTrackByIdAsync(long id, long userId)
		{
			var track = await trackRepository.GetTrackByIdAsync(id);

			if (track == null)
			{
				throw new Exception();
			}

			var trackDto = mapper.Map<TrackDto>(track);

			trackDto.IsFavourite = (await favouriteTrackRepository.GetFavouriteTrackEntityByUserIdAndTrackId(userId, id)) != null;

			trackDto.Url = await storageService.GetUrlAsync(trackDto.Url);
			trackDto.TrackCoverUrl = await storageService.GetUrlAsync(trackDto.TrackCoverUrl);
			trackDto.Artist = track.Artist.Nickname;

			return trackDto;
		}

		public async Task<List<TrackDto>> GetTracksBySearchAsync(string? text, int from, int to, long? userId = null)
		{
			var tracks = await trackRepository.GetTracksByNameWithinRange(text, from, to, userId);

			var dtoConfigureTasks = tracks.Select(async t =>
			{
				t.TrackCoverUrl = await storageService.GetUrlAsync(t.TrackCoverUrl);

				return t;
			});

			return (await Task.WhenAll(dtoConfigureTasks)).ToList();
		}

		public async Task<List<TrackDto>> GetArtistTracksAsync(long artistId, long? userId = null)
		{
			var tracks = await trackRepository.GetAllTracksByArtistIdAsync(artistId, userId);

			var dtoConfigureTasks = tracks.Select(async t =>
			{
				t.TrackCoverUrl = await storageService.GetUrlAsync(t.TrackCoverUrl);

				return t;
			});

			return (await Task.WhenAll(dtoConfigureTasks)).ToList();
		}

		public async Task<List<TrackDto>> GetTracksByAlbumAsync(long albumId, long userId)
		{
			var tracks = await trackRepository.GetTracksByAlbumIdAsync(albumId, userId);

			var dtoConfigureTasks = tracks.Select(async t =>
			{
				t.TrackCoverUrl = await storageService.GetUrlAsync(t.TrackCoverUrl);
				t.Url = await storageService.GetUrlAsync(t.Url);

				return t;
			});

			return (await Task.WhenAll(dtoConfigureTasks)).ToList();
		}

		public async Task<List<TrackDto>> GetTracksWithoutUrlByAlbumAsync(long albumId, long userId)
		{
			var tracks = await trackRepository.GetTracksByAlbumIdAsync(albumId, userId);

			var dtoConfigureTasks = tracks.Select(async t =>
			{
				t.TrackCoverUrl = await storageService.GetUrlAsync(t.TrackCoverUrl);

				return t;
			});

			return (await Task.WhenAll(dtoConfigureTasks)).ToList();
		}

		public async Task<List<TrackDto>> GetFavouriteTracksAsync(long userId)
		{
			var favouriteTracks = await trackRepository.GetFavouriteTracksByUserIdAsync(userId);

			var dtoConfigureTasks = favouriteTracks.Select(async t =>
			{
				t.TrackCoverUrl = await storageService.GetUrlAsync(t.TrackCoverUrl);

				return t;
			});

			return (await Task.WhenAll(dtoConfigureTasks)).ToList();
		}

		public async Task AddTrackToFavourites(long userId, long trackId)
		{
			if ((await favouriteTrackRepository.GetFavouriteTrackEntityByUserIdAndTrackId(userId, trackId)) != null)
			{
				throw new Exception(); //TODO handle
			}

			var favTrack = new FavouriteTrack()
			{
				UserId = userId,
				TrackId = trackId,
				DateAdded = DateTime.UtcNow
			};

			await favouriteTrackRepository.AddToFavouriteAsync(favTrack);

			await unitOfWork.SaveChangesAsync();
		}

		public async Task RemoveTrackFromFavourites(long userId, long trackId)
		{
			var favTrack = await favouriteTrackRepository.GetFavouriteTrackEntityByUserIdAndTrackId(userId, trackId);

			if (favTrack == null)
			{
				throw new Exception(); //TODO handle
			}

			favouriteTrackRepository.RemoveFromFavourite(favTrack);

			await unitOfWork.SaveChangesAsync();
		}

		public async Task<Track> UploadTrackAsync(Stream audioStream, string audioContentType,
			Stream pictureStream, string pictureContentType,
			TrackUploadDto dto, long artistId)
		{
			var audioFileName = Guid.NewGuid().ToString();
			var pictureFileName = Guid.NewGuid().ToString();

			if (!IsValidAudioContentType(audioContentType))
			{
				throw new Exception(); //TODO обработка 
			}

			if (!IsValidImageContentType(pictureContentType))
			{
				throw new Exception(); //TODO обработка 
			}

			var audioUploadSuccess = await storageService.PutAsync(audioFileName, audioStream, audioContentType);
			var pictureUploadSuccess = await storageService.PutAsync(pictureFileName, pictureStream, pictureContentType);

			if(!audioUploadSuccess || !pictureUploadSuccess)
			{
				throw new Exception(); //TODO обработка 
			}

			var newTrack = mapper.Map<Track>(dto);

			newTrack.ArtistId = artistId;
			newTrack.ReleaseDate = DateOnly.FromDateTime(DateTime.Now);
			newTrack.Url = audioFileName;
			newTrack.TrackCoverUrl = pictureFileName;

			var addedTrack = await trackRepository.AddTrackAsync(newTrack);

			if (addedTrack == null)
			{
				throw new Exception(); //TODO обработка
			}

			await unitOfWork.SaveChangesAsync();

			return addedTrack;
		}

		private bool IsValidAudioContentType(string contentType)
		{
			return contentType.StartsWith("audio/");
		}

		private bool IsValidImageContentType(string contentType)
		{
			return contentType.StartsWith("image/");
		}
	}
}
