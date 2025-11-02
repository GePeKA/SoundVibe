using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class TrackEntityConfiguration : IEntityTypeConfiguration<Track>
	{
		public void Configure(EntityTypeBuilder<Track> builder)
		{
			builder.HasKey(t => t.Id);

			builder.Property(t => t.Title).IsRequired();
			builder.Property(t => t.Url).IsRequired();

			builder.HasOne(t => t.Genre)
				.WithMany();
		}
	}
}
