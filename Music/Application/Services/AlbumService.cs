using Application.Abstractions.Repositories;
using Application.Abstractions.Services;
using Application.Dtos;
using AutoMapper;
using Domain.Entities;

namespace Application.Services
{
	public class AlbumService(
		IAlbumRepository albumRepository,
		ITrackRepository trackRepository,
		IFavouriteAlbumRepository favouriteAlbumRepository,
		IMapper mapper,
		IUnitOfWork unitOfWork,
		IStorageService storageService
		) : IAlbumService
	{
		public async Task<AlbumDto> GetAlbumByIdAsync(long albumId, long? userId)
		{
			var album = await albumRepository.GetAlbumByIdAsync(albumId);

			if(album == null)
			{
				throw new Exception(); //TODO exception
			}

			var albumDto = mapper.Map<AlbumDto>(album);
			if (userId.HasValue)
			{
				albumDto.IsFavourite = (await favouriteAlbumRepository.GetFavouriteAlbumEntityByUserIdAndAlbumId(userId.Value, albumId)) != null;
			}
			albumDto.AlbumCoverUrl = await storageService.GetUrlAsync(albumDto.AlbumCoverUrl);

			return albumDto;
		}

		public async Task<List<AlbumDto>> GetAlbumsBySearchAsync(string? text, int from, int to)
		{
			var albums = await albumRepository.GetAlbumsByNameWithinRange(text, from, to);

			var dtoConfigureTasks = albums.Select(async a =>
			{
				a.AlbumCoverUrl = await storageService.GetUrlAsync(a.AlbumCoverUrl);

				return a;
			});

			return (await Task.WhenAll(dtoConfigureTasks)).ToList();
		}

		public async Task<List<AlbumDto>> GetArtistAlbumsAsync(long artistId)
		{
			var albums = await albumRepository.GetAllAlbumsByArtistId(artistId);

			var dtoConfigureTasks = albums.Select(async a =>
			{
				a.AlbumCoverUrl = await storageService.GetUrlAsync(a.AlbumCoverUrl);

				return a;
			});

			return (await Task.WhenAll(dtoConfigureTasks)).ToList();
		}

		public async Task<List<AlbumDto>> GetFavouriteAlbumsAsync(long userId)
		{
			var favouriteAlbums = await albumRepository.GetFavouriteAlbumsByUserIdAsync(userId);

			var dtoConfigureTasks = favouriteAlbums.Select(async a =>
			{
				a.AlbumCoverUrl = await storageService.GetUrlAsync(a.AlbumCoverUrl);

				return a;
			});

			return (await Task.WhenAll(dtoConfigureTasks)).ToList();
		}

		public async Task AddAlbumToFavourites(long userId, long albumId)
		{
			if ((await favouriteAlbumRepository.GetFavouriteAlbumEntityByUserIdAndAlbumId(userId, albumId)) != null)
			{
				throw new Exception(); //TODO handle
			}

			var favAlbum = new FavouriteAlbum()
			{
				AlbumId = albumId,
				UserId = userId
			};

			await favouriteAlbumRepository.AddToFavouriteAsync(favAlbum);

			await unitOfWork.SaveChangesAsync();
		}

		public async Task RemoveAlbumFromFavourites(long userId, long albumId)
		{
			var favAlbum = await favouriteAlbumRepository.GetFavouriteAlbumEntityByUserIdAndAlbumId(userId, albumId);

			if (favAlbum == null)
			{
				throw new Exception(); //TODO handle
			}

			favouriteAlbumRepository.RemoveFromFavourite(favAlbum);

			await unitOfWork.SaveChangesAsync();
		}

		public async Task<Album> UploadAlbumAsync(Stream pictureStream, string pictureContentType, AlbumUploadDto dto, long artistId)
		{
			if(!await trackRepository.CheckIfTracksPresentAndOwnedByArtist(artistId, dto.TracksIds))
			{
				throw new Exception(); //TODO exception
			}

			if(dto.TracksIds.Count == 0)
			{
				throw new Exception(); //TODO exception
			}

			var pictureFileName = Guid.NewGuid().ToString();

			if (!IsValidImageContentType(pictureContentType))
			{
				throw new Exception(); //TODO обработка 
			}

			var pictureUploadSuccess = await storageService.PutAsync(pictureFileName, pictureStream, pictureContentType);

			if (!pictureUploadSuccess)
			{
				throw new Exception(); //TODO обработка 
			}

			var newAlbum = mapper.Map<Album>(dto);

			newAlbum.ArtistId = artistId;
			newAlbum.ReleaseDate = DateOnly.FromDateTime(DateTime.Now);
			newAlbum.AlbumCoverUrl = pictureFileName;

			var addedAlbum = await albumRepository.AddAlbumWithTracksAsync(newAlbum, dto.TracksIds);

			if (addedAlbum == null)
			{
				throw new Exception(); //TODO обработка
			}

			await unitOfWork.SaveChangesAsync();

			return addedAlbum;
		}

		private bool IsValidImageContentType(string contentType)
		{
			return contentType.StartsWith("image/");
		}
	}
}
