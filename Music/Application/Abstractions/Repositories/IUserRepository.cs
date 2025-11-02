using Domain.Entities;
using System.Linq.Expressions;

namespace Application.Abstractions.Repositories
{
	public interface IUserRepository
	{
		public Task<User> AddUserAsync(User user);

		public Task<User> AddUserFavouriteGenres(User user, List<long> genresIds);

		public Task<User?> GetUserByFilterAsync(Expression<Func<User, bool>> filter);

		public Task<bool> IsEmailUniqueAsync(string email);

		public User DeleteUser(User user);
	}
}
