using Application.Abstractions.Services;
using Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("album")]
	[ApiController]
	public class AlbumController(IAlbumService albumService) : Controller
	{
		[HttpGet("get-album-by-id")]
		public async Task<IActionResult> GetAlbumById(int albumId)
		{
			var idClaim = HttpContext.User.FindFirst("id");
			long? userId = idClaim == null ? null : long.Parse(idClaim.Value);

			var albumDto = await albumService.GetAlbumByIdAsync(albumId, userId);

			return Ok(albumDto);
		}

		[HttpGet("get-albums-by-search")]
		public async Task<IActionResult> GetAlbumsBySearch(string? text, int from, int to)
		{
			var albumDtos = await albumService.GetAlbumsBySearchAsync(text, from, to);

			return Ok(albumDtos);
		}

		[Authorize]
		[HttpGet("get-favourite-albums")]
		public async Task<IActionResult> GetFavouriteAlbums()
		{
			var favouriteAlbums = await albumService.GetFavouriteAlbumsAsync(GetUserId());

			return Ok(favouriteAlbums);
		}

		[HttpGet("get-artist-albums")]
		public async Task<IActionResult> GetArtistAlbums(long artistId)
		{
			var albums = await albumService.GetArtistAlbumsAsync(artistId);

			return Ok(albums);
		}

		[Authorize(Roles = "artist")]
		[HttpPost("upload-album")]
		public async Task<IActionResult> UploadAlbum(
			IFormFile posterFile,
			[FromForm] AlbumUploadDto albumUploadDto)
		{
			var uploadedAlbum = await albumService.UploadAlbumAsync(
				posterFile.OpenReadStream(), 
				posterFile.ContentType, albumUploadDto, GetUserId());

			return Ok();
		}

		[Authorize]
		[HttpPost("add-to-favourites")]
		public async Task<IActionResult> AddTrackToFavourites([FromBody] FavouriteDto dto)
		{
			await albumService.AddAlbumToFavourites(GetUserId(), dto.Id);

			return Ok();
		}

		[Authorize]
		[HttpPost("remove-from-favourites")]
		public async Task<IActionResult> RemoveTrackFromFavourites([FromBody] FavouriteDto dto)
		{
			await albumService.RemoveAlbumFromFavourites(GetUserId(), dto.Id);

			return Ok();
		}

		private long GetUserId()
		{
			return long.Parse(HttpContext.User.FindFirst("id")!.Value);
		}
	}
}
