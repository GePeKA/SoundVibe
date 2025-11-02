using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.MapperProfiles
{
	public class UserProfile: Profile
	{
		public UserProfile() 
		{
			CreateMap<User, PersonalInfoDto>();

			CreateMap<SignUpDto, User>()
				.ForMember(u => u.Role, dto => dto.MapFrom(src => "user"));

			CreateMap<User, Artist>()
				.ForMember(dest => dest.Biography, opt => opt.MapFrom(src => ""))
				.ForMember(dest => dest.Country, opt => opt.MapFrom(src => ""));

			CreateMap<Artist, ArtistInfoDto>();
		}
	}
}
