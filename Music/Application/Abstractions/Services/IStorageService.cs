namespace Application.Abstractions.Services
{
	public interface IStorageService
	{
		public Task<bool> PutAsync(string name, Stream fileStream, string contentType);
		public Task<string> GetUrlAsync(string name);
	}
}
