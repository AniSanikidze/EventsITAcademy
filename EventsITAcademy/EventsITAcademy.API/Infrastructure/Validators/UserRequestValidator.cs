using EventsITAcademy.API.Infrastructure.Localizations;
using EventsITAcademy.Application.Users.Requests;
using FluentValidation;

namespace EventsITAcademy.API.Infrastructure.Validators
{
    public class UserRequestValidator : AbstractValidator<CreateUserRequestModel>
    {
        public UserRequestValidator()
        {
            RuleFor(x => x.Email)
                .Matches(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")
                .WithMessage(ValidationErrorMessages.InvalidEmail)
                .NotEmpty()
                .WithMessage(ValidationErrorMessages.EmailRequired);

            RuleFor(x => x.UserName)
                .Must(x => x.Length > 0 && x.Length <= 30)
                .WithMessage(ValidationErrorMessages.UsernameLength);

            RuleFor(x => x.Password)
                .Must(x => x.Length >= 6 && x.Length <= 30)
                .WithMessage(ValidationErrorMessages.PasswordLength);

        }
    }
}
