namespace Application.Dtos
{
	public class AlbumUploadDto
	{
		public string Title { get; set; } = null!;
		public List<long> TracksIds { get; set; }
	}
}
