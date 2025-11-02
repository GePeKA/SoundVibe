using Application.Dtos;
using FluentValidation;

namespace Application.Validators
{
	public class SignUpDtoValidator: AbstractValidator<SignUpDto>
	{
		public SignUpDtoValidator() 
		{
			RuleFor(x => x.Nickname)
				.Length(4, 30).WithMessage("Длина никнейма - 4 - 30 символов");

			RuleFor(x => x.Email)
				.EmailAddress().WithMessage("Неправильный формат почты");

			RuleFor(x => x.Password)
				.NotNull()
				.NotEmpty().WithMessage("Пустой пароль")
				.Matches("^(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$;.,%^&*-]).{8,}$")
				.WithMessage("Минимум 8 символов, хотя бы одна строчная латинская буква, одна цифра и спец. символ");
		}
	}
}
