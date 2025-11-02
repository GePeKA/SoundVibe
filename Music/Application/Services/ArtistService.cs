using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos;
using AutoMapper;

namespace Application.Services
{
	public class ArtistService(
		IArtistRepository artistRepository,
		IStorageService storageService,
		IMapper mapper
		) : IArtistService
	{
		public async Task<ArtistInfoDto> GetArtistInfoAsync(long artistId)
		{
			var artist = await artistRepository.GetArtistByIdAsync(artistId);

			if (artist == null)
			{
				throw new Exception(); //TODO exception
			}

			var artistInfoDto = mapper.Map<ArtistInfoDto>(artist);

			if(artistInfoDto.ProfilePictureUrl != null)
			{
				artistInfoDto.ProfilePictureUrl = await storageService.GetUrlAsync(artistInfoDto.ProfilePictureUrl);
			}

			return artistInfoDto;
		}
	}
}
