using Application.Abstractions.Services;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[Route("artist")]
	[ApiController]
	public class ArtistController(
		IArtistService artistService
		) : Controller
	{
		[HttpGet("get-artist-info")]
		public async Task<IActionResult> GetArtistInfo(long artistId)
		{
			var artistInfo = await artistService.GetArtistInfoAsync(artistId);

			return Ok(artistInfo);
		}
	}
}
