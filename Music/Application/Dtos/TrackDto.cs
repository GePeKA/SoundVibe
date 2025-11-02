namespace Application.Dtos
{
	public class TrackDto
	{
		public long Id { get; set; }
		public string Title { get; set; }
		public bool IsFavourite { get; set; }
		public string Artist { get; set; }
		public long ArtistId { get; set; }
		public string Url { get; set; }
		public string TrackCoverUrl { get; set; }
	}
}
