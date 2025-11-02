using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Extensions
{
	public static class SeedingDataExtension
	{
		public static void SeedWithInitialData(this ModelBuilder modelBuilder)
		{
			var initialGenres = new List<Genre>()
			{
				new() { Id = 1, Name = "Рэп" },
				new() { Id = 2, Name = "Рок" },
				new() { Id = 3, Name = "Альтернатива" },
				new() { Id = 4, Name = "Поп" },
				new() { Id = 5, Name = "Диско" },
				new() { Id = 6, Name = "Русский рэп" },
				new() { Id = 7, Name = "Нью-Джаз" },
				new() { Id = 8, Name = "Метал" },
				new() { Id = 9, Name = "Инди" },
				new() { Id = 10, Name = "Фонк"}
			};

			modelBuilder.Entity<Genre>().HasData(initialGenres);
		}
	}
}
