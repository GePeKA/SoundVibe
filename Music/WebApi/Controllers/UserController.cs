using Application.Abstractions.Services;
using Application.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[ApiController]
	[Authorize]
	[Route("user")]
	public class UserController(IUserService userService) : Controller
	{
		[HttpGet("get-personal-info")]
		public async Task<IActionResult> GetPersonalInfo()
		{
			var userId = GetUserId();
			var personalInfo = await userService.GetPersonalInfoAsync(userId);

			return Ok(personalInfo);
		}

		[Authorize]
		[HttpPost("change-profile-picture")]
		public async Task<IActionResult> ChangeProfilePicture(IFormFile pictureFile)
		{
			await userService.ChangeProfilePictureAsync(
				pictureFile.OpenReadStream(),
				pictureFile.ContentType,
				GetUserId());

			return Ok();
		}

		[Authorize(Roles="user")]
		[HttpPost("become-artist")]
		public async Task<IActionResult> ChangeRoleOnArtist()
		{
			var userId = GetUserId();

			var createdArtistId = await userService.ChangeUserRoleOnArtist(userId);

			return Created("", createdArtistId);
		}

		[Authorize]
		[HttpPost("choose-favourite-genres")]
		public async Task<IActionResult> ChooseFavouriteUserGenres([FromBody] FavouriteGenresDto favouriteGenres)
		{
			await userService.ChooseFavouriteGenres(GetUserId(), favouriteGenres.GenresIds);

			return Ok();
		}

		private long GetUserId()
		{
			return long.Parse(HttpContext.User.FindFirst("id")!.Value);
		}
	}
}
