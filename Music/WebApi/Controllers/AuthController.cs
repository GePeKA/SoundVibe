using Application.Abstractions.Services;
using Application.Dtos;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("auth")]
	public class AuthController(
		IUserService userService,
		IValidator<SignUpDto> signUpDtoValidator): Controller
	{
		[HttpPost("signup")]
		public async Task<IActionResult> SignUpAsync(SignUpDto dto)
		{
			var validationResult = await signUpDtoValidator.ValidateAsync(dto);

			if (!validationResult.IsValid)
			{
				return BadRequest(validationResult.Errors);
			}

			var userId = await userService.RegisterAsync(dto);

			return Created("", userId);
		}

		[HttpPost("signin")]
		public async Task<IActionResult> SignInAsync(LoginDto dto)
		{
			var accessToken = await userService.AuthenticateAsync(dto);

			return Ok(accessToken);
		}
	}
}
