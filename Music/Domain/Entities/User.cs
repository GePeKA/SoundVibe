namespace Domain.Entities
{
	public class User
	{
		public long Id { get; set; }
		public string Nickname { get; set; } = null!;
		public string Role { get; set; } = null!;
		public string? ProfilePictureUrl { get; set; }
		public string Email { get; set; } = null!;
		public string Password { get; set; } = null!;

		public List<Genre> FavouriteGenres { get; set; }
		public List<FavouriteTrack> FavouriteTracks { get; set; }
		public List<FavouriteAlbum> FavouriteAlbums { get; set; }
	}
}
