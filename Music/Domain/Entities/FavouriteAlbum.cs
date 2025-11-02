namespace Domain.Entities
{
	public class FavouriteAlbum
	{
		public long UserId { get; set; }
		public User User { get; set; }

		public long AlbumId { get; set; }
		public Album Album { get; set; }
	}
}
