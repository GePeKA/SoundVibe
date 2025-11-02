using Application.Abstractions.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DataAccess.Repositories
{
	public class UserRepository(AppDbContext dbContext) : IUserRepository
	{
		public async Task<User> AddUserAsync(User user)
		{
			var entry = await dbContext.Users.AddAsync(user);

			return entry.Entity;
		}

		public async Task<User> AddUserFavouriteGenres(User user, List<long> genresIds)
		{
			var genres = await dbContext.Genres.Where(g => genresIds.Contains(g.Id)).ToListAsync();

			user.FavouriteGenres = genres;

			return user;
		}

		public User DeleteUser(User user)
		{
			var deletedUser = dbContext.Users.Remove(user);

			return deletedUser.Entity;
		}

		public async Task<User?> GetUserByFilterAsync(Expression<Func<User, bool>> filter)
		{
			return await dbContext.Users.SingleOrDefaultAsync(filter);
		}

		public async Task<bool> IsEmailUniqueAsync(string email)
		{
			return !await dbContext.Users.AnyAsync(u => u.Email == email);
		}
	}
}
