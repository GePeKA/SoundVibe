namespace Application.Abstractions.Services
{
	public interface IPasswordHasher
	{
		public string Hash(string input);
		public bool Verify(string input, string hashString);
	}
}
