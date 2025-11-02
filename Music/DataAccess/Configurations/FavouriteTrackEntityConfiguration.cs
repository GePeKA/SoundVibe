using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class FavouriteTrackEntityConfiguration : IEntityTypeConfiguration<FavouriteTrack>
	{
		public void Configure(EntityTypeBuilder<FavouriteTrack> builder)
		{
			builder.HasKey(ft => new { ft.UserId, ft.TrackId });

			builder.HasOne(ft => ft.User)
				.WithMany(u => u.FavouriteTracks)
				.HasForeignKey(u => u.UserId);

			builder.HasOne(ft => ft.Track)
				.WithMany()
				.HasForeignKey(ft => ft.TrackId);
		}
	}
}
