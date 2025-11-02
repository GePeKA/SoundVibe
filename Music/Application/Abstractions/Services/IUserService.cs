using Application.Dtos;

namespace Application.Abstractions.Services
{
	public interface IUserService
	{
		Task<PersonalInfoDto> GetPersonalInfoAsync(long userId);

		Task ChangeProfilePictureAsync(Stream pictureStream, string pictureContentType, long userId);

		Task<long?> RegisterAsync(SignUpDto signUpDto);

		Task<string> AuthenticateAsync(LoginDto loginDto);

		Task<long> ChangeUserRoleOnArtist(long userId);

		Task ChooseFavouriteGenres(long userId, List<long> genreIds);
	}
}
