using Application.Abstractions.Services;
using Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("track")]
	[ApiController]
	public class TrackController(
		ITrackService trackService
		) : Controller
	{
		[Authorize]
		[HttpGet("get-track-by-id")]
		public async Task<IActionResult> GetTrackById(int trackId)
		{
			var trackDto = await trackService.GetTrackByIdAsync(trackId, GetUserId());

			return Ok(trackDto);
		}

		[HttpGet("get-tracks-by-search")]
		public async Task<IActionResult> GetTracksBySearch(string? text, int from, int to)
		{
			var idClaim = HttpContext.User.FindFirst("id");
			long? userId = idClaim == null? null : long.Parse(idClaim.Value);

			var trackDtos = await trackService.GetTracksBySearchAsync(text, from, to, userId);

			return Ok(trackDtos);
		}

		[Authorize]
		[HttpGet("get-favourite-tracks")]
		public async Task<IActionResult> GetFavouriteTracks()
		{
			var favouriteTracks = await trackService.GetFavouriteTracksAsync(GetUserId());

			return Ok(favouriteTracks);
		}

		[HttpGet("get-artist-tracks")]
		public async Task<IActionResult> GetArtistTracks(long artistId)
		{
			var idClaim = HttpContext.User.FindFirst("id");
			long? userId = idClaim == null ? null : long.Parse(idClaim.Value);
			var tracks = await trackService.GetArtistTracksAsync(artistId, userId);

			return Ok(tracks);
		}

		[Authorize]
		[HttpPost("get-recommendations")]
		public async Task<IActionResult> GetRecommendations([FromBody] ListenedTracksDto listenedTracks)
		{
			var recommendations = await trackService.GetRecommendationsAsync(GetUserId(), listenedTracks.TracksIds);

			return Ok(recommendations);
		}

		[Authorize]
		[HttpGet("get-tracks-by-album")]
		public async Task<IActionResult> GetTracksByAlbum(long albumId)
		{
			var tracks = await trackService.GetTracksByAlbumAsync(albumId, GetUserId());

			return Ok(tracks);
		}

		[Authorize]
		[HttpGet("get-track-infos-by-album")]
		public async Task<IActionResult> GetTrackInfosByAlbum(long albumId)
		{
			var tracks = await trackService.GetTracksWithoutUrlByAlbumAsync(albumId, GetUserId());

			return Ok(tracks);
		}

		[Authorize(Roles = "artist")]
		[HttpPost("upload-track")]
		public async Task<IActionResult> UploadTrack(
			IFormFile audioFile,
			IFormFile posterFile, 
			[FromForm] TrackUploadDto trackUploadDto)
		{
			var uploadedTrack = await trackService.UploadTrackAsync(
				audioFile.OpenReadStream(), audioFile.ContentType,
				posterFile.OpenReadStream(), posterFile.ContentType,
				trackUploadDto, GetUserId());

			return Ok(uploadedTrack);
		}

		[Authorize]
		[HttpPost("add-to-favourites")]
		public async Task<IActionResult> AddTrackToFavourites([FromBody] FavouriteDto dto)
		{
			await trackService.AddTrackToFavourites(GetUserId(), dto.Id);

			return Ok();
		}

		[Authorize]
		[HttpPost("remove-from-favourites")]
		public async Task<IActionResult> RemoveTrackFromFavourites([FromBody] FavouriteDto dto)
		{
			await trackService.RemoveTrackFromFavourites(GetUserId(), dto.Id);

			return Ok();
		}

		private long GetUserId()
		{
			return long.Parse(HttpContext.User.FindFirst("id")!.Value);
		}
	}
}
