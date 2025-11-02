using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos;
using Domain.Entities;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using Infrastructure.Options;
using AutoMapper;

namespace Infrastructure.Services
{
	public class UserService(
		IUserRepository userRepository,
		IArtistRepository artistRepository,
		IUnitOfWork unitOfWork,
		IPasswordHasher passwordHasher,
		IStorageService storageService,
		IMapper mapper,
		IOptionsMonitor<JwtOptions> jwtOptionsMonitor) : IUserService
	{
		private JwtOptions _jwtOptions = jwtOptionsMonitor.CurrentValue;

		public async Task<string> AuthenticateAsync(LoginDto loginDto)
		{
			var user = await userRepository.GetUserByFilterAsync(u => u.Email == loginDto.Email);
			if (user == null)
			{
				throw new Exception(); //TODO обработать исключение
			}

			if (!passwordHasher.Verify(loginDto.Password, user.Password))
			{
				throw new Exception(); //TODO обработать исключение
			}

			var token = GenerateAccessToken(user, loginDto.RememberMe);

			return token;
		}

		public async Task<PersonalInfoDto> GetPersonalInfoAsync(long userId)
		{
			var user = await userRepository.GetUserByFilterAsync(u => u.Id == userId);

			if (user == null)
			{
				throw new Exception(); //TODO обработать исключение
			}

			var infoDto = mapper.Map<PersonalInfoDto>(user);

			if (infoDto.ProfilePictureUrl != null)
			{
				infoDto.ProfilePictureUrl = await storageService.GetUrlAsync(infoDto.ProfilePictureUrl);
			}

			return infoDto;
		}

		public async Task<long?> RegisterAsync(SignUpDto signUpDto)
		{
			if (!await userRepository.IsEmailUniqueAsync(signUpDto.Email)) 
			{
				throw new Exception(); //TODO обработать исключение
			}

			signUpDto.Password = passwordHasher.Hash(signUpDto.Password);

			var createdUser = await userRepository.AddUserAsync(mapper.Map<User>(signUpDto));

			await unitOfWork.SaveChangesAsync();

			return createdUser.Id;
		}

		public async Task<long> ChangeUserRoleOnArtist(long userId)
		{
			var user = await userRepository.GetUserByFilterAsync(u => u.Id == userId);

			if (user == null)
			{
				throw new Exception(); //TODO обработать исключение
			}

			var deletedUser = userRepository.DeleteUser(user);

			var createdArtist = await artistRepository.AddArtistAsync(mapper.Map<Artist>(user));

			createdArtist.Role = "artist";

			await unitOfWork.SaveChangesAsync();

			return createdArtist.Id;
		}

		private string GenerateAccessToken(User user, bool isLongToken)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(_jwtOptions.Key);
			var descriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim("id", user.Id.ToString()),
					new Claim(ClaimTypes.Role, user.Role)
				}),

				Expires = DateTime.UtcNow.AddHours(isLongToken ? _jwtOptions.LongAccessTokenLifeTimeInHours: _jwtOptions.AccessTokenLifetimeInHours),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			var token = tokenHandler.CreateToken(descriptor);
			return tokenHandler.WriteToken(token);
		}

		public async Task ChangeProfilePictureAsync(Stream pictureStream, string pictureContentType, long userId)
		{
			var pictureFileName = Guid.NewGuid().ToString();

			if (!IsValidImageContentType(pictureContentType))
			{
				throw new Exception(); //TODO exception
			}

			var pictureUploadSuccess = await storageService.PutAsync(pictureFileName, pictureStream, pictureContentType);

			if (!pictureUploadSuccess)
			{
				throw new Exception(); //TODO exception
			}

			var user = await userRepository.GetUserByFilterAsync(u => u.Id == userId);

			if (user == null)
			{
				throw new Exception(); //TODO exception
			}

			user.ProfilePictureUrl = pictureFileName;

			await unitOfWork.SaveChangesAsync();
		}
		
		public async Task ChooseFavouriteGenres(long userId, List<long> genreIds)
		{
			var user = await userRepository.GetUserByFilterAsync(u => u.Id == userId);

			if(genreIds.Count == 0)
			{
				throw new Exception(); //TODO exception
			}

			if(user == null)
			{
				throw new Exception(); //TODO exception
			}

			await userRepository.AddUserFavouriteGenres(user, genreIds);

			await unitOfWork.SaveChangesAsync();
		}

		private bool IsValidImageContentType(string contentType)
		{
			return contentType.StartsWith("image/");
		}

		
	}
}
