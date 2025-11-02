using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class FavouriteAlbumEntityConfiguration : IEntityTypeConfiguration<FavouriteAlbum>
	{
		public void Configure(EntityTypeBuilder<FavouriteAlbum> builder)
		{
			builder.HasKey(fa => new { fa.UserId, fa.AlbumId });

			builder.HasOne(fa => fa.User)
				.WithMany(u => u.FavouriteAlbums)
				.HasForeignKey(fa => fa.UserId);

			builder.HasOne(fa => fa.Album)
				.WithMany()
				.HasForeignKey(fa => fa.AlbumId);
		}
	}
}
