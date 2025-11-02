namespace Domain.Entities
{
	public class Album
	{
		public long Id { get; set; }

		public string Title { get; set; } = null!;
		public DateOnly ReleaseDate { get; set; }
		public string AlbumCoverUrl { get; set; } = null!;

		public long ArtistId { get; set; }
		public Artist Artist { get; set; }

		public List<Track> Tracks { get; set; }
	}
}
