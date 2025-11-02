using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace DataAccess.Configurations
{
	public class AlbumEntityConfiguration : IEntityTypeConfiguration<Album>
	{
		public void Configure(EntityTypeBuilder<Album> builder)
		{
			builder.HasKey(a => a.Id);

			builder.Property(a => a.Title).IsRequired();

			builder.HasMany(a => a.Tracks)
				.WithOne(t => t.Album)
				.HasForeignKey(t => t.AlbumId);
		}
	}
}
