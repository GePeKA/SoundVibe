using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class ListeningHistoryEntityConfiguration : IEntityTypeConfiguration<ListeningHistory>
	{
		public void Configure(EntityTypeBuilder<ListeningHistory> builder)
		{
			builder.HasKey(lh => lh.Id);

			builder.HasOne(lh => lh.Track)
				.WithMany()
				.HasForeignKey(lh => lh.TrackId);

			builder.HasOne(lh => lh.User)
				.WithMany()
				.HasForeignKey(lh => lh.TrackId);
		}
	}
}
