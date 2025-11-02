using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class ArtistEntityConfiguration : IEntityTypeConfiguration<Artist>
	{
		public void Configure(EntityTypeBuilder<Artist> builder)
		{
			builder.ToTable("artists");

			builder.HasMany(a => a.Tracks)
				.WithOne(a => a.Artist)
				.HasForeignKey(a => a.ArtistId);

			builder.HasMany(a => a.Albums)
				.WithOne(al => al.Artist)
				.HasForeignKey(al => al.ArtistId);
		}
	}
}
