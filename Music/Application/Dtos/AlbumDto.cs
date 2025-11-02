namespace Application.Dtos
{
	public class AlbumDto
	{
		public long Id { get; set; }
		public string Title { get; set; } = null!;
		public DateOnly ReleaseDate { get; set; }
		public bool IsFavourite { get; set; }
		public string AlbumCoverUrl { get; set; } = null!;
		public long ArtistId { get; set; }
		public string Artist { get; set; }
	}
}
