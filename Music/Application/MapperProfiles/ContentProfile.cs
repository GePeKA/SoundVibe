using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.MapperProfiles
{
	public class ContentProfile: Profile
	{
		public ContentProfile()
		{
			CreateMap<TrackUploadDto, Track>();

			CreateMap<Track, TrackDto>()
				.ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Artist != null ? src.Artist.Nickname : null));

			CreateMap<Album, AlbumDto>()
				.ForMember(dest => dest.Artist, opt => opt.MapFrom(src => src.Artist != null ? src.Artist.Nickname : null));

			CreateMap<AlbumUploadDto, Album>();
		}
	}
}
