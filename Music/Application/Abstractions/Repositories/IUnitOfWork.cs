namespace Application.Abstractions.Repositories
{
	public interface IUnitOfWork
	{
		public Task SaveChangesAsync();
	}
}
