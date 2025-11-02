using DataAccess.Configurations;
using DataAccess.Extensions;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
	public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
	{
		public DbSet<User> Users => Set<User>();
		public DbSet<Artist> Artists => Set<Artist>();

		public DbSet<Track> Tracks => Set<Track>();
		public DbSet<FavouriteTrack> FavoriteTracks => Set<FavouriteTrack>();

		public DbSet<Album> Albums => Set<Album>();
		public DbSet<FavouriteAlbum> FavouriteAlbums => Set<FavouriteAlbum>();

		public DbSet<ListeningHistory> ListeningHistory => Set<ListeningHistory>();

		public DbSet<Genre> Genres => Set<Genre>();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfiguration(new AlbumEntityConfiguration());
			modelBuilder.ApplyConfiguration(new ArtistEntityConfiguration());
			modelBuilder.ApplyConfiguration(new FavouriteAlbumEntityConfiguration());
			modelBuilder.ApplyConfiguration(new FavouriteTrackEntityConfiguration());
			modelBuilder.ApplyConfiguration(new ListeningHistoryEntityConfiguration());
			modelBuilder.ApplyConfiguration(new TrackEntityConfiguration());
			modelBuilder.ApplyConfiguration(new UserEntityConfiguration());

			modelBuilder.SeedWithInitialData();

			base.OnModelCreating(modelBuilder);
		}
	}
}
