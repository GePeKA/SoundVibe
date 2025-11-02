using Application.Abstractions.Repositories;

namespace DataAccess.Repositories
{
	public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
	{
		public async Task SaveChangesAsync()
		{
			await dbContext.SaveChangesAsync();
		}
	}
}
