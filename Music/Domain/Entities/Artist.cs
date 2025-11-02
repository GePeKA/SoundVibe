namespace Domain.Entities
{
	public class Artist: User
	{
		public string? Biography { get; set; }
		public string? Country { get; set; }

		public List<Track> Tracks { get; set; }
		public List<Album> Albums { get; set; }
	}
}
