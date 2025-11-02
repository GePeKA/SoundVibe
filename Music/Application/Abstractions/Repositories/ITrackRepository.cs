using Application.Dtos;
using Domain.Entities;

namespace Application.Abstractions.Repositories
{
	public interface ITrackRepository
	{
		Task<Track> AddTrackAsync(Track track);

		Task<List<Track>> GetAllTracks();

		Task<List<TrackDto>> GetRecommendationsAsync(long userId, List<long> listenedTracks);

		Task<Track?> GetTrackByIdAsync(long id);

		Task<List<TrackDto>> GetAllTracksByArtistIdAsync(long artistId, long? userId = null);

		Task<List<TrackDto>> GetTracksByAlbumIdAsync(long albumId, long userId);

		Task<List<TrackDto>> GetFavouriteTracksByUserIdAsync(long userId);

		Task<List<TrackDto>> GetTracksByNameWithinRange(string text, int from, int to, long? userId = null);

		Task<bool> CheckIfTracksPresentAndOwnedByArtist(long artistId, List<long> tracksIds);
	}
}
