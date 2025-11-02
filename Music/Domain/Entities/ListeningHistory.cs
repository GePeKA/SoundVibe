namespace Domain.Entities
{
	public class ListeningHistory
	{
		public long Id { get; set; }
		public DateTimeOffset DateTimeListened { get; set; }

		public long UserId { get; set; }
		public User User { get; set; }

		public long TrackId { get; set; }
		public Track Track { get; set; }
	}
}
