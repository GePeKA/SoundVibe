using Application.Dtos;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
	public static class ValidatorsRegisterExt
	{
		public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddScoped<IValidator<SignUpDto>, SignUpDtoValidator>();

			return serviceCollection;
		}
	}
}
