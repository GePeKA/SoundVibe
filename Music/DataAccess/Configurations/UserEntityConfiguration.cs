using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
	public class UserEntityConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(u => u.Id);

			builder.Property(u => u.Nickname).IsRequired();
			builder.Property(u => u.Role).IsRequired();
			builder.Property(u => u.Email).IsRequired();
			builder.Property(u => u.Password).IsRequired();

			builder.HasIndex(u => u.Email)
				.IsUnique();

			builder.HasMany(u => u.FavouriteGenres)
				.WithMany();
		}
	}
}
