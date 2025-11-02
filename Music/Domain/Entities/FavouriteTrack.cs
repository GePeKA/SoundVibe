namespace Domain.Entities
{
	public class FavouriteTrack
	{
		public long UserId { get; set; }
		public User User { get; set; }

		public long TrackId { get; set; }
		public Track Track { get; set; }

		public DateTime DateAdded { get; set; }
	}
}
