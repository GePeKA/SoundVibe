using Application.Dtos;
using Domain.Entities;

namespace Application.Abstractions.Services
{
	public interface ITrackService
	{
		Task<TrackDto> GetTrackByIdAsync(long id, long userId);

		Task<List<TrackDto>> GetTracksBySearchAsync(string? text, int from, int to, long? userId = null);

		Task<List<TrackDto>> GetArtistTracksAsync(long artistId, long? userId = null);

		Task<List<TrackDto>> GetTracksByAlbumAsync(long albumId, long userId);

		Task<List<TrackDto>> GetTracksWithoutUrlByAlbumAsync(long albumId, long userId);

		Task<List<TrackDto>> GetRecommendationsAsync(long userId, List<long> listenedTracksIds);

		Task<List<TrackDto>> GetFavouriteTracksAsync(long userId);

		Task AddTrackToFavourites(long userId, long trackId);

		Task RemoveTrackFromFavourites(long userId, long trackId);

		Task<Track> UploadTrackAsync(Stream audioStream, string audioContentType,
			Stream pictureStream, string pictureContentType,
			TrackUploadDto dto, long artistId);
	}
}
