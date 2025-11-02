namespace Domain.Entities
{
	public class Track
	{
		public long Id { get; set; }
		public string Title { get; set; } = null!;
		public DateOnly ReleaseDate { get; set; }
		public string Url { get; set; } = null!;
		public string TrackCoverUrl { get; set; } = null!;
		public string? Lyrics { get; set; }

		public long ArtistId { get; set; }
		public Artist Artist { get; set; }

		public long GenreId { get; set; }
		public Genre Genre { get; set; }

		public long? AlbumId { get; set; }
		public Album? Album { get; set; }
	}
}
