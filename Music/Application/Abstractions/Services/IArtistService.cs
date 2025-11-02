using Application.Dtos;
using Domain.Entities;

namespace Application.Abstractions.Services
{
	public interface IArtistService
	{
		Task<ArtistInfoDto> GetArtistInfoAsync(long artistId);
	}
}
