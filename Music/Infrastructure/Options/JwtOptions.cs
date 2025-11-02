namespace Infrastructure.Options
{
	public class JwtOptions
	{
		public string Key { get; set; } = null!;
		public int AccessTokenLifetimeInHours { get; set; }
		public int LongAccessTokenLifeTimeInHours { get; set; }
	}
}
