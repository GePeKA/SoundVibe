namespace Application.Dtos
{
	public class TrackUploadDto
	{
		public string Title { get; set; } = null!;
		public string? Lyrics { get; set; }
		public long GenreId { get; set; }
	}
}
